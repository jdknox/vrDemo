using UnityEngine;
using System.Collections;

public class TowerTrigger : MonoBehaviour {

    private GameObject hourHand;
    private GameObject minuteHand;
    private GameObject gearHour;
    private GameObject gearMinute;


    // Use this for initialization
    void Awake () {
        hourHand = GameObject.Find("hourHand");
        minuteHand = GameObject.Find("minuteHand");
        gearHour = GameObject.Find("gearHour");
        gearMinute = GameObject.Find("gearMinute");
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    // update clock hands when player leaves the tower
    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "playerHand")
        {
            float minuteAngle = Mathf.Round((gearMinute.transform.localRotation.eulerAngles.z % 360) / 6) * 6f;
            float hourAngle = Mathf.Round((gearHour.transform.localRotation.eulerAngles.z % 360) / 12) * 12f;

            minuteHand.transform.localRotation = Quaternion.AngleAxis(minuteAngle, Vector3.forward);
            hourHand.transform.localRotation = Quaternion.AngleAxis(hourAngle + minuteAngle / 12f, Vector3.forward);
        }
    }
}
