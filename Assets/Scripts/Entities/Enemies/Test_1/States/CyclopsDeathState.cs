using UnityEngine;

public class CyclopsDeathState : IState
{
    private Controller controller;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public CyclopsDeathState(Controller c, SpriteRenderer r)
    {
      controller = c;
      spriteRenderer = r;
    }

    public void Entry()
    {
      controller.animator.SetBool("isDead", true);
      
      //controller.GetComponent<Collider2D>().enabled = false;
      spriteRenderer.color = Color.red;

      controller.statsManager.Die(controller.gameObject);
    }

    public void Update() { controller.rigid2D.velocity = Vector2.zero; }
    public void FixedUpdate() { }
    public void Exit() { }
}