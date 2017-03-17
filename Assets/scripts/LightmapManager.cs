using UnityEngine;
using System.Collections;

public class LightmapManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public static Vector4 loadVector(string prefix)
    {
        Vector4 bakedScaleOffset = Vector4.zero;
        for (int i = 0; i < 4; i++)
        {
            //bakedScaleOffset[i] = EditorPrefs.GetFloat(prefix + i);
        }

        return bakedScaleOffset;
    }

    public static string saveVector(string prefix, Vector4 scaleOffset)
    {
        for (int i = 0; i < 4; i++)
        {
            string key = prefix + "_" + i;
            ///EditorPrefs.SetFloat(key, scaleOffset[i]);
            Debug.Log(key + " = " + scaleOffset[i]);
        }
        return prefix + scaleOffset;
    }
}
