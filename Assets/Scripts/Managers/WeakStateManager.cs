using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakStateManager : MonoBehaviour
{
    public static WeakStateManager instance;
    public Transform player;
    public bool weakened;
    public float weakCountdown, weakInterval;

    private void Awake()
    {
        instance= this;
    }
    private void Update()
    {
        if (weakened)
        {
            weakCountdown-=Time.deltaTime;
        }

        if(weakCountdown < 0) 
        {
            weakened = false;
            weakCountdown = weakInterval;
            player.tag = "Player";
            ToyToolboxInteractionManager.itemTaken = true;
            AudioManager.instance.CheckBGMToPlay();
        }
    }
    public void SwitchToWeakState()
    {
        weakened=true;
        AudioManager.instance.WeakStateMusic();
    }

}
