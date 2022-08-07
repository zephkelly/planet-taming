using UnityEngine;

public class SlimeDeathState : IState
{
    private Controller controller;
    private SpriteRenderer spriteRenderer;

    public SlimeDeathState(Controller c, SpriteRenderer r)
    {
      controller = c;
      spriteRenderer = r;
    }

    public void Entry()
    {
      Debug.Log(controller.gameObject.tag + " is dead");
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