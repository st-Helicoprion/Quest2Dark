using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SphereExpand : MonoBehaviour
{
    int a = 0;
    [SerializeField] float speed = 1;
    [SerializeField] float scale = 1;
    void Start()
    {
        transform.localScale=Vector3.one*scale;
    }
    void Update()
    {
        //一直按著空白鍵可以讓圓變大，沒有上限，但是圓的大小不會小於一個特定值
        if (transform.localScale.x >= scale || a >= 0)
        {
            transform.localScale += Vector3.one * a * speed * Time.deltaTime;
        }
    }
}
