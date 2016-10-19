using UnityEngine;
using System.Collections;

public class KeyInteract : MonoBehaviour {

    public GameObject stonePlate;
    public Texture stonePlateTexture;

    private int lightmapIndex;
    private Renderer stonePlateRenderer;

	// Use this for initialization
	void Awake () {
        // save lightmap index for later
        stonePlateRenderer = stonePlate.GetComponent<Renderer>();
        lightmapIndex = stonePlateRenderer.lightmapIndex;

        // remove lightmap until the key is collected
        stonePlateRenderer.lightmapIndex = -1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // "unlight" stone: change texture to plain stone and put the lightmap back
    public void removeKeyLight()
    {
        stonePlateRenderer.lightmapIndex = lightmapIndex;
        stonePlateRenderer.material.mainTexture = stonePlateTexture;
    }
}
