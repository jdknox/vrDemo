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
    private string updatedSettings = "N/A";
    //private ArrayList unhideMeshes = new ArrayList { "towerInterior" };

    [MenuItem("Window/Custom Bake")]
    
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<CustomBakeEditor>();
    }

    public bool allowBaking()
    {
        return SceneManager.GetActiveScene().name == "lightBakingScene";
    }

    void OnEnable()
    {
        if (!allowBaking())
        {
            return;
        }
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
        // only save the objects to back if we're in the light baking scene
        if ( !allowBaking() )
        {
            Debug.Log("inside light baking scene");
            return;
        }
        string bakedObjectsString = gameObjectsToString() + ",";
        bakedObjectsString += string.Join(",", inactiveObjects.ToArray());

        EditorPrefs.SetString(BAKED_OBJECTS, bakedObjectsString);
        Debug.Log("saving: '" + bakedObjectsString + "'\n count: " + bakedObjects.Count);
    }

    void OnHierarchyChange()
    {
        // close and reopen window if anything changes, forced refresh
        if (safeToShow)
        {
            this.Close();
            safeToShow = false;     // mutex of sorts, to allow window to update, but not in an infinite loop
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
        if (!allowBaking())
        {
            GUILayout.Label("Disabled except for light baking scene.", EditorStyles.boldLabel);
            return;
        }

        GUILayout.Label("Lights to hide during baking", EditorStyles.boldLabel);
        lightToIgnore = (GameObject)EditorGUI.ObjectField(new Rect(3, 24, position.width - 6, 20), "Light to Ignore", lightToIgnore, typeof(GameObject), true);
        
        GUILayout.Space(20);
        GUILayout.Label("Objects for custom lightmap", EditorStyles.boldLabel);

        if ( bakedObjects != null )
        {
            ///Debug.Log("number of baked objects: " + bakedObjects.Count);
            /// Populate window with GameObjects to bake
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

            /// Save the scaleOffsets for each active baked object
            if ( GUILayout.Button("Save offsets") )
            {
                updatedSettings = updateBakedObjectOffsets();
            Debug.Log(updatedSettings);
            }
            EditorGUILayout.SelectableLabel(updatedSettings);
        }

        GUILayout.Space(20);

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

        Lightmapping.completed -= onFinishBake;
        Debug.Log("FINISHED BAKING");
    }

    string updateBakedObjectOffsets()
    {
        string updatedSettings = "";

        /// TODO: update this to use the game objects provided by the GUI above
        GameObject currentGameObject = GameObject.Find("lightBaking_stonePlate");
        if ( currentGameObject )
        {
            var offset = currentGameObject.GetComponent<Renderer>().lightmapScaleOffset;
            updatedSettings += LightmapManager.saveVector(currentGameObject.name, offset);
        }
        return updatedSettings;
    }
}
