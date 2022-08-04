using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
  [SerializeField] GameObject cameraMain;
  [SerializeField] GameObject playerPrefab;
  [SerializeField] Transform playerSpawnPoint;
  private GameObject playerCurrent;

  public void Start()
  {
    playerCurrent = GameObject.FindGameObjectWithTag("Player");
    cameraMain = GameObject.FindGameObjectWithTag("MainCamera");
  }

  public void RespawnPlayer(GameObject player)
  {
    StartCoroutine(DeathWait(player));

    IEnumerator DeathWait(GameObject player)
    {
      yield return new WaitForSeconds(3f);

      playerCurrent = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
      cameraMain.GetComponent<CameraFollow>().ChangeFocus(playerCurrent.transform);

      Debug.Log("Player respawned at: " + playerSpawnPoint.position);
    }
  }
}