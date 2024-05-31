using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTimeLimit;
    public bool gameForceEnd, sendForceSignal;

    public Transform player;

    [Header("Spawning")]
    public Transform spawnPoints;
    public int spawnPointID;
    public int enemyWaveID;
    public int enemySummonID;
    public int[] enemyPattern;
    public List<int> playerEquipID = new();
    public GameObject playerPrefab, endPlayerPrefab, enemyDoor; 
    public List<GameObject> keyItem = new();
    public GameObject[] enemyPrefab;

    public static bool readyToReboot, win, enemySpawned, holdEnemySpawn;

    [Header("Labyrinth")]
    public List<GameObject> roomSpawnPoints;
    public List<GameObject> equipSpawnPoints;
    public List<GameObject> equipHiders;
    public float enemySpawnInterval;
    public float enemySpawnCountdown;
    public List<int> enemySpawnPointsSave;

    
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
            ItemCycleManager.StoreKeyItemList();
           
            DontDestroyOnLoad(gameObject);

            
        }

        restart.action.performed += ExitGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        SpawnPlayerInRoom();

    }
    // Update is called once per frame
    void Update()
    {
        /*if(ExitRoomTrigger.isReadytoPlay)
        {
            SpawnPlayerOnMap();
        }*/

        if (SceneManager.GetActiveScene().name == "LabyrinthGameScene")
        {
            if (!TutorialsManager.isTut && !DialogueManager.isStory && PrizeBoxManager.taken && !gameForceEnd)
            {
                gameTimeLimit -= Time.deltaTime;
            }

            if (gameTimeLimit < 0 && !win)
            {
                gameTimeLimit = 600;
                gameForceEnd = true;
                sendForceSignal = true;
                win = true;
            }

            if (EnemyInteractionManager.killPlayer)
            {
                PlayerDeath();

            }

            if (win)
            {
                WinConditionReached();
            }

            if (!TutorialsManager.isTut && PrizeBoxManager.taken && !DialogueManager.isStory && !holdEnemySpawn && enemyWaveID < 4)
            {
                enemySpawnCountdown -= Time.deltaTime;
            }
            else return;

            if (enemySpawnCountdown < 0 && SceneManager.GetActiveScene().name == "LabyrinthGameScene")
            {
                enemySpawnCountdown = enemySpawnInterval;
                StartCoroutine(EnemySpawnCoroutine());
            }
            else return;
        }
        else return;

    }

    #region Characters
    public void SpawnPlayerInRoom()
    { 
        ClearSaveBools();
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

        SpawnPlayerInMap();


        yield return null;
       
    }

    public void SpawnPlayerInMap()
    {
        int i = Random.Range(0, 3);
        player = Instantiate(playerPrefab, roomSpawnPoints[i].transform.position + new Vector3(0, 2, 0), roomSpawnPoints[i].transform.rotation).transform;
        InitPlayerLinks();


        StartCoroutine(SpawnPlayerEquip(i));
        

    }

    void InitPlayerLinks()
    {
        UIViewAligner.player = player.gameObject.GetNamedChild("Main Camera").transform;

        if (SceneManager.GetActiveScene().name == "LabyrinthGameScene")
        {
            IntroSpawnReporter.player = UIViewAligner.player;
            WeakStateManager.instance.player = player;
            PrizeBoxManager.taken = false;
            ToyToolboxInteractionManager.itemTaken= false;  

            TutorialsManager.instance.controlsMap = FindObjectOfType<DebugHelper>();
            TutorialsManager.instance.controlsMap.enabled = false;

            TutorialsManager.cicadaTut = false;
            TutorialsManager.topTut = false;
            TutorialsManager.planeTut = false;
            TutorialsManager.gunTut = false;

            TutorialsManager.instance.cicadaGoal = false;
            TutorialsManager.instance.gunGoal = false;
            TutorialsManager.instance.planeGoal = false;
            TutorialsManager.instance.topGoal = false;
        }
       

        /*if (PlayerPrefs.GetInt("IntroEnd") == 1)
        {
            TutorialsManager.instance.controlsMap.enabled = true;
        }
        else return;*/
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


    }

    public void GetEnemySpawnPoints()
    {
        if (enemyWaveID < 4)
        {
            
            for (int i = 0; i < enemyPattern[enemyWaveID]; i++)
            {
                int rand = Random.Range(0, spawnPoints.childCount);
                enemySpawnPointsSave.Add(rand);
                Instantiate(enemyDoor, spawnPoints.GetChild(enemySpawnPointsSave[i]).position + new Vector3(0.5f, 3, 0), Quaternion.identity);
                
            }
            
        }
        else return;
    }

   IEnumerator SpawnEnemiesDynamic()
    {
        if (enemyWaveID < 4 && SceneManager.GetActiveScene().name == "LabyrinthGameScene")
        {
            enemySpawned = true;
            yield return new WaitForSeconds(1);
            SummonsDoorComp.open = true;
            for (int i = 0; i < enemySpawnPointsSave.Count; i++)
            {
                Instantiate(enemyPrefab[enemySummonID], spawnPoints.GetChild(enemySpawnPointsSave[i]).position + new Vector3(0, 5, 0), Quaternion.identity);
                Debug.Log("Enemy spawned");
                enemySummonID++;

            }
            enemyWaveID++;
            enemySpawnPointsSave.Clear();
        }
        
        
    }

    IEnumerator EnemySpawnCoroutine()
    {
        GetEnemySpawnPoints();
        holdEnemySpawn = true;
        yield return new WaitForSeconds(30);
        holdEnemySpawn = false;
        StartCoroutine(SpawnEnemiesDynamic());
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
        if (playerToy.name == "NewCicada(Clone)" || playerToy.name == "NewGun(Clone)")
        {
            playerToy.transform.GetChild(0).GetComponent<ToyToolboxInteractionManager>().HideEquipVisuals();
        }
        else
            playerToy.GetComponent<ToyToolboxInteractionManager>().HideEquipVisuals();
        /*if(PlayerPrefs.GetInt("IntroDone") != 1)
        {
            if(playerToy.name=="NewCicada(Clone)"||playerToy.name=="NewGun(Clone)")
            {
                playerToy.transform.GetChild(0).GetComponent<ToyToolboxInteractionManager>().HideEquipVisuals();
            }
            else
            playerToy.GetComponent<ToyToolboxInteractionManager>().HideEquipVisuals();
        }*/


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
            equipSpawnPoints.Remove(equipSpawnPoints[n]);
            ItemCycleManager.RemoveToyPrefabInGameManager(j);
            usedSpawnIDs.Add(n);

        }
        yield return null;
        StartCoroutine(MapSpawnCoroutine());
    }

    IEnumerator SummonPrizeEquip()
    {
        Transform prizeSpawn = GameObject.Find("PrizeSpawn").transform;
        prizeSpawn.position = equipHiders[0].transform.position;
        equipHiders.Remove(equipHiders[0]);
        yield return new WaitForSeconds(1);
        Instantiate(keyItem[0], prizeSpawn.position, Quaternion.identity);
        ItemCycleManager.RemoveToyPrefabInGameManager(0);
        if(GameEndReporter.tutorialDone)
        {
            readyToReboot= true;
        }
        /*if(PlayerPrefs.GetInt("IntroDone") == 1)
        {
            readyToReboot = true;
        }*/
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
    void WinConditionReached()
    {
        StartCoroutine(WinCoroutine());
    }

    IEnumerator WinCoroutine()
    {
        win = false;
        player.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        yield return new WaitForSeconds(3);
        //SpawnPlayerInRoom();
        StartCoroutine(EndRoomCoroutine());
        

    }

    IEnumerator EndRoomCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("EndGameScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        roomSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Respawn"));
        player = Instantiate(endPlayerPrefab, roomSpawnPoints[3].transform.position + new Vector3(0, 2, 0), roomSpawnPoints[3].transform.rotation).transform;
        EndingManager.instance.cam = player.GetComponentInChildren<Camera>();
        InitPlayerLinks();
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
        /*player.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        /*yield return new WaitForSeconds(5);
        SpawnPlayerInRoom();*/
        EnemyInteractionManager.killPlayer = false;
        yield return null;
        if(!WeakStateManager.instance.weakened)
        {
            WeakStateManager.instance.SwitchToWeakState();
        }
        
    }
    #endregion

    #region Utils
    public void ClearSaveBools()
    {
        gameTimeLimit = 600;
        enemySpawnCountdown = enemySpawnInterval;

        StopAllCoroutines();

        TutorialsManager.waitForTutEnd = false;
        GameEndReporter.tutorialDone = false;
        gameForceEnd = false;
        PrizeBoxManager.taken = false;
        DialogueManager.instance.story1= false;
        DialogueManager.instance.story2= false;
        DialogueManager.instance.story3= false;
     

        AudioManager.instance.audioSource.clip = AudioManager.instance.BGMAudioClips[11];
        AudioManager.instance.audioSource.Play();
    }

    public void ExitGame(InputAction.CallbackContext obj)
    {
        if(this!=null)
        {
            if(obj.ReadValue<float>() ==1)
            {
                Application.Quit();
            }
        }
    }
    #endregion
}
