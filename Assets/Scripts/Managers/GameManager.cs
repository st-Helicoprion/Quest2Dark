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
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   
    public Transform spawnPoints, player;
    public int spawnPointID, lastFoundItemID, curFoundItemID, enemyWaveID, enemySummonID;
    public int[] enemyPattern;
    public List<int> playerEquipID = new();
    public GameObject playerPrefab;
    public ItemCycleManager cycleManager; 
    public List<GameObject> keyItem = new();
    public GameObject[] enemyPrefab;
    public AudioSource audioSource;
    public static bool readyToReboot, win;

    //labyrinth code
    public List<GameObject> roomSpawnPoints, equipSpawnPoints, equipHiders;
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
            //restart.action.performed += ManualRestartGame;
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

        if(win)
        {
            WinConditionReached();
        }

    }

    #region Characters
    void SpawnPlayerInRoom()
    {
        roomSpawnPoints.Clear();
        equipSpawnPoints.Clear();
        enemyWaveID = 0;
        enemySummonID= 0;
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
        player.gameObject.GetNamedChild("Canvas").transform.GetChild(1).GetComponent<TextMeshPro>().enabled = false;
        UIViewAligner.player = player.gameObject.GetNamedChild("Main Camera").transform;
        IntroSpawnReporter.player = UIViewAligner.player;
        PrizeBoxManager.taken = false;

      /*  if (Application.platform == RuntimePlatform.Android)
        {*/
            NetworkManager.Singleton.StartHost();
      /*  }*/

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
        spawnPoints = GameObject.Find("Monster spawn points").transform;

        //AudioManager.instance.CheckBGMToPlay();
        
        yield return null;
        
        Debug.Log("Game area loaded");
        
        yield return new WaitForSeconds(1);

        StartCoroutine(SummonPrizeEquip());
        StartCoroutine(SpawnEnemies());


    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(enemySpawnInterval);

        if (enemyWaveID < 4)
        {
            for (int i = 0; i < enemyPattern[enemyWaveID]; i++)
            {
                int rand = Random.Range(0, spawnPoints.childCount);
                Instantiate(enemyPrefab[enemySummonID], spawnPoints.GetChild(rand).position + new Vector3(0, 5, 0), Quaternion.identity);
                Debug.Log("Enemy spawned");
                enemySummonID++;
            }
            enemyWaveID++;

            yield return null;

            StartCoroutine(SpawnEnemies());
        }
       
    }

    #endregion

    #region Toys
    IEnumerator SpawnPlayerEquip(int i)
    {

        yield return new WaitForSeconds(1);

        int equipIDToSummon = Random.Range(0, playerEquipID.Count);

        GameObject playerToy = Instantiate(keyItem[playerEquipID[equipIDToSummon]], equipSpawnPoints[i].transform.position, Quaternion.identity);
        TutorialsManager.givenToy = playerToy;
        ItemCycleManager.RemoveToyPrefabInGameManager(playerEquipID[equipIDToSummon]);
        equipSpawnPoints.Remove(equipSpawnPoints[i]);
        if(!PlayerPrefs.HasKey("IntroDone"))
        {
            if(playerToy.name=="NewCicada(Clone)")
            {
                playerToy.transform.GetChild(0).GetComponent<ToyToolboxInteractionManager>().HideEquipVisuals();
            }
            else
            playerToy.GetComponent<ToyToolboxInteractionManager>().HideEquipVisuals();
        }
        
        HideKeyItemSpawns();

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
        prizeSpawn.position = equipHiders[0].transform.position;
        equipHiders.Remove(equipHiders[0]);
        yield return new WaitForSeconds(1);
        Instantiate(keyItem[0], prizeSpawn.position, Quaternion.identity);
        ItemCycleManager.RemoveToyPrefabInGameManager(0);
        readyToReboot= true;
    }


    void HideKeyItemSpawns()
    {
        Transform HiderContainer = GameObject.Find("ToyHiders").transform;
        for(int i =0; i<HiderContainer.childCount;i++)
        {
            equipHiders.Add(HiderContainer.GetChild(i).gameObject);
        }
        for (int i =0; i<equipSpawnPoints.Count; i++)
        {
            equipSpawnPoints[i].transform.position = equipHiders[i].transform.position;
            equipHiders.Remove(equipHiders[i]);
        }
       
    }

    #endregion

    #region Win
        void CheckWinCondition()
    {

    }
    void WinConditionReached()
    {
        StartCoroutine(WinCoroutine());
    }

    IEnumerator WinCoroutine()
    {
        win = false;
        player.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        player.gameObject.GetNamedChild("Canvas").transform.GetChild(1).GetComponent<TextMeshPro>().enabled = true;
        player.gameObject.GetNamedChild("Canvas").transform.GetChild(1).GetComponent<TextMeshPro>().text = "good run";
        yield return new WaitForSeconds(5);
        SpawnPlayerInRoom();
        readyToReboot = false;

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
        player.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        player.gameObject.GetNamedChild("Canvas").transform.GetChild(1).GetComponent<TextMeshPro>().enabled = true;
        player.gameObject.GetNamedChild("Canvas").transform.GetChild(1).GetComponent<TextMeshPro>().text = "you are pacified";
        EnemyInteractionManager.killPlayer = false;
        yield return new WaitForSeconds(5);

        SpawnPlayerInRoom();
        
    }
    #endregion

    #region Utils
/*     void ManualRestartGame(InputAction.CallbackContext obj)
    {
        if(obj.ReadValue<float>()==1&&readyToReboot)
        {
            SpawnPlayerInRoom();
            readyToReboot = false;
        }
    }*/

   
    #endregion
}
