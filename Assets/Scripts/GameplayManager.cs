using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
  public GameObject cameraMain;
  public GameObject playerPrefab; 
    public Transform playerSpawnPoints;
    private GameObject playerCurrent;

  public void Start()
    {
        playerCurrent = GameObject.FindGameObjectWithTag("Player");
        cameraMain = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void Update()
    {
    }

    public void RespawnPlayer(GameObject player)
    {
        StartCoroutine(DeathWait(player));
        Debug.Log("Player respawned at: " + playerSpawnPoints.position);
    }

    IEnumerator DeathWait(GameObject player)
    {
        Destroy(player);

        yield return new WaitForSeconds(3f);

        playerCurrent = Instantiate(playerPrefab, playerSpawnPoints.position, Quaternion.identity);
        
        cameraMain.GetComponent<CameraFollow>().ChangeFocus(playerCurrent.transform);
    }
}