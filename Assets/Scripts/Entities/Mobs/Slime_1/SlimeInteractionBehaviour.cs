using UnityEngine;

public class SlimeInteractionBehaviour : MonoBehaviour
{
  private SlimeController slimeController;
  private Controller controller;
  private SlimeStats slimeStats;

  public Animator emoteAnimator; //Ref by slimeinteractionstate
  private Transform ourTransform;
  private SpriteRenderer emoteSprite;
  private LayerMask entitiesLayerMask;

  public int interactionTendency;
  public float timeTillInteractionUpdate;
  public bool isInteracting = false;

  public float interactionSize = 0.6f;

  public void Awake()
  {
    //Need the emote animator so the slimeinteractstate can access it
    if (emoteAnimator == null) emoteAnimator = GameObject.FindGameObjectWithTag("Emote").GetComponent<Animator>();

    emoteSprite = emoteAnimator.GetComponent<SpriteRenderer>();

    slimeController = gameObject.GetComponent<SlimeController>();
    controller = gameObject.GetComponent<Controller>();
    slimeStats = gameObject.GetComponent<SlimeStats>();
  }

  public void Start()
  {
    timeTillInteractionUpdate = 0;

    DisableEmote();

    entitiesLayerMask = 1 << LayerMask.NameToLayer("Entity");
  }

  public void Update()
  {
    //If we are already interacting, return
    if (isInteracting) return;

    //Interaction loop
    if (timeTillInteractionUpdate <= 0) UpdateInteraction();
    timeTillInteractionUpdate -= Time.deltaTime;

    //Only raycast if we want to interact
    if (interactionTendency != 1) return;

    RaycastHit2D[] hit = Physics2D.CircleCastAll(
        transform.position,
        Random.Range(2f, 5f),
        Vector2.zero, 0f,
        entitiesLayerMask);

    for (int i = 0; i < hit.Length; i++) //Searching through hit[]
    {
      GameObject hitGameObject = hit[i].collider.gameObject;

      if (!hitGameObject.CompareTag("Slime")) return;
      if (hitGameObject == this.gameObject) return;

      hitGameObject.TryGetComponent(out SlimeInteractionBehaviour sIB);
      SlimeInteractionBehaviour otherSlimeBehaviour = sIB;

      //If other slime wants to interact as well then invoke interact state on both
      if (otherSlimeBehaviour.interactionTendency == 1)
      {
        Controller otherController = hit[i].collider.GetComponent<Controller>();

        controller.stateManager.ChangeState(new SlimeInteractionState(controller, slimeController, otherController, this));
        otherController.stateManager.ChangeState(new SlimeInteractionState(otherController, 
                                                                           otherController.GetComponent<SlimeController>(),
                                                                           controller,
                                                                           otherController.GetComponent<SlimeInteractionBehaviour>()));

        break;
      }
    }
  }

  public void UpdateInteraction()
  {
    interactionTendency = Random.Range(0, Random.Range(8, 13));
    timeTillInteractionUpdate = 5f;
  }

  public void EnableEmote() => emoteSprite.enabled = true;
  public void DisableEmote() => emoteSprite.enabled = false;
}