using UnityEngine;
using System.Collections;


public class TowerTrigger : MonoBehaviour {

    public int HOUR = 2;
    public int MINUTE = 12;

    private GameObject hourHand;
    private GameObject minuteHand;
    private GameObject gearHour;
    private GameObject gearMinute;

    private int hour;
    private int minute;

    private UpdateClockHands[] clockScripts;

    // Use this for initialization
    void Awake () {
        hourHand = GameObject.Find("hourHand");
        minuteHand = GameObject.Find("minuteHand");
        gearHour = GameObject.Find("gearHour");
        gearMinute = GameObject.Find("gearMinute");

        clockScripts = GameObject.FindObjectsOfType(typeof(UpdateClockHands)) as UpdateClockHands[];        
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    // update clock hands when player leaves the tower
    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "playerHand")
        {
            updateClock();
        }
    }

    public bool checkClockTime()
    {
        updateClock();

        return (hour == HOUR) && (minute == MINUTE);
    }

    private void updateClock()
    {
        hour = (int)Mathf.Round((gearHour.transform.localRotation.eulerAngles.z) / 30) % 12;
        minute = (int)Mathf.Round((gearMinute.transform.localRotation.eulerAngles.z) / 6) % 60;

        float hourAngle = hour * 30f;
        float minuteAngle = minute * 6f;

        hourHand.transform.localRotation = Quaternion.AngleAxis(hourAngle + minuteAngle / 12f, Vector3.forward);
        minuteHand.transform.localRotation = Quaternion.AngleAxis(minuteAngle, Vector3.forward);

        foreach (var clockScript in clockScripts)
        {
            clockScript.updateClock();
        }

        Debug.Log("time: " + (hour) + ":" + (minute));
    }
}
