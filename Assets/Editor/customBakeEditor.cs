using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class CustomBakeEditor : EditorWindow
{

    [SerializeField]
    public GameObject lightToIgnore;
    public List<GameObject> bakedObjects;

    private List<string> inactiveObjects;
    const string BAKED_OBJECTS = "bakedObjects";
    private static bool safeToShow = true;
    private static int crazy = 0;
    //private ArrayList unhideMeshes = new ArrayList { "towerInterior" };

    [MenuItem("Window/Custom Bake")]
    
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<CustomBakeEditor>();
    }

    void OnEnable()
    {
        bakedObjects = new List<GameObject>();
        inactiveObjects = new List<string>();
        this.autoRepaintOnSceneChange = true;

        foreach (var objectString in EditorPrefs.GetString(BAKED_OBJECTS).Split(','))
        {
            if ( objectString.Length > 0 )
            {
                GameObject gameObj = GameObject.Find(objectString);
                if (gameObj)
                {
                    bakedObjects.Add(gameObj);
                }
                else
                {
                    inactiveObjects.Add(objectString);
                }
                
                ///Debug.Log("loading " + gameObj + "\nnew count: " + bakedObjects.Count);
            }
        }
        ///Debug.Log("inactive objects: " + string.Join(",", inactiveObjects.ToArray()));
    }

    void OnDisable()
    {
        string bakedObjectsString = gameObjectsToString() + ",";
        bakedObjectsString += string.Join(",", inactiveObjects.ToArray());
        EditorPrefs.SetString(BAKED_OBJECTS, bakedObjectsString);
        Debug.Log("saving: '" + bakedObjectsString + "'\n count: " + bakedObjects.Count);
    }

    void OnHierarchyChange()
    {
        if (safeToShow)
        {
            this.Close();
            safeToShow = false;
            CustomBakeEditor.ShowWindow();
        } else {
            safeToShow = true;
        }
    }

    private string gameObjectsToString()
    {
        return string.Join(",", (from x in bakedObjects select x ? x.name : "").ToArray());
    }
    
    void OnGUI()
    {
        GUILayout.Label("Lights to hide during baking", EditorStyles.boldLabel);
        lightToIgnore = (GameObject)EditorGUI.ObjectField(new Rect(3, 24, position.width - 6, 20), "Light to Ignore", lightToIgnore, typeof(GameObject), true);
        
        GUILayout.Space(20);
        GUILayout.Label("Objects for custom lightmap", EditorStyles.boldLabel);
        //Rect rect = EditorGUILayout.BeginVertical();
        if ( bakedObjects != null )
        {
            ///Debug.Log("number of baked objects: " + bakedObjects.Count);
            for (int i = 0; i < bakedObjects.Count; i++)
            {
                bakedObjects[i] = (GameObject)EditorGUILayout.ObjectField("Object " + i, bakedObjects[i], typeof(GameObject), true);
            }
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Inactive Objects");
                EditorGUILayout.SelectableLabel(string.Join(" ", inactiveObjects.ToArray()));
            }
            EditorGUILayout.EndHorizontal();

            if ( GUILayout.Button("Add") )
            {
                bakedObjects.Add((GameObject)EditorGUILayout.ObjectField("_", null, typeof(GameObject), true));
            }
        }
        //EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        //bakeSelected = EditorGUILayout.Toggle("Bake Selected Only", bakeSelected);
        if ( GUILayout.Button("Build Lighting") )
        {
            if (lightToIgnore)
            {
                lightToIgnore.GetComponent<Light>().enabled = false;
            }

            Debug.Log("bake all lighting, ignore: " + lightToIgnore);
            Lightmapping.BakeAsync();
            
            Lightmapping.completed += onFinishBake;
        }
    }

    void onFinishBake()
    {
        if (lightToIgnore)
        {
            lightToIgnore.GetComponent<Light>().enabled = true;
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
