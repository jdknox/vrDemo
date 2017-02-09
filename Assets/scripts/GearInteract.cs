using UnityEngine;
using System.Collections;

public class GearInteract : ObjectInteract {

    public GameObject stonePlate;
    private TowerTrigger towerTrigger;
    private KeyInteract keyInteract;
    private GearRetract gearRetract;
    private GameObject playerHand;
    
    // for rotation
    private Vector3 beginInteractVector;
    private Vector3 hingeAxis;
    private Quaternion beginOrientation;
    private Quaternion endOrientation;
    private float currentAngularVelocity;
    
	// Use this for initialization
	void Awake () {
        playerHand = GameObject.FindGameObjectWithTag("playerHand");
        hingeAxis = transform.TransformDirection(GetComponent<HingeJoint>().axis);

        towerTrigger = GameObject.FindObjectOfType(typeof(TowerTrigger)) as TowerTrigger;
        keyInteract = GameObject.FindObjectOfType(typeof(KeyInteract)) as KeyInteract;
        gearRetract = GameObject.FindObjectOfType(typeof(GearRetract)) as GearRetract;
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

        if( towerTrigger.checkClockTime() )
        {
            gearRetract.retractGears();
            keyInteract.dropKey();
        }
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

/*
KeyInteract keyInteract = GameObject.FindObjectOfType(typeof(KeyInteract)) as KeyInteract;
keyInteract.dropKey();
 */
