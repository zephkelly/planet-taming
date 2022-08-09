using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
  [SerializeField] GameObject cameraMain;
  [SerializeField] GameObject playerPrefab;
  [SerializeField] Transform playerSpawnPoint;
  private GameObject playerCurrent;

  private CameraController mainCamera;

  public void Awake()
  {
    mainCamera = cameraMain.GetComponent<CameraController>();
  }

  public void Start()
  {
    playerCurrent = GameObject.FindGameObjectWithTag("Player");
    cameraMain = GameObject.FindGameObjectWithTag("MainCamera");
  }

  public void RespawnPlayer()
  {
    StartCoroutine(DeathWait());

    IEnumerator DeathWait()
    {
      yield return new WaitForSeconds(3f);

      playerCurrent = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
      mainCamera.ChangeFocus(playerCurrent.transform);

      Debug.Log("Player respawned at: " + playerSpawnPoint.position);
    }
  }
}