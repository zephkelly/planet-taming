using UnityEngine;

public class SlimeRunState : IState
{
    private EnemyController controller;
    private Transform attackingEntity;
    private Vector3 runDirection;

    private float runTime;
    private float timeTillJump;
    private float moveImpluseStrength;

    public SlimeRunState(EnemyController c, Transform t)
    {
      controller = c;
      attackingEntity = t;
    }

    public void Entry()
    {
      timeTillJump = 0f;
      runTime = Random.Range(10f, 18f);

      runDirection = (controller.transform.position - attackingEntity.position).normalized;
    }

    public void Update()
    {
      while (runTime > 0)
      {
        runTime -= Time.deltaTime;
        timeTillJump -= Time.deltaTime;

        if (timeTillJump > 0) return;
        Jump();

        return;
      }

      controller.stateMachine.ChangeState(new SlimeIdleState(controller));
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }

    public void Jump()
    {
      timeTillJump = Random.Range(0.5f, 1f);
      moveImpluseStrength = Random.Range(12f, 14f);

      controller.enemyRigidbody.AddForce(runDirection * moveImpluseStrength, ForceMode2D.Impulse);

      runDirection = (controller.transform.position - attackingEntity.position).normalized;
    }
}