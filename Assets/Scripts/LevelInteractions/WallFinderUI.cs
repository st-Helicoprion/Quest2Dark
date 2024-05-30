using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class WallFinderUI : MonoBehaviour
{
    public Transform[] blockers;
    public static bool sent;
    public int blockerNum;
    public Transform cam;
    public static float blockerInterval;
    // Start is called before the first frame update
    void Start()
    {
        blockerNum= 0;

        FindBlockers();
    }

    // Update is called once per frame
    void Update()
    {
        if(blockerInterval<0)
        {
            blockerInterval = 1;
            sent = false;
        }

        if (Physics.Raycast(cam.position, cam.forward-new Vector3(0,1.25f,0), out RaycastHit hit, 2))
        {
            if (hit.transform.CompareTag("Ground") && !sent)
            {
                if (blockerNum < blockers.Length)
                {
                    MoveBlockers(hit);

                }
                else
                {
                    blockerNum = 0;
                    MoveBlockers(hit);
                }
            }
            else return;
        }
        else return;
    }

    public void FindBlockers()
    {
        Transform blockersContainer = GameObject.Find("PlayerBlockers").transform;

        for (int i = 0; i < blockersContainer.childCount; i++)
        {
            blockers[i] = blockersContainer.GetChild(i).transform;
        }
    }
    public void MoveBlockers(RaycastHit hit)
    {
        float randNum = Random.Range(-0.3f, 0.4f);
        sent = true;
        blockers[blockerNum].position = new Vector3(hit.point.x, cam.position.y+randNum, hit.point.z);
        blockerNum++;
    }
}
