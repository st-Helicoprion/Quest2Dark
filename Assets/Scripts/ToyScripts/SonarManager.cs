using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SonarManager : MonoBehaviour
{
    public Transform sonar;
    public static CicadaHitboxManager hitboxManager;
    public float maxSonarHeight, minSonarHeight, increaseRate, internalSonarCount;
    public GameObject internalSonar, cicadaStick, cicadaTub, cicadaRope, sonarDust;
    public ToyToolboxInteractionManager interactionManager;
    public AudioSource cicadaAudioSource;
    public float buffer;
    public Renderer sonarSkin;

    // Start is called before the first frame update
    void Start()
    {
        buffer = 0;
        maxSonarHeight = 5;
        minSonarHeight = -50;
        sonar = GameObject.Find("PlayerSonar").transform;
        CheckForHitBoxManager();
        sonarSkin = sonar.GetComponent<Renderer>();
        sonarDust = sonar.parent.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //use for mechanic test otherwise disable
        //CheckForHitBoxManager();

        SonarHeightClamp();
        ActivateInternalSonar();
        DetectSonar();
        CheckStickActive();

        if(interactionManager.isInBox)
        {
            DecreaseSonarHeight();
        }

        if(!sonarSkin.enabled)
        {
            if(ToyToolboxInteractionManager.itemTaken)
            {
                sonarSkin.enabled = true;
                sonarDust.SetActive(true);
            }
        }

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

        maxSonarHeight = Mathf.Clamp(maxSonarHeight, -5, 20);
        Vector3 sonarHeight = sonar.localPosition;
        sonarHeight.y = Mathf.Clamp(sonarHeight.y, minSonarHeight, maxSonarHeight);
        sonar.localPosition = sonarHeight;
    }

    void DecreaseSonarHeight()
    {
        if (sonar.localPosition.y > minSonarHeight)
        {
            sonar.localPosition -= new Vector3(0, 2, 0);
        }
        else return;

    }

    void IncreaseSonarHeight()
    {
        if(!cicadaAudioSource.isPlaying)
        {
            cicadaAudioSource.pitch = Random.Range(0.5f,0.8f);
            cicadaAudioSource.PlayOneShot(AudioManager.instance.ToysSFX[3]);
        }
        sonar.localPosition += new Vector3(0, increaseRate, 0);
        
    }

    void ActivateInternalSonar()
    {
        if (sonar.localPosition.y > minSonarHeight)
        {
            sonar.tag = "Sonar";
            internalSonarCount += Time.deltaTime;

            if (internalSonarCount > 1)
            {
                Instantiate(internalSonar, sonar.parent);
                internalSonarCount = 0;
            }

        }
        else 
            sonar.tag= "Untagged";

    }

    void DetectSonar()
    {
        if (hitboxManager != null)
        {
            if (hitboxManager.isSonarUp == true)
            {
                IncreaseSonarHeight();
                buffer = 2.5f;

            }
            else
            {
                if(buffer>0)
                {
                    buffer -= Time.deltaTime;
                }
                if (buffer <0)
                {
                    DecreaseSonarHeight();

                }

            }

            }
        else return;
    }

    void CheckStickActive()
    {
        if (cicadaStick.activeInHierarchy)
        {
            cicadaTub.SetActive(true);
            cicadaRope.SetActive(true);
        }
        else
        {
            cicadaTub.SetActive(false);
            cicadaRope.SetActive(false);
        }
    }
}
