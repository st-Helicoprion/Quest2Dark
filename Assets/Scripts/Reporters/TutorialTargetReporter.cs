using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTargetReporter : MonoBehaviour
{
    IEnumerator KnockbackCoroutine(Collision collision)
    {
        for (int i = 0; i < 3; i++)
        {
            transform.localPosition -= new Vector3(0,0,1);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    IEnumerator VanishCoroutine()
    {
        yield return new WaitForSeconds(3);
        TutorialsManager.instance.planeGoal = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            TutorialsManager.instance.gunGoal = true;
            StartCoroutine(KnockbackCoroutine(collision));

        }
    }
    private void OnTriggerEnter(Collider other)
    {
      
        if(other.name=="woodairplane")
        {
            
            StartCoroutine(VanishCoroutine());
        }
    }
}
