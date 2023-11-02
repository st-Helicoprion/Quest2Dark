using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   
    public Transform spawnPoints, roomSpawn, player;
    public GameObject[] toolboxSpawn;
    public int spawnPointID, lastFoundItemID, curFoundItemID;
    public static int enemyID;
    public int[] playerEquipID;
    public GameObject playerPrefab, toolbox;
    public List<GameObject> keyItem = new();
    public GameObject[] enemyPrefab;
    

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            
        }
        else
        {
            instance = this;
            ToolboxManager.StoreKeyItemList();
            DontDestroyOnLoad(gameObject);
        }

          
    }

    // Start is called before the first frame update
    void Start()
    {
       SpawnPlayerInRoom();
      
    }
    // Update is called once per frame
    void Update()
    {
        if(ExitRoomTrigger.isReadytoPlay)
        {
            SpawnPlayerOnMap();
        }

        if(InteractionManager.killPlayer)
        {
            PlayerDeath();
            
        }

        if(KeyItemReporter.itemFound)
        {
            KeyItemFound();
            KeyItemReporter.itemFound = false;
        }
    }

    #region Characters
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
        Instantiate(playerPrefab, roomSpawn.position + new Vector3(0, 5, 0), Quaternion.identity);
        player = FindObjectOfType<XROrigin>().transform;
        toolboxSpawn = GameObject.FindGameObjectsWithTag("ToySpawn");
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

        StartCoroutine(ToolboxManager.ResetKeyItemList());
        StartCoroutine(SpawnPlayerEquip());

        yield return null;
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
        enemyID = -1;
        spawnPoints = GameObject.Find("SpawnPoints").transform;
        spawnPointID = UnityEngine.Random.Range(0,spawnPoints.childCount-1);

        //spawn player into map
        Instantiate(playerPrefab, spawnPoints.GetChild(spawnPointID).position + new Vector3(0, 5, 0), Quaternion.identity);
        player = FindObjectOfType<XROrigin>().transform;
        toolboxSpawn = GameObject.FindGameObjectsWithTag("ToySpawn");

        //remove player spawn point
        Destroy(spawnPoints.GetChild(spawnPointID).gameObject);

        StartCoroutine(SpawnPlayerEquip());
        
        yield return null;
        
        Debug.Log("Game area loaded");
        
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        player.GetComponentInChildren<TextMeshProUGUI>().text = "Find Toys \n 0/3";
        yield return new WaitForSeconds(3);

        player.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        StartCoroutine(SpawnKeyItem());
        
    }
    IEnumerator SpawnEnemies()
    {
        //UnityEngine.Random.Range(1, 3)
            for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
            {
                Debug.Log("enemyID is : " + enemyID);
                Instantiate(enemyPrefab[enemyID], spawnPoints.GetChild(UnityEngine.Random.Range(0,spawnPoints.childCount-1)).transform.position + new Vector3(0, 5, 0), Quaternion.identity);
                Debug.Log("Enemy spawned");
      
            }
        yield return null;
            StartCoroutine(HopterManager.FindEnemies());
    }
    #endregion

    #region Toys
    IEnumerator SpawnPlayerEquip()
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i< playerEquipID.Length; i++)
        {
            Instantiate(keyItem[playerEquipID[i]], toolboxSpawn[i].transform.position, Quaternion.identity);

        }
        BindKeyItemToManagers();
    }
    IEnumerator SpawnKeyItem()
    {
        List<int> usedSpawnIDs = new List<int>();
        for(int i =0; i<keyItem.Count; i++)
        {
            int n = UnityEngine.Random.Range(0, spawnPoints.childCount-1);
            while (usedSpawnIDs.Contains(n))
            {
                Debug.Log("same spawn point skipped");
                n = UnityEngine.Random.Range(0, spawnPoints.childCount-1);

            }

            Instantiate(keyItem[i], spawnPoints.GetChild(n).transform.position + new Vector3(0, 5, 0), Quaternion.identity);
            usedSpawnIDs.Add(n);

        }
        yield return null;
        Debug.Log("Ready to bind key items");
        BindKeyItemToManagers();
     
    }

    void BindKeyItemToManagers()
    {
        HopterManager.CheckForHopter();
        SonarManager.CheckForHitBoxManager();
    }

    void KeyItemFound() //player gets key item, spawns in more enemies
    {
        Debug.Log("Key item found");
        
        enemyID++;
        if (enemyID < enemyPrefab.Length)
        {
            StartCoroutine(SpawnEnemies());
        }
        else 
        {
            enemyID = 0;
            StartCoroutine(SpawnEnemies());

        }
    }
    #endregion

    #region Win
        void CheckWinCondition()
    {

    }
    void WinConditionReached()
    {

    }

    IEnumerator WinCoroutine()
    {
        yield return null;
    }
    #endregion

    #region Death
    void PlayerDeath()
    {
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        Debug.Log("death coroutine test");
        player.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        player.GetComponentInChildren<TextMeshProUGUI>().text = "You are Dead";
        
        InteractionManager.killPlayer = false;
        yield return new WaitForSeconds(5);

        SpawnPlayerInRoom();
        
    }
    #endregion
}
