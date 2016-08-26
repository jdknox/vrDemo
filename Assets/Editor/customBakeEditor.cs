using UnityEngine;
using UnityEditor;
using System.Collections;

public class CustomBakeEditor : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = false;
    float myFloat = 1.23f;

    [SerializeField]
    public GameObject obj;
    private ArrayList unhideMeshes = new ArrayList { "towerInterior" };

    [MenuItem("Window/Custom Bake")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomBakeEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        GUILayout.Label("", EditorStyles.boldLabel);
        obj = (GameObject)EditorGUI.ObjectField(new Rect(3, 24, position.width - 6, 20), "Light to Ignore", obj, typeof(GameObject), true);

        obj = GameObject.Find("specularDayLight");
        

        if ( GUILayout.Button("Build Lighting") )
        {
            Debug.Log("build lighting, ignore: " + obj);
            obj.GetComponent<Light>().enabled = false;

            myBool = Lightmapping.BakeAsync();
            Lightmapping.completed += onFinishBake;
        }
        myBool = EditorGUILayout.Toggle("bakeAsync", myBool);
    }

    void onFinishBake()
    {
        obj.GetComponent<Light>().enabled = true;
        myBool = false;
        Lightmapping.completed -= onFinishBake;
        Debug.Log("FINISHED BAKING");
    }
}
