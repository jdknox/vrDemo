using UnityEngine;
using System.Collections;

public class HandInteraction : MonoBehaviour {

    private Animator handAnimator;
    private GameObject player;
    private GameObject playerHand;
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
        handAnimator = playerHand.GetComponent<Animator>();
        initialHandOffset = playerHand.transform.localPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "interactable")
        {
            Debug.Log("hand collides with: " + other.tag.ToString());
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
        if (other.tag == "interactable")
        {
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
        float handVerticalOrientation = 2.0f - Mathf.Abs(2.0f - playerHand.transform.rotation.eulerAngles.x / 90f);
        playerHand.transform.localPosition = initialHandOffset + handOffset * handVerticalOrientation;
        //debugInfo.outsideText = "hand length: " + playerHand.transform.localPosition.z;
        //debugInfo.outsideText += "hand rotation: " + handVerticalOrientation;

        if (canInteract)
        {
            if (GvrController.AppButtonUp)
            {
                interacting = false;
                selectedObject.GetComponent<ObjectInteract>().unregister();
            }
            if (GvrController.AppButtonDown && selectedObject != null)
            {
                interacting = true;
                selectedObject.GetComponent<ObjectInteract>().register();
            }

            if (interacting)
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
