using UnityEngine;
using System.Collections;

public class GearRetract : MonoBehaviour {

    private GameObject gearHour;
    private GameObject gearMinute;

    // for retracting
    private ConstantForce retractForce;
    private Vector3 extendedPosition;
    public float retractThreshold;
    public bool _retractGears = false;

    // Use this for initialization
    void Awake () {
        gearHour = GameObject.Find("gearHour");
        gearMinute = GameObject.Find("gearMinute");

        extendedPosition = transform.localPosition;
        retractForce = GetComponent<ConstantForce>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_retractGears)
        {
            //GetComponent<Rigidbody>().isKinematic = true;
            retractForce.relativeForce = new Vector3(0, 5, 0);

            if (transform.localPosition.y - extendedPosition.y > retractThreshold)
            {
                retractForce.relativeForce = Vector3.zero;
                _retractGears = false;
            }
        }
    }

    public void retractGears()
    {
        _retractGears = true;
        gearHour.GetComponent<Rigidbody>().isKinematic = true;
        gearMinute.GetComponent<Rigidbody>().isKinematic = true;
    }
}
