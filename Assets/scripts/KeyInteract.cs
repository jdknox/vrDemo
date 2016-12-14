using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyInteract : ObjectInteract
{

    public StonePlate stonePlate;

    private GameObject playerHand;
    //private GameObject stonePlate;
    private Renderer stonePlateRenderer;
    private int lightmapIndex;
    private Vector4 bakedScaleOffset;

    // Use this for initialization
    void Awake () {
        playerHand = GameObject.FindGameObjectWithTag("playerHand");
        stonePlateRenderer = stonePlate.GetComponent<Renderer>();
        //stonePlate = _stonePlate.GetComponent<StonePlate>();

        List<LightmapData> lm = new List<LightmapData>(LightmapSettings.lightmaps);
        LightmapData newLightmap = new LightmapData();
        newLightmap.lightmapFar = stonePlate.unlitLightMap;
        lm.Add(newLightmap);
        LightmapSettings.lightmaps = lm.ToArray();

        // save lightmap index for later
        lightmapIndex = LightmapSettings.lightmaps.Length - 1; //stonePlateRenderer.lightmapIndex;
        for (int i = 0; i < 4; i++)
        {
            bakedScaleOffset[i] = PlayerPrefs.GetFloat("lightBaking_stonePlate" + "_" + i);
            Debug.Log("lightBaking_stonePlate" + "_" + i + " = " + bakedScaleOffset[i]);
        }
        
        // remove lightmap until the key is collected
        stonePlateRenderer.lightmapIndex = -1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void register()
    {
        HandInteraction.onInteract += interact;

        // pick up key
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = playerHand.transform;
        transform.localPosition = Vector3.back * 4.0f;
        transform.localRotation = Quaternion.identity;
        GetComponent<BoxCollider>().isTrigger = true;

        removeKeyLight();
    }
    public override void unregister()
    {
        HandInteraction.onInteract -= interact;
        
    }

    // "unlight" stone: change texture to plain stone and put the lightmap back
    public void removeKeyLight()
    {
        stonePlateRenderer.lightmapIndex = lightmapIndex;

        stonePlateRenderer.lightmapScaleOffset = bakedScaleOffset;//new Vector4(0.05517578125f, 0.05517578125f, 0.1923828125f, 0.771484375f);
        stonePlateRenderer.material.mainTexture = stonePlate.stonePlateUnlitTexture;
    }

    public void dropKey()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        stonePlateRenderer.material.mainTexture = stonePlate.stonePlateKeyTexture;
    }

    private void interact()
    {
        
    }

    /* for runtime scripting:
Renderer r = GameObject.Find("stonePlate").GetComponent<Renderer>();
r.lightmapIndex = 7;

var lm = LightmapSettings.lightmaps;
lm[6] = lm[5];
LightmapSettings.lightmaps = lm;
Renderer r = GameObject.Find("stonePlate").GetComponent<Renderer>();
r.lightmapIndex = 5;
Debug.Log(lm[6].lightmapFar);

var keyInteract = GameObject.FindObjectOfType(typeof(KeyInteract)) as KeyInteract;
keyInteract.removeKeyLight();
Debug.Log(keyInteract.stonePlate.GetComponent<Renderer>().lightmapScaleOffset);
Debug.Log(PlayerPrefs.GetFloat("lightBaking_stonePlate" + "_" + "0"));
     */
}
