using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

    public GameObject pitchObject;
    public Camera endCameraR;
    public const float magic = 2.03f; ///0.87f;
    public float magicAngle = 10;

    private GameObject playerCamera;
    private GameObject portal;
    private Camera endCamera;

    private Vector3 portalPosition;
    private Vector3 portalForward;

    private const float twoRad2Deg = 2 * Mathf.Rad2Deg;

	// Use this for initialization
	void Awake () {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        portal = GameObject.FindGameObjectWithTag("portal");
        endCamera = GetComponent<Camera>();

        portalForward = portal.transform.rotation * Vector3.forward;
        portalPosition = portal.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //manualCameraUpdate();
        smartCameraUpdate();
    }

    void smartCameraUpdate()
    {
        Vector3 direction = playerCamera.transform.position - portalPosition;

        float fov_2 = Mathf.Atan(1f / direction.magnitude);
        float tanF_2 = Mathf.Tan(fov_2) / 2;
        endCamera.fieldOfView = twoRad2Deg * fov_2;

        direction.y = 0;
        float x = Vector3.Dot(-portal.transform.right, direction);
        /*Debug.DrawRay(portal.transform.position, -portal.transform.forward);
        Debug.DrawRay(portal.transform.position, direction, Color.yellow);
        Debug.DrawRay(playerCamera.transform.position, -portal.transform.right * x, Color.red);*/

        float phi = twoRad2Deg * Mathf.Atan((playerCamera.transform.position.y - magic) * tanF_2);
        float theta = twoRad2Deg * Mathf.Atan((x) * tanF_2);
        ///Debug.Log("right: " + -portal.transform.right + "\nendCamera yaw (deg): " + theta);

        Vector3 finalRotation = new Vector3(phi, theta, 0);
        transform.localEulerAngles = finalRotation;
        finalRotation.y += magicAngle;
        endCameraR.transform.localEulerAngles = finalRotation;
        endCameraR.fieldOfView = endCamera.fieldOfView;
    }

    void manualCameraUpdate()
    {
        Vector3 beginPosition = playerCamera.transform.position;
        //Debug.DrawRay(beginPosition, portalForward, Color.red);

        Vector3 directionXZ = portalPosition - playerCamera.transform.position;   // direction = portalPosition - beginPosition (for debug rays)
        Vector3 directionYZ = directionXZ;
        endCamera.fieldOfView = 40 / directionYZ.magnitude + 36;

        directionXZ.y = 0;
        //Debug.DrawRay(beginPosition, directionXZ);

        //directionYZ.y = -directionYZ.y;
        float pitchAngle = Mathf.Sign(-directionYZ.y) * Vector3.Angle(directionXZ, directionYZ);
        /*Debug.Log("pitch: " + pitchAngle);
        Debug.DrawRay(beginPosition, directionXZ, Color.red);
        Debug.DrawRay(beginPosition, directionYZ, Color.yellow);*/

        // rotate the portal camera to match the player's orientation to the portal
        Quaternion cameraYaw = Quaternion.FromToRotation(portalForward, directionXZ);
        transform.localRotation = cameraYaw;
        /*Debug.DrawRay(beginPosition, cameraYaw * portalForward, Color.green);*/

        // remove roll from camera angle
        Vector3 direction = transform.localEulerAngles;
        direction.x = pitchAngle;
        direction.z = 0;
        transform.localEulerAngles = direction;
        //Debug.Log(transform.localEulerAngles);
    }
}
