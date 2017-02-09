using UnityEngine;
using System.Collections.Generic;

public class EnterPortal : MonoBehaviour {

    private List<GameObject> objectsToHide = new List<GameObject>();

	// Use this for initialization
	void Awake () {
        string[] objectNames = { "big_tree", "treeTrunk (4)" };
        foreach (var objectName in objectNames)
        {
            objectsToHide.Add(GameObject.Find(objectName));
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ///Debug.Log("hide " + objectsToHide.Count + " objects: " + objectsToHide.ToString());
            foreach (var gameObject in objectsToHide)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var objectToHide in objectsToHide)
            {
                objectToHide.SetActive(true);
            }
        }
    }
}
