using UnityEngine;

public class SlimeDeathState : IState
{
    private Controller controller;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public SlimeDeathState(Controller c, SpriteRenderer r)
    {
      controller = c;
      spriteRenderer = r;

      animator = controller.GetComponent<Animator>();
    }

    public void Entry()
    {
      animator.SetBool("isDead", true);
      
      controller.GetComponent<Collider2D>().enabled = false;
      spriteRenderer.color = Color.red;
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}