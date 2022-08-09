using UnityEngine;

public class SlimeInteractionState : IState
{
  private SlimeInteractionBehaviour ourBehaviour;
  private Controller ourController;
  private Controller friendController;

  private SpriteRenderer emoteBubbleSprite;
  private Animator animator;
  private Animator emoteBubble;

  private Transform ourTransform;
  private Transform friendTransform;

  private Vector3 interactionLocation;
  private Vector3 locationDirection;
  private float locationDistance;
  private float _refVelocity;

  private float conversationTime;
  private float timeTillEmote;
  private float idleTime;

  private bool canEmote;
  private bool isEmoting;

  public SlimeInteractionState(Controller us, Controller friend)
  {
    ourController = us;
    friendController = friend;

    ourBehaviour = ourController.GetComponent<SlimeInteractionBehaviour>();
    animator = ourController.GetComponent<Animator>();
    
    emoteBubble = ourBehaviour.emoteAnimator;
    emoteBubbleSprite = emoteBubble.GetComponent<SpriteRenderer>();

    emoteBubbleSprite.enabled = false;
    isEmoting = false;
    //emoteBubble.gameObject.SetActive(false);
  }

  public void Entry()
  {
    //Making sure were in the correct animation state
    animator.SetBool("isJumping", false);
    animator.SetBool("isRunning", false);

    //Random chance to use an emote bubble
    canEmote = Random.Range(0, 3) == 0 ? true : false;

    if (canEmote)
    {
      int num = Random.Range(1, 3);
      emoteBubble.SetInteger("randomNum", num);

      timeTillEmote = Random.Range(5f, 7f);
    }

    ourTransform = ourController.transform;
    friendTransform = friendController.transform;

    //Calculating the distance between the two characters
    interactionLocation = (friendTransform.position + ourTransform.position) / 2;

    //Setting up our behaviour
    ourBehaviour.isInteracting = true;
    ourBehaviour.interactionTendency = 0;
    conversationTime = Random.Range(15f, 20f);
    idleTime = Random.Range(0.5f, 5f);
  }

  public void Update()
  { 
    //Keep constant track of the distance between the two characters
    //and the interaction location relative to us
    locationDirection = (interactionLocation - ourTransform.position);
    locationDistance = Vector3.Distance(ourTransform.position, interactionLocation);

    if(idleTime > 0) idleTime -= Time.deltaTime;

    if (conversationTime < 0)
    {
      ourBehaviour.isInteracting = false;

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
    if (locationDistance <= ourBehaviour.interactionSize) 
    { 
      conversationTime -= Time.deltaTime;

      if (canEmote)
      {
        if (timeTillEmote > 0) timeTillEmote -= Time.deltaTime;
        else
        {
          //One frame trigger to animator
          if (isEmoting == true) return;
          isEmoting = true;
          
          emoteBubbleSprite.enabled = true;
          emoteBubble.SetTrigger("startEmote");
        }
      }

      return; 
    }

    //This is the short delay the slime has before moving to location
    if (idleTime > 0) return; 

    ourController.rigid2D.AddForce(locationDirection * Mathf.SmoothDamp(0.6f, ourController.walkSpeed * 2, ref _refVelocity, 0.05f), ForceMode2D.Force);
  }

  public void Exit()
  {
    emoteBubbleSprite.enabled = false;
  }
}