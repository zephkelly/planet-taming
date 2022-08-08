using UnityEngine;

public class SlimeInteractionState : IState
{
  private Controller ourController;
  private Controller friendController;
  private SlimeInteractionBehaviour ourBehaviour;

  private Transform ourTransform;
  private Transform friendTransform;

  private Vector3 interactionLocation;
  private Vector3 locationDirection;
  private float locationDistance;
  private float _refVelocity;

  private float conversationTime;
  private float idleTime;

  public SlimeInteractionState(Controller us, Controller friend)
  {
    ourController = us;
    friendController = friend;

    ourBehaviour = ourController.gameObject.GetComponent<SlimeInteractionBehaviour>();
  }

  public void Entry()
  {
    ourTransform = ourController.transform;
    friendTransform = friendController.transform;

    interactionLocation = (friendTransform.position + ourTransform.position) / 2;

    ourBehaviour.isInteracting = true;
    ourBehaviour.interactionTendency = 0;

    conversationTime = Random.Range(15f, 20f);
    idleTime = Random.Range(0.5f, 5f);
  }

  public void Update()
  { 
    locationDirection = (interactionLocation - ourTransform.position);
    locationDistance = Vector3.Distance(ourTransform.position, interactionLocation);

    if(idleTime > 0) idleTime -= Time.deltaTime;

    if (conversationTime < 0)
    {
      int num = Random.Range(0, 1);

      ourBehaviour.isInteracting = false;

      switch (num)
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
    if (locationDistance <= ourBehaviour.interactionSize) 
    { 
      conversationTime -= Time.deltaTime;
      return; 
    }

    if (idleTime > 0) return;

    ourController.rigid2D.AddForce(locationDirection * Mathf.SmoothDamp(0.6f, ourController.walkSpeed * 2, ref _refVelocity, 0.05f), ForceMode2D.Force);
  }

  public void Exit()
  {
  }
}