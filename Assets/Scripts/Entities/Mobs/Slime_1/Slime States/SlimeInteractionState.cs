using UnityEngine;

public class SlimeInteractionState : IState
{
  private SlimeInteractionBehaviour ourInteractionBehaviour;
  private Controller ourController;
  private Controller friendController;

  private Animator slimeAnimator;
  private Animator emoteAnimator;
  //private SpriteRenderer emoteSprite;
  private Transform ourTransform;
  private Transform friendTransform;
  private Vector3 interactionLocation;
  private Vector3 locationDirection;

  private float locationDistance;
  private float _refVelocity;
  private float conversationTime;
  private float idleBeforeInteraction;

  private float timeTillEmote;
  private bool canEmote;
  private bool isEmoting;

  public SlimeInteractionState(Controller us, Controller friend, SlimeInteractionBehaviour slimeBehaviour)
  {
    ourController = us;
    friendController = friend;

    ourInteractionBehaviour = slimeBehaviour;

    slimeAnimator = ourController.animator;
    emoteAnimator = ourInteractionBehaviour.emoteAnimator;

    isEmoting = false;
  }

  public void Entry()
  {
    ourInteractionBehaviour.DisableEmote();

    //Making sure were in the correct animation state
    slimeAnimator.SetBool("isJumping", false);
    slimeAnimator.SetBool("isRunning", false);

    //Random chance to use an emote bubble
    canEmote = Random.Range(0, 3) == 0 ? true : false;

    if (canEmote)
    {
      int num = Random.Range(1, 3);
      emoteAnimator.SetInteger("randomNum", num);

      timeTillEmote = Random.Range(5f, 7f);
    }

    ourTransform = ourController.transform;
    friendTransform = friendController.transform;

    //Calculating the distance between the two characters
    interactionLocation = (friendTransform.position + ourTransform.position) / 2;

    //Setting up our behaviour
    ourInteractionBehaviour.isInteracting = true;
    ourInteractionBehaviour.interactionTendency = 0;
    conversationTime = Random.Range(15f, 20f);
    idleBeforeInteraction = Random.Range(0.5f, 5f);
  }

  public void Update()
  { 
    //Keep constant track of the distance between the two characters
    //and the interaction location relative to us
    locationDirection = (interactionLocation - ourTransform.position);
    locationDistance = Vector3.Distance(ourTransform.position, interactionLocation);

    if(idleBeforeInteraction > 0) idleBeforeInteraction -= Time.deltaTime;

    if (conversationTime < 0)
    {
      ourInteractionBehaviour.isInteracting = false;

      //Maybe we jump out of convo, maybe we idle
      switch (Random.Range(0, 1))
      {
        case 0:
          ourController.stateManager.ChangeState(new SlimeIdleState(ourController));
          break;
        case 1:      
          ourController.stateManager.ChangeState(new SlimeJumpState(ourController));
          break;
      }
    }
  }

  public void FixedUpdate()
  {
    //If we are at the location, we can stop moving and
    //start the conversation and emoting
    if (locationDistance <= ourInteractionBehaviour.interactionSize) 
    { 
      conversationTime -= Time.deltaTime;

      if (canEmote)
      {
        if (timeTillEmote > 0) timeTillEmote -= Time.deltaTime;
        else
        {
          //One frame trigger to slimeAnimator
          if (isEmoting == true) return;
          isEmoting = true;
          Debug.Log("Enabling emote");
          ourInteractionBehaviour.EnableEmote();
          emoteAnimator.SetTrigger("startEmote");
        }
      }

      return; 
    }

    //This is the short delay the slime has before moving to location
    if (idleBeforeInteraction > 0) return; 

    ourController.rigid2D.AddForce
    (
      locationDirection * Mathf.SmoothDamp(0.6f, ourController.walkSpeed * 2, ref _refVelocity, 0.05f),
      ForceMode2D.Force
    );
  }

  public void Exit()
  {
    ourInteractionBehaviour.DisableEmote();
  }
}