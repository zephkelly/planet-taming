using UnityEngine;

public class SlimeRunState : IState
{
    private Controller controller;
    private Animator animator;
    private Transform attackingEntity;
    private Vector3 runDirection;

    private float runTime;
    private float timeTillJump;
    private float moveImpluseStrength;

    public SlimeRunState(Controller c, Transform t)
    {
      controller = c;
      attackingEntity = t;

      animator = controller.GetComponent<Animator>();
    }

    public void Entry()
    {
      animator.SetBool("isRunning", true);

      timeTillJump = 0f;
      runTime = Random.Range(11f, 13f);

      runDirection = (controller.transform.position - attackingEntity.position).normalized;
    }

    public void Update()
    {
      while (runTime > 0)
      {
        runTime -= Time.deltaTime;
        timeTillJump -= Time.deltaTime;

        if (timeTillJump <= 0) Jump();

      return;
      }

      animator.SetBool("isRunning", false);

      controller.stateManager.ChangeState(new SlimeIdleState(controller));
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }

    public void Jump()
    {
      timeTillJump = Random.Range(0.5f, 1.2f);
      moveImpluseStrength = Random.Range(12f, 14f);

      controller.rigid2D.AddForce(runDirection * moveImpluseStrength, ForceMode2D.Impulse);

      runDirection = (controller.transform.position - attackingEntity.position).normalized;
    }
}