using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewAligner : MonoBehaviour
{
    public static Transform player;
    public bool tutUI;

    private void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        if (PrizeBoxManager.taken&&tutUI)
        {
            gameObject.SetActive(false);
        }
        else return;
    }
}
