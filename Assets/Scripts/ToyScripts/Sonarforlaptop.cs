using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonarforlaptop : MonoBehaviour
{
   
    public Transform sonar;
    public static CicadaHitboxManager hitboxManager;
    public float maxSonarHeight, minSonarHeight, increaseRate, internalSonarCount;
    public GameObject internalSonar;
    // cicadaStick, cicadaTub, cicadaRope;
    // public ToyToolboxInteractionManager interactionManager;
    // public AudioSource cicadaAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        maxSonarHeight = 5;
        minSonarHeight = -50;
        sonar = GameObject.Find("PlayerSonar").transform;
        CheckForHitBoxManager();
    }

    // Update is called once per frame
    void Update()
    {
        //use for mechanic test otherwise disable
        //CheckForHitBoxManager();

        SonarHeightClamp();
        ActivateInternalSonar();
        // DetectSonar();
        // CheckStickActive();

        // if(interactionManager.isInBox)
        // {
        //     DecreaseSonarHeight();
        // }

    }

    public static void CheckForHitBoxManager()
    {
        if (GameObject.FindGameObjectWithTag("Cicada") != null && hitboxManager == null)
        {
            hitboxManager = FindObjectOfType<CicadaHitboxManager>();
        }
        else return;
    }

    void SonarHeightClamp()
    {

        maxSonarHeight = Mathf.Clamp(maxSonarHeight, -10, 40);
        Vector3 sonarHeight = sonar.localPosition;
        sonarHeight.y = Mathf.Clamp(sonarHeight.y, minSonarHeight, maxSonarHeight);
        sonar.localPosition = sonarHeight;
    }

    void DecreaseSonarHeight()
    {
        sonar.localPosition -= new Vector3(0, 2, 0);

    }

    void IncreaseSonarHeight()
    {
        // if(!cicadaAudioSource.isPlaying)
        // {
        //     cicadaAudioSource.PlayOneShot(AudioManager.instance.ToysSFX[3]);
        // }
        sonar.localPosition += new Vector3(0, increaseRate, 0);

    }

    void ActivateInternalSonar()
    {
        if (sonar.localPosition.y > minSonarHeight)
        {
            internalSonarCount += Time.deltaTime;

            if (internalSonarCount > .75f)
            {
                Instantiate(internalSonar, sonar.parent);
                internalSonarCount = 0;
            }

        }
        else return;

    }

    void DetectSonar()
    {
        if (hitboxManager != null)
        {
            if (hitboxManager.isSonarUp == true)
            {
                IncreaseSonarHeight();

            }
            else DecreaseSonarHeight();

        }
        else return;
    }

    // void CheckStickActive()
    // {
    //     if (cicadaStick.activeInHierarchy)
    //     {
    //         cicadaTub.SetActive(true);
    //         cicadaRope.SetActive(true);
    //     }
    //     else
    //     {
    //         cicadaTub.SetActive(false);
    //         cicadaRope.SetActive(false);
    //     }
    // }
}
