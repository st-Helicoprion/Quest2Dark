using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PushPedestalBehavior : MonoBehaviour
{
    public Transform player;
    public bool ascendIsDone, itemLost;
    public float ascendCount, descendCount, lifetime;

    private void Start()
    {
        player = GameManager.instance.player;
    }
    // Update is called once per frame
    void Update()
    {
        CheckPlayerDistance(20);
        

        ascendCount += Time.deltaTime;

        if(ascendCount<3&&!ascendIsDone)
        {
            transform.Translate(Vector3.up*Time.deltaTime);

        }
        else ascendIsDone= true;
       
        

        if(itemLost)
        {
            descendCount += Time.deltaTime;
            if (descendCount < 3)
            {
                transform.Translate(-1 * Time.deltaTime * Vector3.up);
            }
            else Destroy(this.gameObject);
        }
    }

    void CheckPlayerDistance(float minDistance)
    {
        if(Vector3.Distance(this.transform.position, player.position)<minDistance)
        {
            return;
        }
        else lifetime += Time.deltaTime;


        if (lifetime > Vector3.Distance(this.transform.position, player.position))
        {
            itemLost = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("RightHand")||collision.transform.CompareTag("LeftHand"))
        {
            itemLost= true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.TryGetComponent<KeyItemReporter>(out _))
        {
            itemLost = true;
        }
    }

  
}
