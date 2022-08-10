using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInteractionBehaviour : MonoBehaviour
{
  private Controller controller;
  private SlimeStats slimeStats;

  private Transform ourTransform;

  public Animator emoteAnimator; //Ref by slimeinteractionstate
  private SpriteRenderer emoteSprite;

  private LayerMask slimeRaycastMask;
  public int interactionTendency;
  public float interactionTendencyTimer;
  public bool isInteracting = false;

  public float interactionSize = 0.6f;

  public void Awake()
  {
    //Need the emote animator so the slimeinteractstate can access it
    if (emoteAnimator == null) emoteAnimator = GameObject.FindGameObjectWithTag("Emote").GetComponent<Animator>();

    emoteSprite = emoteAnimator.GetComponent<SpriteRenderer>();

    controller = gameObject.GetComponent<Controller>();
    slimeStats = gameObject.GetComponent<SlimeStats>();
  }

  public void Start()
  {
    interactionTendencyTimer = 0;

    DisableEmote();

    slimeRaycastMask = 1 << LayerMask.NameToLayer("Slime");
  }

  public void Update()
  {
    //If we are already interacting, return
    if (isInteracting) return;

    //Interaction loop
    interactionTendencyTimer -= Time.deltaTime;
    if (interactionTendencyTimer <= 0) UpdateInteraction();

    //Only raycast if we wat to interact
    if (interactionTendency != 1) return;

    RaycastHit2D[] hit = Physics2D.CircleCastAll
    (
      transform.position,
      Random.Range(2f, 5f),
      Vector2.zero, 0f, slimeRaycastMask
    );

    for (int i = 0; i < hit.Length; i++) //Searching through hit[]
    {
      GameObject hitGameObject = hit[i].collider.gameObject;

      //Check if object is a slime, and also if we arent hitting ourselves...
      if (hitGameObject.tag != "Slime" || hitGameObject == this.gameObject) return;

      //If other slime wants to interact as well then invoke interact state on both
      if (hitGameObject.GetComponent<SlimeInteractionBehaviour>().interactionTendency == 1)
      {
        Controller otherController = hit[i].collider.GetComponent<Controller>();

        controller.stateManager.ChangeState(new SlimeInteractionState(controller, otherController, this));
        otherController.stateManager.ChangeState(new SlimeInteractionState(otherController, controller, this));

        break;
      }
    }
  }

  public void UpdateInteraction()
  {
    interactionTendency = Random.Range(0, Random.Range(8, 13));
    interactionTendencyTimer = 5f;
  }

  public void EnableEmote() => emoteSprite.enabled = true;
  public void DisableEmote() => emoteSprite.enabled = false;
}