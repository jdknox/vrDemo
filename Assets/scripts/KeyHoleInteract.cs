using UnityEngine;
using System.Collections;

public class KeyHoleInteract : ObjectInteract
{
    private bool keyInRange = false;
    private GameObject key;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("keyhole collide: " + other.name);
        if ( other.name == "key" )
        {
            keyInRange = true;
            key = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "key")
        {
            keyInRange = false;
        }
    }

    public override void register()
    {
        HandInteraction.onInteract += interact;
    }

    public override void unregister()
    {
        HandInteraction.onInteract -= interact;
    }

    private void interact()
    {
        if ( keyInRange )
        {
            //Debug.Log("key in range.");
            key.transform.parent = this.transform;
            key.transform.localPosition = Vector3.zero;
            key.transform.localRotation = Quaternion.identity;
        }
    }
}
