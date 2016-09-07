using UnityEngine;
using System.Collections;

public class DoorInteract : ObjectInteract {

    public ControllerDebugInfo debugInfo;

    private Rigidbody doorBody;
    public GameObject towerInterior;

    // Use this for initialization
    void Start () {
        //playerHand = GameObject.FindGameObjectWithTag("playerHand");
        doorBody = GetComponent<Rigidbody>();
        //towerInterior = GameObject.Find("stoneClockTowerInterior");
    }

    public override void register()
    {
        HandInteraction.onInteract += open;
    }
    public override void unregister()
    {
        HandInteraction.onInteract -= open;
    }

    private void open()
    {
        doorBody.isKinematic = false;
        //Debug.Log(GameObject.Find("towerInterior"));
        towerInterior.SetActive(true);
        towerInterior.GetComponent<MeshRenderer>().enabled = true;
        
    }
}
