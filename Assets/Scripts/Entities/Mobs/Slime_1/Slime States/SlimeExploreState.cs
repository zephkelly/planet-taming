using UnityEngine;

public class SlimeExploreState : IState
{
  private SlimeController slimeController;
  private Controller controller;
  private Transform slimeTransform;

  private Vector2 exploreDirection;
  private LayerMask colliderLayerMask;

  private float exploreTimer;
  private float collisionRayLength = 0.5f;

  public SlimeExploreState(Controller c)
  {
    controller = c;
    slimeController = c.GetComponent<SlimeController>();

    colliderLayerMask = 1 << LayerMask.NameToLayer("Collidable");
  }

  public void Entry()
  {
    slimeTransform = controller.transform;
    exploreTimer = slimeController.ExploreLength;

    exploreDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
  }

  public void Update()
  {
    UpdateState();

    FireCircleCast();
  }


  public void FixedUpdate()
  {
    controller.rigid2D.AddForce(exploreDirection * controller.walkSpeed, ForceMode2D.Force);
  }

  public void Exit()
  {

  }

  private void FireCircleCast()
  {
    RaycastHit2D[] hits = Physics2D.CircleCastAll(slimeTransform.position, collisionRayLength, Vector2.zero, 0f, colliderLayerMask);

    for (int i = 0; i < hits.Length; i++)
    {
      RaycastHit2D hit = hits[i];
      Collider2D hitCollider = hit.collider;

      Vector2 normal = hit.normal;
      Vector2 perpendicular = Vector2.Perpendicular(normal);
      float dot = Vector2.Dot(exploreDirection, normal); //Dot is how much we are looking at a wall

      //If we are veering towards wall...
      if (dot < 0)
      {
        exploreDirection = Vector2.Reflect(exploreDirection, normal);
      }
      
      //If we are looking at wall...
      if (dot < -0.8f)
      {
        Vector2 _ref = Vector2.zero;
        exploreDirection = Vector2.SmoothDamp(exploreDirection, perpendicular, ref _ref, 0.3f);
      }
    }
  }

  private void UpdateState()
  {
    while (exploreTimer > 0)
    {
      exploreTimer -= Time.deltaTime;
      return;
    }

    controller.stateManager.ChangeState(new SlimeIdleState(controller));
  }
}