using UnityEngine;
using System.Collections;

public class KeyHoleInteract : ObjectInteract
{
    public GameObject endDoor;
    public GameObject doorFrame;

    private bool keyInRange = false;
    private GameObject key;
    private Renderer doorFrameRenderer;

	// Use this for initialization
	void Awake () {
        doorFrameRenderer = doorFrame.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        ///Debug.Log("keyhole collide: " + other.name);
        if ( other.name == "key" )
        {
            keyInRange = true;
            key = other.gameObject;
            doorFrameRenderer.lightmapIndex = 7;
        }
        else
        {
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "key")
        {
            keyInRange = false;
            if (key.transform.parent != this.transform)     // don't remove lightmap if key is in place
                doorFrameRenderer.lightmapIndex = 0;
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
            key.tag = "Untagged";

            endDoor.GetComponent<Rigidbody>().isKinematic = false;
            doorFrameRenderer.lightmapIndex = 7;
        }
    }
}
