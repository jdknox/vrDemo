using UnityEngine;
using System.Collections;

public class CrankInteract : ObjectInteract
{
    private GameObject playerHand;
    private GameObject crankMechanismShadow;

    private bool connectedToClock = false;

	void Awake () {
        playerHand = GameObject.FindGameObjectWithTag("playerHand");
        crankMechanismShadow = GameObject.Find("crankMechanismShadow");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void register()
    {
        if ( connectedToClock )
        {
            HandInteraction.onInteract += rotate;
        }
        else
        {
            HandInteraction.onInteract += holding;

            Debug.Log("old parent: " + gameObject.transform.parent.name);
            gameObject.transform.SetParent(playerHand.transform);
            gameObject.transform.localPosition = new Vector3(1, 2.5f, 6);
            gameObject.transform.localRotation = Quaternion.Euler(90, 90, -100);
            crankMechanismShadow.SetActive(false);
        }
    }
    public override void unregister()
    {
        if ( connectedToClock )
        {
            HandInteraction.onInteract -= rotate;
        }
        else
        {
            HandInteraction.onInteract -= holding;
        }
    }

    private void rotate()
    {
    }

    private void holding()
    { }
}
