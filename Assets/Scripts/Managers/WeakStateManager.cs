using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if(weakCountdown<weakInterval-2)
        {
            player.tag = "Player";
        }

        if(weakCountdown < 0) 
        {
            weakened = false;
            weakCountdown = weakInterval;
            if(SceneManager.GetActiveScene().name == "LabyrinthGameScene")
            {
                ToyToolboxInteractionManager.itemTaken = true;
            }
            AudioManager.instance.CheckBGMToPlay();
        }
    }
    public void SwitchToWeakState()
    {
        weakened=true;
        if (!StickGiant.musicOn)
        {
            AudioManager.instance.WeakStateMusic();

        }
        else return;
    }

}
