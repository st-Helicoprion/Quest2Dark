using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TutorialsManager : MonoBehaviour
{
    public static TutorialsManager instance;
    public static bool waitForTutEnd, intro;
    public bool cicadaTut, gunTut, planeTut, topTut;
    public GameObject kek, toyBox;
    public static GameObject givenToy;
    public static Transform player, kekActive;
    public static IntroSpawnReporter spawnPointToUse;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if(PlayerPrefs.HasKey("IntroDone"))
            {
                waitForTutEnd= true;
                
            }
            if (PlayerPrefs.HasKey("Cicada"))
            {
                cicadaTut = true;
            }
            if (PlayerPrefs.HasKey("Gun"))
            {
                gunTut = true;
            }
            if (PlayerPrefs.HasKey("Plane"))
            {
                planeTut = true;
            }
            if (PlayerPrefs.HasKey("Top"))
            {
                topTut = true;
            }
        }


    }
  
    public void CheckStoryStartTutorial()
    {
        player = GameObject.Find("Main Camera").transform;
       
        if (PlayerPrefs.HasKey("IntroDone"))
        {
            return;
        }
        else
        {
            IntroTutorial();
        }
    }
    
    public void IntroTutorial()
    {
        intro = true;
        kekActive = Instantiate(kek, spawnPointToUse.transform.position, Quaternion.identity).transform;
        kekActive.LookAt(player.position);
        StartCoroutine(IntroDialogue(kekActive.GetComponentInChildren<TextMeshPro>()));
    }

    IEnumerator IntroDialogue(TextMeshPro dialogueText)
    {
        yield return new WaitForSeconds(1.5f);
        dialogueText.text = "Is that the\nHERO I see ?";
        yield return new WaitForSeconds(3);
        dialogueText.text = "We've been waiting !\nYou have to help us !";
        yield return new WaitForSeconds(3);
        dialogueText.text = "You must have trouble\nseeing...hold on now..";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Swing at this~";

        GameObject givenToyBox = Instantiate(toyBox, givenToy.transform.position, Quaternion.identity);
        if (givenToy.name == "NewCicada(Clone)")
        {
            givenToy.transform.GetChild(0).GetComponent<ToyToolboxInteractionManager>().ShowEquipVisuals();
        }
        else
        givenToy.GetComponent<ToyToolboxInteractionManager>().ShowEquipVisuals();
        waitForTutEnd = true;

    }

    IEnumerator CicadaTutorial(TextMeshPro dialogueText)
    {
        cicadaTut = true;
        
        dialogueText.text = "Ah HERO\nI see you have found\nthe CICADA";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Swing the tub around\nthe stick to expand a\n sonar domain";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Sonars is how we\n see things in\nthis world";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Learn to see with\n that and reach\n the brain in the sky";
        yield return new WaitForSeconds(3);
        dialogueText.text = "If you see white noise,\nRUN as far away from\nit as possible";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Best of luck HERO, KEK out";

        yield return new WaitForSeconds(3);
        
        Destroy(kekActive.gameObject);
    }

    IEnumerator GunTutorial(TextMeshPro dialogueText)
    {
        gunTut = true;
        
        dialogueText.text = "Ah HERO\nI see you have found\nthe GUN";
        yield return new WaitForSeconds(3);
        dialogueText.text = "It shoots sonar bullets\nwhen you press the\ntrigger";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Sonars is how we\n see things in\nthis world";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Learn to see with\n that and reach\n the brain in the sky";
        yield return new WaitForSeconds(3);
        dialogueText.text = "The GUN can also knock-\nback certain enemies";
        yield return new WaitForSeconds(3);
        dialogueText.text = "If you see white noise,\nRUN as far away from\nit as possible";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Best of luck HERO, KEK out";

        yield return new WaitForSeconds(3);
       
        Destroy(kekActive.gameObject);
    }

    IEnumerator PlaneTutorial(TextMeshPro dialogueText)
    {
        planeTut = true;
       
        dialogueText.text = "Ah HERO\nI see you have found\nthe PLANE";
        yield return new WaitForSeconds(3);
        dialogueText.text = "It makes sonar pulses\nas it flies, just put\nyour hand through the\nring to launch it";
        yield return new WaitForSeconds(6);
        dialogueText.text = "Sonars is how we\n see things in\nthis world";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Learn to see with\n that and reach\n the brain in the sky";
        yield return new WaitForSeconds(3);
        dialogueText.text = "The PLANE can also mark\n certain enemies\nmaking them easier to spot";
        yield return new WaitForSeconds(3);
        dialogueText.text = "It regenerates after \nyou throw it so\ndon't worry about\nrunning out";
        yield return new WaitForSeconds(3);
        dialogueText.text = "If the plane won't fly,\ntry pressing the Oculus\nbutton to recenter";
        yield return new WaitForSeconds(3);
        dialogueText.text = "If you see white noise,\nRUN as far away from\nit as possible";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Best of luck HERO, KEK out";

        yield return new WaitForSeconds(3);
       
        Destroy(kekActive.gameObject);
    }

    IEnumerator TopTutorial(TextMeshPro dialogueText)
    {
        topTut = true;
        
        dialogueText.text = "Ah HERO\nI see you have found\nthe TOP";
        yield return new WaitForSeconds(3);
        dialogueText.text = "It expands a sonar domain\naway from you when\nit spins, put your hand\nthrough the ring, and pull\nto jump start it";
        yield return new WaitForSeconds(6);
        dialogueText.text = "Sonars is how we\n see things in\nthis world";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Learn to see with\n that and reach\n the brain in the sky";
        yield return new WaitForSeconds(3);
        dialogueText.text = "The TOP can also attracat\n certain enemies\nwhen they hear its noise";
        yield return new WaitForSeconds(3);
        dialogueText.text = "It returns after you\n throw it, and you can\n also grip to recall it !";
        yield return new WaitForSeconds(3);
        dialogueText.text = "If the TOP won't launch,\ntry pressing the Oculus\nbutton to recenter";
        yield return new WaitForSeconds(3);
        dialogueText.text = "If you see white noise,\nRUN as far away from\nit as possible";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Best of luck HERO, KEK out";

        yield return new WaitForSeconds(3);
       
        Destroy(kekActive.gameObject);
    }
    public void CallOutTutorialType()
    {
        if(kekActive==null)
        {
            kekActive = Instantiate(kek, spawnPointToUse.transform.position, Quaternion.identity).transform;
            kekActive.LookAt(player.position);
        }
        else
        {
            if (PlayerPrefs.GetInt("TutID") == 0 && cicadaTut == false)
            {
                kekActive.position = spawnPointToUse.transform.position;
                StartCoroutine(CicadaTutorial(kekActive.GetComponentInChildren<TextMeshPro>()));
            }
            else if (PlayerPrefs.GetInt("TutID") == 1 && gunTut == false)
            {
                kekActive.position = spawnPointToUse.transform.position;
                StartCoroutine(GunTutorial(kekActive.GetComponentInChildren<TextMeshPro>()));
            }
            else if (PlayerPrefs.GetInt("TutID") == 2 && planeTut == false)
            {
                kekActive.position = spawnPointToUse.transform.position;
                StartCoroutine(PlaneTutorial(kekActive.GetComponentInChildren<TextMeshPro>()));
            }
            else if (PlayerPrefs.GetInt("TutID") == 3 && topTut == false)
            {
                kekActive.position = spawnPointToUse.transform.position;
                StartCoroutine(TopTutorial(kekActive.GetComponentInChildren<TextMeshPro>()));
            }
            else return;
        }
       
       
    }
    

}
