using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   
    public Transform spawnPoints, roomSpawn, player;
    public int spawnPointID, lastFoundItemID, curFoundItemID;
    public static int enemyID;
    public List<int> playerEquipID = new();
    public GameObject playerPrefab;
    //public List<KeyItemReporter> keyItem = new();
    public List<GameObject> keyItem = new();
    public GameObject[] equipSpawnPoints;
    public GameObject[] enemyPrefab;
    public AudioSource audioSource;

    //labyrinth code
    public GameObject labyrinthMap;
    public Vector3[] mapRotations, mapPositions;
    public float enemySpawnInterval;
    public NavMeshSurface mapAIPath;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            
        }
        else
        {
            instance = this;
            audioSource= GetComponent<AudioSource>();
            ToolboxManager.StoreKeyItemList();
            DontDestroyOnLoad(gameObject);
        }

          
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayerInRoom();
        //demo code
        //player = FindObjectOfType<XROrigin>().transform;

    }
    // Update is called once per frame
    void Update()
    {
        if(ExitRoomTrigger.isReadytoPlay)
        {
            SpawnPlayerOnMap();
        }

        if(EnemyInteractionManager.killPlayer)
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LabyrinthGameScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        roomSpawn = GameObject.Find("RoomSpawn").transform;
        Instantiate(playerPrefab, roomSpawn.position + new Vector3(0, 2, 0), Quaternion.identity);
        player = FindObjectOfType<XROrigin>().transform;
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

        labyrinthMap = GameObject.Find("MAZE");
        mapAIPath = GameObject.Find("LevelTerrain").GetComponent<NavMeshSurface>();
        labyrinthMap.transform.parent.gameObject.SetActive(false);

        AudioManager.instance.CheckBGMToPlay();
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
        RandomizeMapEntryPoint();
        yield return new WaitForSeconds(1);
        mapAIPath.BuildNavMesh();
        spawnPoints = GameObject.Find("SpawnPoints").transform;

        //AudioManager.instance.CheckBGMToPlay();
        
        yield return null;
        
        Debug.Log("Game area loaded");
        
        yield return new WaitForSeconds(3);

        
        StartCoroutine(SpawnKeyItem());
        StartCoroutine(SpawnEnemies());
        //ToolboxManager.instance.InitialHideToolbox();

    }

    void RandomizeMapEntryPoint()
    {
        int entryPointID = UnityEngine.Random.Range(0,mapRotations.Length);

        labyrinthMap.transform.parent.gameObject.SetActive(true);

        labyrinthMap.transform.localPosition = mapPositions[entryPointID];
        labyrinthMap.transform.eulerAngles = mapRotations[entryPointID];

        
    }
    IEnumerator SpawnEnemies()
    {
        //UnityEngine.Random.Range(1, 3)

        for (int i = 0; i < 3; i++)
        {
            enemyID = UnityEngine.Random.Range(0, enemyPrefab.Length);
            Debug.Log("enemyID is : " + enemyID);
            Instantiate(enemyPrefab[enemyID], spawnPoints.GetChild(UnityEngine.Random.Range(0,spawnPoints.childCount)).transform.position + new Vector3(0, 5, 0), Quaternion.identity);
            Debug.Log("Enemy spawned");
      
        }

        //temporary code for demo
        /*for (int i = 0; i < 1; i++)
        {
            Debug.Log("enemyID is : " + enemyID);
            Instantiate(enemyPrefab[enemyID + 1], spawnPoints.GetChild(UnityEngine.Random.Range(0, spawnPoints.childCount - 1)).transform.position + new Vector3(0, 5, 0), Quaternion.identity);
            Debug.Log("Demo enemy spawned");

        }*/

        yield return new WaitForSeconds(enemySpawnInterval);
        StartCoroutine(SpawnEnemies());
        
    }

    void CheckToSpawnEnemy()
    {
        float intervalReset = 120;
        enemySpawnInterval -= Time.deltaTime;
           
        if(enemySpawnInterval<0)
        {
            enemySpawnInterval = intervalReset;
            StartCoroutine(SpawnEnemies());
        }
    }
    #endregion

    #region Toys
    IEnumerator SpawnPlayerEquip()
    {
        yield return new WaitForSeconds(1);

        equipSpawnPoints = GameObject.FindGameObjectsWithTag("ToySpawn");

        for(int i = 0; i< playerEquipID.Count; i++)
        {
            Debug.Log("total player equip id :"+playerEquipID.Count);
            Debug.Log("total equip spwn points :" + equipSpawnPoints.Length);
            Instantiate(keyItem[playerEquipID[i]], equipSpawnPoints[i].transform.position, Quaternion.identity);
            Debug.Log("equipments loaded");
        }

      
    }

    IEnumerator SpawnKeyItem()
    {
        List<int> usedSpawnIDs = new List<int>();
        for(int i =0; i<keyItem.Count; i++)
        {
            int n = UnityEngine.Random.Range(0, spawnPoints.childCount);
            while (usedSpawnIDs.Contains(n))
            {
                Debug.Log("same spawn point skipped");
                n = UnityEngine.Random.Range(0, spawnPoints.childCount);

            }

            Instantiate(keyItem[i], spawnPoints.GetChild(n).transform.position + new Vector3(0, 5, 0), Quaternion.identity);
            usedSpawnIDs.Add(n);

        }
        yield return null;
        Debug.Log("Ready to bind key items");
        
       
    }


    void KeyItemFound() //player gets key item, spawns in more enemies
    {
        Debug.Log("Key item found");

        //StartCoroutine(UpdateToyFoundMessage());
        
    }

    IEnumerator UpdateToyFoundMessage()
    {
        audioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[0]);
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        player.GetComponentInChildren<TextMeshProUGUI>().text = "Toys Found\n" + ToolboxManager.itemIDList.Count + "/4";
        yield return new WaitForSeconds(3);
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
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
        
        EnemyInteractionManager.killPlayer = false;
        yield return new WaitForSeconds(5);

        SpawnPlayerInRoom();
        
    }
    #endregion
}
