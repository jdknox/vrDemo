using UnityEngine;
using System.Collections;

public class UpdateClockHands : MonoBehaviour {

    public Projector projector;
    public GameObject hand;
    public ControllerDebugInfo debugInfo;

    private float zOffset;
    private float orthoSize;

	// Use this for initialization
	void Start () {
        zOffset = projector.transform.localPosition.z;
        orthoSize = projector.orthographicSize;

        updateClock();
	}
	
	// Update is called once per frame
	// void Update () {
    public void updateClock () {
        float angle = hand.transform.localRotation.eulerAngles.z;
        float angleOffset = -34f;
        float magic = 67f;

        float phi = Mathf.Cos(Mathf.Deg2Rad * (angle + angleOffset));
        float sign = Mathf.Sign(phi);
        float yAngle = magic * sign * Mathf.Pow(sign * phi, 0.6f);

        magic = 57;
        phi = Mathf.Sin(Mathf.Deg2Rad * (angle + angleOffset));
        sign = Mathf.Sign(phi);
        float xAngle = magic * sign * Mathf.Pow(sign * phi, 1.05f);

        float xOffset = 0.04f * Mathf.Cos(Mathf.Deg2Rad * (angle - 45f));

        magic = -0.45f;
        phi = Mathf.Cos(Mathf.Deg2Rad * (angle - 120f));
        sign = Mathf.Sign(phi);
        float orthoOffset = magic * Mathf.Pow(sign * phi, 1.6f);

        debugInfo.outsideText = xAngle + ", " + yAngle.ToString();
        gameObject.transform.localRotation = Quaternion.Euler(xAngle, yAngle, 0f);
        projector.transform.localPosition = new Vector3(xOffset, 0f, zOffset);
        projector.orthographicSize = orthoSize + orthoOffset;
        //Debug.Log(hand.transform.localRotation.eulerAngles.z);
	}
}
