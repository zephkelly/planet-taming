using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
  [SerializeField] GameObject cameraMain;
  private CameraController mainCamera;

  [SerializeField] GameObject playerPrefab;
  [SerializeField] Transform playerSpawnPoint;
  private GameObject playerCurrent;

  [SerializeField] bool spawnSlimes;
  [SerializeField] GameObject slimePrefab;
  [SerializeField] Transform[] slimeSpawnZones;
  [SerializeField] float slimeZoneRadius = 6;
  private List<GameObject> slimeInstances = new List<GameObject>();
  
  [SerializeField] int slimeCount = 6;


  public void Awake()
  {
    mainCamera = cameraMain.GetComponent<CameraController>();
  }

  public void Start()
  {
    playerCurrent = GameObject.FindGameObjectWithTag("Player");
    cameraMain = GameObject.FindGameObjectWithTag("MainCamera");

    if (spawnSlimes) PopulateSlimeZones();
  }

  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
  }

  public void PopulateSlimeZones()
  {
    foreach (Transform spawnZone in slimeSpawnZones)
    {
      for (int i = 0; i < slimeCount; i++)
      {
        Vector3 randomPositionInZone = new Vector3(
            Random.Range(spawnZone.position.x - slimeZoneRadius, spawnZone.position.x + slimeZoneRadius),
            Random.Range(spawnZone.position.y - slimeZoneRadius, spawnZone.position.y + slimeZoneRadius),
            spawnZone.position.z);

        GameObject slime = Instantiate(slimePrefab, randomPositionInZone, Quaternion.identity);
        slimeInstances.Add(slime);
      }
    }
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