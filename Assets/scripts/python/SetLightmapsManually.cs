using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode]
public class SetLightmapsManually : MonoBehaviour
{
	public int m_lightmapIndex = 255;
    public Vector4 lightmapScaleOffset = new Vector4(1, 1, 0, 0);
    public Vector2 lightmapPixelOffset;
	public bool m_getLightmapValues = false;
	public bool m_setLightmapValues = false;
	public Texture2D[] m_lightmapArray;
	public bool m_setLightmapArray = false;

    [Multiline]
    public string pythonCodeSource = "";
    public string pythonCodeDestination = "";

    void Update ()
	{
		if (m_setLightmapValues)
		{
			m_setLightmapValues = false;

			Renderer r;

			if (r = GetComponent<Renderer>())
			{
				r.lightmapIndex = m_lightmapIndex;
                r.lightmapScaleOffset = lightmapScaleOffset;
			}
		}

		if (m_getLightmapValues)
		{
			m_getLightmapValues = false;

			Renderer r;
			
			if (r = GetComponent<Renderer>())
			{
				m_lightmapIndex = r.lightmapIndex;
                lightmapScaleOffset = r.lightmapScaleOffset;
                lightmapPixelOffset = new Vector2(lightmapScaleOffset.z, lightmapScaleOffset.w) * LightmapSettings.lightmaps[0].lightmapFar.width;

                updatePythonCode();
			}
		}

		if (m_setLightmapArray)
		{
			m_setLightmapArray = false;

			if (m_lightmapArray.Length > 0)
			{
				LightmapData[] lightmapData = new LightmapData[m_lightmapArray.Length];

				for (int i=0; i<lightmapData.Length; i++)
				{
					lightmapData[i] = new LightmapData();
					lightmapData[i].lightmapFar = m_lightmapArray[i];
				}

				LightmapSettings.lightmaps = lightmapData;
			}
		}
	}

    /// <summary>
    ///  Save lightmap data to string for use in python script.  The python script copies a section of a custom lightmap
    ///  into its correct (x, y) location for easier lightmap swapping on `GameObject`s.
    /// </summary>
    private void updatePythonCode()
    {
        string template = "\n\t'{0}': {{\n\t\t'source': ({1}),\n\t\t'destination': ()\n\t}},";
        string scaleOffset = string.Format("{0}, {1}, {2}", lightmapScaleOffset.x, lightmapScaleOffset.z, lightmapScaleOffset.w);
        pythonCodeSource = string.Format(template, name, scaleOffset).Replace("\t", "    ");
        pythonCodeDestination = scaleOffset;
    }
}
