using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform spawnPoints, roomSpawn, player;
    public int spawnPointID;
    public static int playerEquipID, enemyID;
    public GameObject playerPrefab, enemyPrefab, keyItem, toolbox;

    public static event Action SpawninRoom;
    public static event Action SpawninLevel;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

       
        
    }
    private void OnEnable()
    {
        SpawninRoom += SpawnPlayerInRoom;
        SpawninLevel += SpawnPlayerOnMap;
        


    }

    private void OnDisable()
    {
        SpawninRoom = null;
        SpawninLevel = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawninRoom();
    
    }
    // Update is called once per frame
    void Update()
    {
        if(ExitRoomTrigger.isReadytoPlay)
        {
            SpawninLevel();
        }

        if(PlayerKiller.killPlayer)
        {
            PlayerDeath();
            
        }
    }

    void SpawnPlayerInRoom()
    {
        StartCoroutine(RoomSpawnCoroutine());
    }
    IEnumerator RoomSpawnCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("RogueRoomScene");
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        roomSpawn = GameObject.Find("RoomSpawn").transform;
        Instantiate(playerPrefab, roomSpawn.position + new Vector3 (0,5,0), Quaternion.identity);
        player = FindObjectOfType<XROrigin>().transform;
        yield return null;
        SpawnToolbox();
        Debug.Log("main area loaded");
    }

    void SpawnPlayerOnMap()
    {
        StartCoroutine(MapSpawnCoroutine());
    }
    IEnumerator MapSpawnCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameLevelMain");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        spawnPoints = GameObject.Find("SpawnPoints").transform;
        spawnPointID = UnityEngine.Random.Range(0,spawnPoints.childCount);
        Instantiate(playerPrefab, spawnPoints.GetChild(spawnPointID).transform.position + new Vector3(0,5,0), Quaternion.identity);
        player = FindObjectOfType<XROrigin>().transform;
        yield return null;

        SpawnToolbox();
        
        Debug.Log("Game area loaded");
        yield return new WaitForSeconds(1);

        SpawnEnemies();
        SpawnKeyItem();
        
    }

    void SpawnEnemies()
    {
            for (int i = 0; i < spawnPoints.childCount; i++)
            {
                if (i != spawnPointID)
                {
                Instantiate(enemyPrefab, spawnPoints.GetChild(i).transform.position + new Vector3(0, 5, 0), Quaternion.identity);
                Debug.Log("Enemy spawned");
                }

            }
    }

    void SpawnKeyItem()
    {
        int n = UnityEngine.Random.Range(0, spawnPoints.childCount);
        if (n != spawnPointID)
        {
            Instantiate(keyItem, spawnPoints.GetChild(n).transform.position + new Vector3(0, 5, 0), Quaternion.identity);
        }
        else
        {
            Debug.Log("same spawn point skipped");
            SpawnKeyItem();

        }
    }

    void SpawnToolbox()
    {
            Instantiate(toolbox, player);
            SonarManager.hitboxManager = FindObjectOfType<HitboxManager>();
    }
    
    void PlayerDeath()
    {
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        player.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        player.GetComponentInChildren<Canvas>().enabled= true;
        PlayerKiller.killPlayer = false;
        

        yield return new WaitForSeconds(5);
        
        SpawninRoom();
       
    }
}
