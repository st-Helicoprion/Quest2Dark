using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HopterManager : MonoBehaviour
{
    public int hopterMaxNum;
    public float hopterCount;
    public GameObject hopterPrefab,summonHopter;
    public static GameObject[] enemiesInScene;
    public static List<GameObject> untaggedEnemies = new List<GameObject>();
    public static List<GameObject> hoptersInScene = new List<GameObject>();
    public static Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        hopterMaxNum = 2;
        untaggedEnemies.Clear();
        //demo code
        StartCoroutine(FindEnemies());
    }

    public static IEnumerator FindEnemies()
    {
        enemiesInScene = GameObject.FindGameObjectsWithTag("Finger");
        for(int i =0;i<enemiesInScene.Length;i++)
        {
            Debug.Log(enemiesInScene.Length);

        }

        yield return null;

        for (int i= 0 ;i<enemiesInScene.Length;i++)
        {
            if (untaggedEnemies.Contains(enemiesInScene[i]))
            {
                continue;
            }
            untaggedEnemies.Add(enemiesInScene[i]);
        }
        Debug.Log("enemies added to list");
    }

    void SendOutHopter()
    {
        //SpinAnim();
        Debug.Log("hopter out");
        summonHopter = Instantiate(hopterPrefab, transform.position, Quaternion.identity);
        hoptersInScene.Add(summonHopter);

    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Hopter"))
        {
            

            if (anim == null)
            {
                //anim = other.transform.GetChild(0).GetComponent<Animator>();
               
                //demo code
                anim=other.GetComponent<Animator>();
            }

            if (hoptersInScene.Count < hopterMaxNum)
            {
                SendOutHopter();

            }
            else
            {
                Destroy(hoptersInScene[0]);
                hoptersInScene.RemoveAt(0);
                StartCoroutine(FindEnemies());
                SendOutHopter();
            }
        }
    }

    void SpinAnim()
    {
        anim.SetTrigger("Spin");
    }
}
