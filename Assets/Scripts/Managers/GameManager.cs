using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   
    public Transform spawnPoints, player;
    public int spawnPointID, lastFoundItemID, curFoundItemID;
    public static int enemyID;
    public List<int> playerEquipID = new();
    public GameObject playerPrefab;
    public ItemCycleManager cycleManager; 
    public List<GameObject> keyItem = new();
    public GameObject[] enemyPrefab;
    public AudioSource audioSource;

    //labyrinth code
    public List<GameObject> roomSpawnPoints, equipSpawnPoints;
    public float enemySpawnInterval;

    public InputActionReference restart;

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
            ItemCycleManager.StoreKeyItemList();
            restart.action.performed += RestartGame;
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
        /*if(ExitRoomTrigger.isReadytoPlay)
        {
            SpawnPlayerOnMap();
        }*/

        if(EnemyInteractionManager.killPlayer)
        {
            PlayerDeath();
            
        }
/*
        if (KeyItemReporter.itemFound)
        {
            KeyItemFound();
            KeyItemReporter.itemFound = false;
        }*/
    }

    #region Characters
    void SpawnPlayerInRoom()
    {
        roomSpawnPoints.Clear();
        equipSpawnPoints.Clear();
        StartCoroutine(ItemCycleManager.ResetKeyItemList());
        StartCoroutine(RoomSpawnCoroutine());
    }
    IEnumerator RoomSpawnCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LabyrinthGameScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        roomSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Respawn"));
        for(int j=0; j<roomSpawnPoints.Count;j++)
        {
            equipSpawnPoints.Add(roomSpawnPoints[j].transform.GetChild(0).gameObject);
        }
        int i = Random.Range(0,3);
        Instantiate(playerPrefab, roomSpawnPoints[i].transform.position + new Vector3(0, 2, 0), roomSpawnPoints[i].transform.localRotation);
        player = FindObjectOfType<XROrigin>().transform;
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

        StartCoroutine(SpawnPlayerEquip(i));
        StartCoroutine(MapSpawnCoroutine());

        yield return null;
       
    }

    void SpawnPlayerOnMap()
    {
        StartCoroutine(MapSpawnCoroutine());
    }
    IEnumerator MapSpawnCoroutine()
    {
       
        yield return new WaitForSeconds(1);
        spawnPoints = GameObject.Find("Monster spawn point").transform;

        //AudioManager.instance.CheckBGMToPlay();
        
        yield return null;
        
        Debug.Log("Game area loaded");
        
        yield return new WaitForSeconds(3);

       
        StartCoroutine(SummonPrizeEquip());
        StartCoroutine(SpawnEnemies());


    }

    IEnumerator SpawnEnemies()
    {
        //UnityEngine.Random.Range(1, 3)

        for (int i = 0; i < 1; i++)
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

    #endregion

    #region Toys
    IEnumerator SpawnPlayerEquip(int i)
    {
        yield return new WaitForSeconds(1);

        int equipIDToSummon = Random.Range(0, playerEquipID.Count);

        Instantiate(keyItem[playerEquipID[equipIDToSummon]], equipSpawnPoints[i].transform.position, Quaternion.identity);
        ItemCycleManager.RemoveToyPrefabInGameManager(playerEquipID[equipIDToSummon]);
        equipSpawnPoints.Remove(equipSpawnPoints[i]);

        yield return new WaitForSeconds(1);

        StartCoroutine(SpawnKeyItem());
    }

    IEnumerator SpawnKeyItem()
    {
        List<int> usedSpawnIDs = new();
        for(int i =0; i<= equipSpawnPoints.Count; i++)
        {
            int n = Random.Range(0, equipSpawnPoints.Count);
                while (usedSpawnIDs.Contains(n)&& equipSpawnPoints.Count > 1)
                {
                    Debug.Log("same spawn point skipped");
                    n = Random.Range(0, equipSpawnPoints.Count);

                }
           
            int j= Random.Range(0,keyItem.Count);
            Instantiate(keyItem[j], equipSpawnPoints[n].transform.position, Quaternion.identity);
            ItemCycleManager.RemoveToyPrefabInGameManager(j);
            equipSpawnPoints.Remove(equipSpawnPoints[n]);
            usedSpawnIDs.Add(n);

        }
        yield return null;
     
    }

    IEnumerator SummonPrizeEquip()
    {
        Transform prizeSpawn = GameObject.Find("PrizeSpawn").transform;
        yield return new WaitForSeconds(1);
        Instantiate(keyItem[0], prizeSpawn.position, Quaternion.identity);
        ItemCycleManager.RemoveToyPrefabInGameManager(0);
    }


    void KeyItemFound() //player gets key item, spawns in more enemies
    {
        Debug.Log("Key item found");

        StartCoroutine(UpdateToyFoundMessage());
        
    }

    IEnumerator UpdateToyFoundMessage()
    {
        audioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[0]);
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        player.GetComponentInChildren<TextMeshProUGUI>().text = "Toys Found\n" + ItemCycleManager.itemIDList.Count + "/4";
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
        player.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        player.GetComponentInChildren<TextMeshProUGUI>().text = "You are Dead";

        EnemyInteractionManager.killPlayer = false;
        yield return new WaitForSeconds(5);

        SpawnPlayerInRoom();
        
    }
    #endregion

    #region Utils
    void RestartGame(InputAction.CallbackContext obj)
    {
        if(obj.ReadValue<float>()==1)
        {
            SpawnPlayerInRoom();
        }
    }
    #endregion
}
