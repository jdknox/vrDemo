using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateClockHands : MonoBehaviour {

    public Projector projector;
    public GameObject hand;
    public ControllerDebugInfo debugInfo;
    //public Dictionary<string, int> time;

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

        // this is a mess, but it updates the projected shadows (to avoid realtime shadows)
        float angle = hand.transform.localRotation.eulerAngles.z;
        float angleOffset = -34f;
        float magic = 67f;

        // update the y-axis of the projector to line up with the light
        float phi = Mathf.Cos(Mathf.Deg2Rad * (angle + angleOffset));
        float sign = Mathf.Sign(phi);
        float yAngle = magic * sign * Mathf.Pow(sign * phi, 0.6f);

        // update the x-axis
        magic = 57;
        phi = Mathf.Sin(Mathf.Deg2Rad * (angle + angleOffset));
        sign = Mathf.Sign(phi);
        float xAngle = magic * sign * Mathf.Pow(sign * phi, 1.05f);

        // shift the projector so it centers on the hour/minute hand pivot
        float xOffset = 0.04f * Mathf.Cos(Mathf.Deg2Rad * (angle - 45f));

        // scale the projcted image so it doesn't get too big at certain angles
        magic = -0.45f;
        phi = Mathf.Cos(Mathf.Deg2Rad * (angle - 120f));
        sign = Mathf.Sign(phi);
        float orthoOffset = magic * Mathf.Pow(sign * phi, 1.6f);

        // actual rotation once the values are calculated
        gameObject.transform.localRotation = Quaternion.Euler(xAngle, yAngle, 0f);
        projector.transform.localPosition = new Vector3(xOffset, 0f, zOffset);
        projector.orthographicSize = orthoSize + orthoOffset;
    }
}
