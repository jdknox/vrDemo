using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyInteract : ObjectInteract
{

    public StonePlate stonePlate;
    public GvrAudioSource keyDropAudio;
    //public LightmapManager lightmapManager;

    private GameObject playerHand;
    //private GameObject stonePlate;
    private Renderer stonePlateRenderer;
    private int lightmapIndex;

    // Use this for initialization
    void Awake ()
    {
        playerHand = GameObject.FindGameObjectWithTag("playerHand");
        stonePlateRenderer = stonePlate.GetComponent<Renderer>();
        //#lightmapManager = GetComponent<LightmapManager>();

        lightmapIndex = pushLightmap(stonePlate.unlitLightmap);

        // remove lightmap until the key is collected
        stonePlateRenderer.lightmapIndex = -1;
    }

    private int pushLightmap(Texture2D lightmap)
    {
        // add new lightmap to LightmapSettings.lightmaps array
        List<LightmapData> lm = new List<LightmapData>(LightmapSettings.lightmaps);
        LightmapData newLightmap = new LightmapData();
        newLightmap.lightmapFar = lightmap;
        lm.Add(newLightmap);
        LightmapSettings.lightmaps = lm.ToArray();

        // return new lightmap index
        return LightmapSettings.lightmaps.Length - 1;
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        keyDropAudio.Play();

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

    // "unlight" stone: change texture to plain stone and put the original lightmap back
    public void removeKeyLight()
    {
        stonePlateRenderer.lightmapIndex = lightmapIndex;

        //Vector4 bakedScaleOffset = LightmapManager.loadVector("lightBaking_stonePlate_");
        //stonePlateRenderer.lightmapScaleOffset = stonePlate.unlitBakedLightmapScaleOffset;
        stonePlateRenderer.material.mainTexture = stonePlate.stonePlateUnlitTexture;
    }

    public void dropKey()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        gameObject.tag = "interactable";
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
