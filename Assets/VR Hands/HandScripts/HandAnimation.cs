using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]
public class HandAnimation : MonoBehaviour
{
    [SerializeField] private InputActionReference gripAction;
    [SerializeField] private InputActionReference pinchAction;
    private Animator animator;
    public bool grip, handNotEmpty;
    public int handID;
    public static int itemID;
    public CircleLightManager circleLightManager;
    

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
        if(this!=null)
        {
            if (obj.ReadValue<float>() == 1)
            {
                GetComponent<Collider>().enabled = true;

                if (animator != null)
                {

                    animator.SetFloat("Grip", 1f);
                    grip = true;


                }
                else return;


            }
            else if (obj.ReadValue<float>() == 0 && this != null)
            {
                grip = false;
                StartCoroutine(CheckForEmptyHand());
            }

            else return;
        }
        
    }

    private void GripRelease(InputAction.CallbackContext obj) => animator.SetFloat("Grip", 0f);

    private void Pinching(InputAction.CallbackContext obj) => animator.SetFloat("Pinch", obj.ReadValue<float>());

    private void PinchRelease(InputAction.CallbackContext obj) => animator.SetFloat("Pinch", 0f);

    public IEnumerator CheckForEmptyHand()
    {
        yield return null;

            if(handNotEmpty)
            {
                
                animator.SetFloat("Grip", 1f);
                
            }
            else
            {
                
                animator.SetFloat("Grip", 0f);
               
            }
           
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out KeyItemReporter keyItem))
        {
            if (grip)
            {
                handNotEmpty = true;
                itemID = keyItem.itemID;
                //circleLightManager.ChangeLightColor(itemID);
                StartCoroutine(CheckForEmptyHand());

            }
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ToyToolboxInteractionManager>(out _))
        {
            handNotEmpty = false;
            StartCoroutine(CheckForEmptyHand());
        }


    }
}
