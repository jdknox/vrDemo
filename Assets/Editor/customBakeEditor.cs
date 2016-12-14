using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;

public class CustomBakeEditor : EditorWindow
{
    bool bakeSelected = false;

    [SerializeField]
    public GameObject obj;
    //private ArrayList unhideMeshes = new ArrayList { "towerInterior" };

    [MenuItem("Window/Custom Bake")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomBakeEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        GUILayout.Label("", EditorStyles.boldLabel);
        //obj = GameObject.Find("specularDayLight");
        obj = (GameObject)EditorGUI.ObjectField(new Rect(3, 24, position.width - 6, 20), "Light to Ignore", obj, typeof(GameObject), true);

        bakeSelected = EditorGUILayout.Toggle("Bake Selected Only", bakeSelected);
        if ( GUILayout.Button("Build Lighting") )
        {
            if (obj)
            {
                obj.GetComponent<Light>().enabled = false;
            }

            if ( bakeSelected )
            {
                Debug.Log("bake selected lighting, ignore: " + obj);
                Lightmapping.BakeSelectedAsync();
            } else {
                Debug.Log("bake all lighting, ignore: " + obj);
                Lightmapping.BakeAsync();
            }
            
            Lightmapping.completed += onFinishBake;
        }
    }

    void onFinishBake()
    {
        if (obj)
        {
            obj.GetComponent<Light>().enabled = true;
        }

        if ( SceneManager.GetActiveScene().name == "lightBakingScene" )
        {
            updateBakedObjectOffsets();
        }

        Lightmapping.completed -= onFinishBake;
        Debug.Log("FINISHED BAKING");
    }

    void updateBakedObjectOffsets()
    {
        GameObject currentGameObject = GameObject.Find("lightBaking_stonePlate");
        if ( currentGameObject )
        {
            var offset = currentGameObject.GetComponent<Renderer>().lightmapScaleOffset;
            for (int i = 0; i < 4; i++)
            {
                PlayerPrefs.SetFloat(currentGameObject.name + "_" + i, offset[i]);
                Debug.Log(currentGameObject.name + "_" + i + " = " + offset[i]);
            }
        }
    }
}
