// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissioßns and
// limitations under the License.

using System;
using UnityEngine;
using UnityEngine.UI;

public class ControllerDemoManager : MonoBehaviour
{
    public GameObject controllerPivot;
    public GameObject character;
    public GameObject playerCamera;
    public GameObject messageCanvas;
    public Text messageText;

    public Material cubeInactiveMaterial;
    public Material cubeHoverMaterial;
    public Material cubeActiveMaterial;

    public ControllerDebugInfo debugInfo;
    public float deadZone;

    //private Renderer controllerCursorRenderer;

    // Currently selected GameObject.
    private GameObject selectedObject;
    private GameObject towerDoor;
    private CharacterManager characterManager;

    // True if we are dragging the currently selected GameObject.
    private bool dragging;

    void Awake()
    {
        towerDoor = GameObject.Find("towerDoorPivot");
        
        character.SetActive(true);
    }

    void Start()
    {
        characterManager = GameObject.Find("character").GetComponent<CharacterManager>();
        debugInfo.outsideText = GameObject.Find("character").ToString();
        //debugInfo.outsideText = characterManager.ToString();
    }

    void Update()
    {
        UpdatePointer();
        UpdateStatusMessage();
        //UpdateCharacterPosition();
        updateDoor();
    }

    private void updateDoor()
    {
        float maxExtent = 160f;
        float minExtent = 0.0f;

        var angle = Mathf.Lerp(minExtent, maxExtent, Mathf.Sin(Time.time));
        //towerDoor.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //Quaternion.
        //towerDoor.transform.Rotate(Vector3.up, angle * Time.deltaTime, Space.World);
        
        //characterManager.ToString();
    }

    void FixedUpdate()
    {
        UpdateCharacterPosition();
    }

    private void UpdateCharacterPosition()
    {
        //character.SetActive(true);

        //debugInfo.outsideText = "dir: " + playerCamera.transform.forward.ToString() + "\n";

        if (GvrController.IsTouching)
        {
            float fudgeFactor = 1.43f;  // daydream controller doesn't go all the way to 1.0 with the printed cover

            var moveVector = 2 * GvrController.TouchPos - Vector2.one;
            moveVector.y = Mathf.Clamp(-moveVector.y, -0.5f, 1.0f);
            var magnitude = moveVector.magnitude;

            if (magnitude < deadZone)
                moveVector = Vector2.zero;


            var direction = moveVector.normalized;
            //debugInfo.outsideText = "pre input: " + moveVector.ToString() + "\n";

            // clean up input values, avoid the dead zone, and normalize -1 to +1

            ;
            magnitude = (magnitude - deadZone) / (1.0f - deadZone);
            //magnitude = Mathf.Clamp(magnitude, 0.0f, cm.maxPlayerSpeed);
            moveVector = direction * magnitude * Time.deltaTime * characterManager.maxPlayerSpeed * fudgeFactor;
            //debugInfo.outsideText += "final move vec: " + moveVector.ToString() + "\n";
            //debugInfo.outsideText += "final move speed: " + moveVector.magnitude.ToString() + "\n";

            var oldY = character.transform.position.y;
            character.transform.position += playerCamera.transform.right * moveVector.x;
            character.transform.position += playerCamera.transform.forward * moveVector.y;
            character.transform.position = new Vector3(character.transform.position.x, oldY, character.transform.position.z);
        }
    }

    private void UpdatePointer()
    {

        //Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward) * 10;

        if (GvrController.State != GvrConnectionState.Connected)
        {
            controllerPivot.SetActive(false);
        }
        controllerPivot.SetActive(true);
        controllerPivot.transform.rotation = GvrController.Orientation;

        if (dragging)
        {
            if (GvrController.TouchUp)
            {
                EndDragging();
            }
        }
        else
        {  // nothing picked up yet
            RaycastHit hitInfo;
            Vector3 rayDirection = GvrController.Orientation * Vector3.forward;
            Debug.DrawRay(playerCamera.transform.position, rayDirection * 50f, Color.green, 0f);

            if (Physics.Raycast(playerCamera.transform.position, rayDirection, out hitInfo))
            {
                if (hitInfo.collider && hitInfo.collider.gameObject && hitInfo.collider.gameObject.tag == "interactable")
                {
                    SetSelectedObject(hitInfo.collider.gameObject);
                }
            }
            else
            {
                SetSelectedObject(null);
            }
            if (GvrController.ClickButtonDown && selectedObject != null)
            {
                StartDragging();
            }
        }
    }

    private void SetSelectedObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<Renderer>().material = cubeInactiveMaterial;
        }
        if (obj != null)
        {
            obj.GetComponent<Renderer>().material = cubeActiveMaterial;
        }
        selectedObject = obj;
    }

    private void StartDragging()
    {
        dragging = true;
        selectedObject.GetComponent<Renderer>().material = cubeHoverMaterial;

        // Reparent the active cube so it's part of the ControllerPivot object. That will
        // make it move with the controller.
        selectedObject.transform.SetParent(controllerPivot.transform, true);
    }

    private void EndDragging()
    {
        dragging = false;
        selectedObject.GetComponent<Renderer>().material = cubeActiveMaterial;

        // Stop dragging the cube along.
        selectedObject.transform.SetParent(null, true);
    }

    private void UpdateStatusMessage()
    {
        // This is an example of how to process the controller's state to display a status message.
        switch (GvrController.State)
        {
            case GvrConnectionState.Connected:
                messageCanvas.SetActive(false);
                break;
            case GvrConnectionState.Disconnected:
                messageText.text = "Controller disconnected.";
                messageText.color = Color.white;
                messageCanvas.SetActive(true);
                break;
            case GvrConnectionState.Scanning:
                messageText.text = "Controller scanning...";
                messageText.color = Color.cyan;
                messageCanvas.SetActive(true);
                break;
            case GvrConnectionState.Connecting:
                messageText.text = "Controller connecting...";
                messageText.color = Color.yellow;
                messageCanvas.SetActive(true);
                break;
            case GvrConnectionState.Error:
                messageText.text = "ERROR: " + GvrController.ErrorDetails;
                messageText.color = Color.red;
                messageCanvas.SetActive(true);
                break;
            default:
                // Shouldn't happen.
                Debug.LogError("Invalid controller state: " + GvrController.State);
                break;
        }
    }
}
