using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandAnimation : MonoBehaviour
{
    [SerializeField] private InputActionReference gripAction;
    [SerializeField] private InputActionReference pinchAction;
    private Animator animator;
    public bool isGripping, handNotEmpty;
    

    private void OnEnable()
    {
        //grip
        /*gripAction.action.performed += Gripping;
        gripAction.action.canceled += GripRelease;*/
        gripAction.action.performed += GripState;
        gripAction.action.canceled += GripState;

        /*//pinch
        pinchAction.action.performed += Pinching;
        pinchAction.action.canceled += PinchRelease;*/
    }


    private void Awake() => animator = GetComponent<Animator>();

    private void Gripping(InputAction.CallbackContext obj) => animator.SetFloat("Grip", obj.ReadValue<float>());

    public void GripState(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() == 1)
        {
            PlayerStateManager.CheckPlayerState();

            if (animator != null)
            {
                if (!isGripping)
                {
                    animator.SetFloat("Grip", 1f);
                    isGripping = true;
                }
                else
                {
                    animator.SetFloat("Grip", 0f);
                    isGripping = false;
                }

            }
            else return;


        }
        else if (obj.ReadValue<float>() == 0 && this != null)
            StartCoroutine(CheckForEmptyHand());
        else return;
    }

    private void GripRelease(InputAction.CallbackContext obj) => animator.SetFloat("Grip", 0f);

    private void Pinching(InputAction.CallbackContext obj) => animator.SetFloat("Pinch", obj.ReadValue<float>());

    private void PinchRelease(InputAction.CallbackContext obj) => animator.SetFloat("Pinch", 0f);

    public IEnumerator CheckForEmptyHand()
    {
        yield return null;

            if(handNotEmpty)
            {
                isGripping = true;
                animator.SetFloat("Grip", 1f);
            }
            else
            {
                isGripping = false;
                animator.SetFloat("Grip", 0f);
            }
           
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cicada") || other.CompareTag("Gun") || other.CompareTag("Hopter") || other.CompareTag("SpinningTop"))
        {
            handNotEmpty = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cicada") || other.CompareTag("Gun") || other.CompareTag("Hopter") || other.CompareTag("SpinningTop"))
        { 
            handNotEmpty = false;
            StartCoroutine(CheckForEmptyHand());
        }
    }
}
