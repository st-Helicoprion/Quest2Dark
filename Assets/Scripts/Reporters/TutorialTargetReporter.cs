using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTargetReporter : MonoBehaviour
{
    IEnumerator KnockbackCoroutine(Collision collision)
    {
        for (int i = 0; i < 2; i++)
        {
            transform.localPosition -= new Vector3(0,0,collision.transform.position.z - transform.position.z);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator VanishCoroutine()
    {
        yield return new WaitForSeconds(5);

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
            TutorialsManager.instance.planeGoal = true;
            StartCoroutine(VanishCoroutine());
        }
    }
}
