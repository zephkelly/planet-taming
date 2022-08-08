using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInteractionBehaviour : MonoBehaviour
{
  private Controller controller;
  private SlimeStats slimeStats;

  private LayerMask slimeLayerMask;
  public int interactionTendency;
  public float interactionTendencyTimer;
  public bool isInteracting;

  public float interactionSize = 0.6f;

  public void Start()
  {
    interactionTendencyTimer = 0;
    controller = gameObject.GetComponent<Controller>();
    slimeStats = gameObject.GetComponent<SlimeStats>();

    slimeLayerMask = 1 << LayerMask.NameToLayer("Slime");

    isInteracting = false;
  }

  public void Update()
  {
    if (!isInteracting)
    {
      if (interactionTendencyTimer <= 0)
      {
        UpdateInteraction();
      }

      interactionTendencyTimer -= Time.deltaTime;

      if (interactionTendency == 1)
      {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 10f, Vector2.zero, 0f, slimeLayerMask);

        for (int i = 0; i < hit.Length; i++)
        {
          if (hit[i].collider.gameObject == this.gameObject) return;

          if (hit[i].collider.GetComponent<SlimeInteractionBehaviour>().interactionTendency == 1)
          {
            controller.stateManager.ChangeState(new SlimeInteractionState(controller, hit[i].collider.GetComponent<Controller>()));
            hit[i].collider.GetComponent<Controller>().stateManager.ChangeState(new SlimeInteractionState(hit[i].collider.GetComponent<Controller>(), controller));
            
            break;
          }
        }
      }
    }
  }

  public void UpdateInteraction()
  {
    interactionTendency = Random.Range(0, Random.Range(8, 13));
    interactionTendencyTimer = 5f;
  }
}
