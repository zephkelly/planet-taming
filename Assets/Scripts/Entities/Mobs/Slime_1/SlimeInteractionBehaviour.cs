using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInteractionBehaviour : MonoBehaviour
{
  private Controller controller;
  private SlimeStats slimeStats;

  public Animator emoteAnimator;

  private LayerMask slimeLayerMask;
  public int interactionTendency;
  public float interactionTendencyTimer;
  public bool isInteracting = false;

  public float interactionSize = 0.6f;

  public void Awake()
  {
    //Need the emote animator so the slimeinteractstate can access it
    if (emoteAnimator == null) emoteAnimator = GameObject.FindGameObjectWithTag("Emote").GetComponent<Animator>();

    controller = gameObject.GetComponent<Controller>();
    slimeStats = gameObject.GetComponent<SlimeStats>();
  }

  public void Start()
  {
    interactionTendencyTimer = 0;

    slimeLayerMask = 1 << LayerMask.NameToLayer("Slime");
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
      Vector2.zero, 0f, slimeLayerMask
    );

    for (int i = 0; i < hit.Length; i++) //Searching through hit[]
    {
      if (hit[i].collider.gameObject == this.gameObject) return; //If object is ourself ignore

      //If other slime wants to interact as well then invoke interact state on both
      if (hit[i].collider.GetComponent<SlimeInteractionBehaviour>().interactionTendency == 1)
      {
        controller.stateManager.ChangeState
          (new SlimeInteractionState(controller, hit[i].collider.GetComponent<Controller>()));
        hit[i].collider.GetComponent<Controller>().stateManager.ChangeState
          (new SlimeInteractionState(hit[i].collider.GetComponent<Controller>(), controller));

        break;
      }
    }
  }

  public void UpdateInteraction()
  {
    interactionTendency = Random.Range(0, Random.Range(8, 13));
    interactionTendencyTimer = 5f;
  }
}
