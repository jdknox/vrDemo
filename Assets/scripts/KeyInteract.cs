using UnityEngine;
using System.Collections;

public class KeyInteract : ObjectInteract
{

    public GameObject stonePlate;

    private GameObject playerHand;
    private int lightmapIndex;
    private Renderer stonePlateRenderer;

	// Use this for initialization
	void Awake () {
        playerHand = GameObject.FindGameObjectWithTag("playerHand");

        // save lightmap index for later
        stonePlateRenderer = stonePlate.GetComponent<Renderer>();
        lightmapIndex = stonePlateRenderer.lightmapIndex;

        // remove lightmap until the key is collected
        stonePlateRenderer.lightmapIndex = -1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void register()
    {
        HandInteraction.onInteract += interact;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = playerHand.transform;
        transform.localPosition = Vector3.back * 4.0f;
        GetComponent<BoxCollider>().isTrigger = true;
    }
    public override void unregister()
    {
        HandInteraction.onInteract -= interact;
        
    }

    // "unlight" stone: change texture to plain stone and put the lightmap back
    public void removeKeyLight()
    {
        stonePlateRenderer.lightmapIndex = lightmapIndex;
        stonePlateRenderer.material.mainTexture = stonePlate.GetComponent<StonePlate>().stonePlateUnlitTexture;
    }

    public void dropKey()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        stonePlateRenderer.material.mainTexture = stonePlate.GetComponent<StonePlate>().stonePlateKeyTexture;
    }

    private void interact()
    { }
}
