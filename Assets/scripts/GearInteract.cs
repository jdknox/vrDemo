using UnityEngine;
using System.Collections;

public class GearInteract : ObjectInteract {

    public ControllerDebugInfo debugInfo;
    public GameObject stonePlate;

    private GameObject playerHand;
    private Vector3 beginInteractVector;
    private Vector3 hingeAxis;
    private Quaternion beginOrientation;
    private Quaternion endOrientation;
    private float currentAngularVelocity;

	// Use this for initialization
	void Awake () {
        playerHand = GameObject.FindGameObjectWithTag("playerHand");
        hingeAxis = transform.TransformDirection(GetComponent<HingeJoint>().axis);
    }

    void OnEnable()
    {
    }
    void OnDisable()
    {
    }

	// Update is called once per frame
	void Update () {
	
	}

    public override void register()
    {
        HandInteraction.onInteract += rotate;
        beginInteractVector = playerHand.transform.position - gameObject.transform.position;
        beginInteractVector -= Vector3.Dot(beginInteractVector, hingeAxis) * hingeAxis;     // project to z-plane
        beginOrientation = gameObject.transform.rotation;
        endOrientation = beginOrientation;
    }
    public override void unregister()
    {
        HandInteraction.onInteract -= rotate;
        gameObject.GetComponent<Rigidbody>().angularVelocity = currentAngularVelocity * hingeAxis;
        currentAngularVelocity = 0f;
    }

    private void rotate()
    {
        Vector3 currentInteractVector = playerHand.transform.position - gameObject.transform.position;
        currentInteractVector -= Vector3.Dot(currentInteractVector, hingeAxis) * hingeAxis;     // project to z-plane

        Quaternion totalRotate = Quaternion.FromToRotation(beginInteractVector, currentInteractVector);
        totalRotate *= beginOrientation;

        // save velocity so the gear continues to rotate after letting go
        currentAngularVelocity = Quaternion.Angle(endOrientation, totalRotate) * -Mathf.Sign(Vector3.Cross(beginInteractVector, currentInteractVector).z);
        gameObject.transform.rotation = totalRotate;
        endOrientation = totalRotate;

        // Debug.Log("angluar velocity " + currentAngularVelocity.ToString());
    }
}
