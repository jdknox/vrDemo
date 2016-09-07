using UnityEngine;
using System.Collections;

public class HandInteraction : MonoBehaviour {

    private Animator handAnimator;
    private GameObject playerHand;
    private GameObject player;
    private GameObject controllerPivot;
    public ControllerDebugInfo debugInfo;
    
    public Vector3 handOffset;

    public delegate void interactAction();
    public static event interactAction onInteract;

    private bool canInteract = false;
    private bool interacting = false;
    private GameObject selectedObject;
    private Vector3 initialHandOffset;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHand = GameObject.FindGameObjectWithTag("playerHand");
        controllerPivot = GameObject.Find("ControllerPivot");
        handAnimator = playerHand.GetComponent<Animator>();
        initialHandOffset = playerHand.transform.localPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "interactable" && other.isTrigger)
        {
            Debug.Log("hand collides with: " + other.tag.ToString());
            //Debug.Log("ENTER: hand intersects " + other.ToString() + "?: " + GetComponent<Collider>().bounds.Intersects(other.bounds));
            handAnimator.SetTrigger("interactable");
            if (other.gameObject.GetComponent<ObjectInteract>() != null)
            {
                selectedObject = other.gameObject;
                canInteract = true;
                Debug.Log("player hand near: " + selectedObject.ToString());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ( (other.tag == "interactable") && other.isTrigger )
        {
            Debug.Log("player hand leaving: " + other.ToString());
            //Debug.Log("EXIT: hand intersects " + other.ToString() + "?: " + GetComponent<Collider>().bounds.Intersects(other.bounds));
            handAnimator.SetTrigger("point");
            handAnimator.SetBool("poking", false);
            selectedObject.GetComponent<ObjectInteract>().unregister();
            canInteract = false;
            interacting = false;
            selectedObject = null;
        }
    }

    // Update is called once per frame
    void Update () {
        // extend hand when above or below eye-line
        float handVerticalOrientation = 2.0f - Mathf.Abs(2.0f - playerHand.transform.rotation.eulerAngles.x / 90f);
        /* Vector3 h = playerHand.transform.position - player.transform.position - controllerPivot.transform.localPosition;
        playerHand.transform.localPosition = initialHandOffset + handOffset * ( handOffset.z * h.magnitude / h.z);*/
        playerHand.transform.localPosition = initialHandOffset + handOffset * (handVerticalOrientation * handVerticalOrientation);
        
        if ( canInteract )
        {
            if ( GvrController.AppButtonUp || GvrController.ClickButtonUp )
            {
                interacting = false;
                selectedObject.GetComponent<ObjectInteract>().unregister();
            }
            if (  (selectedObject != null) && GvrController.AppButtonDown || GvrController.ClickButtonDown )
            {
                interacting = true;
                selectedObject.GetComponent<ObjectInteract>().register();
            }

            if ( interacting )
            {
                Debug.DrawLine(selectedObject.transform.position, playerHand.transform.position, Color.blue);
                //debugBegin.transform.position = selectedObject.transform.position;
                //debugEnd.transform.position = playerHand.transform.position;
                //debugInfo.outsideText = "line: " + selectedObject.transform.position.ToString() + playerHand.transform.position.ToString();
            }
        }
    }

    void FixedUpdate()
    {
        if (interacting)
        {
            onInteract();
        }
    }
}
