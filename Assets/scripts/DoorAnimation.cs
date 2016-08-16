using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour
{
    private Animator handAnimator;
    private GameObject player;
    private GameObject playerHand;

    private bool canInteract = false;
    private Rigidbody doorBody;

    // Use this for initialization
    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHand = GameObject.FindGameObjectWithTag("playerHand");
        handAnimator = playerHand.GetComponent<Animator>();
        doorBody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("door collides with: " + other.gameObject.ToString());
        if ( other.gameObject == playerHand )
        {
            handAnimator.SetTrigger("interactable");
            canInteract = true;
            //Debug.Log("player hand near door");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if ( other.gameObject == playerHand )
        {
            handAnimator.SetTrigger("point");
            canInteract = false;
            handAnimator.SetBool("poking", false);
        }
    }

    // Update is called once per frame
    void Update ()
    {
	    if ( canInteract )
        {
            var poking = GvrController.ClickButtonDown || GvrController.AppButtonDown;
            if ( poking )
            {
                handAnimator.SetBool("poking", true);
                doorBody.isKinematic = false;
                //Debug.Log("open door");
            }
            else if ( GvrController.ClickButtonUp || GvrController.AppButtonUp )
            {
                handAnimator.SetTrigger("interactable");
                handAnimator.SetBool("poking", false);
            }
        }
	}
}
