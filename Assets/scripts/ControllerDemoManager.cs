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

// The controller is not available for versions of Unity without the
// // GVR native integration.
#if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)

using UnityEngine;
using UnityEngine.UI;

public class ControllerDemoManager : MonoBehaviour
{
    public GameObject controllerPivot;
    public GameObject character;
    public GameObject playerCamera;
    public GameObject messageCanvas;
    public Text messageText;

    public ControllerDebugInfo debugInfo;
    public float deadZone;

    private CharacterManager characterManager;
    private GameObject playerCollider;

    // testing manipulating objects
    private Quaternion objectStartRotation;
    private Quaternion elbowStartRotation;

    void Awake()
    {
        playerCollider = GameObject.Find("playerCollider");
        character.SetActive(true);
        GameObject.Find("towerInterior").SetActive(false);
    }

    void Start()
    {
        characterManager = GameObject.Find("character").GetComponent<CharacterManager>();
        //debugInfo.outsideText = GameObject.Find("character").ToString();
            //debugInfo.outsideText = characterManager.ToString();
        
    }

    void Update()
    {
        UpdatePointer();
        UpdateStatusMessage();

        // rotate the player collider box only on the y-axis        
        playerCollider.GetComponent<BoxCollider>().transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);
            //debugInfo.outsideText = "camera y rotation: " + (playerCamera.transform.rotation.eulerAngles) + ": " + playerCamera.transform.rotation.y;//.localRotation = playerCamera.transform.rotation;
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
            moveVector.y = Mathf.Clamp(-moveVector.y, -0.81f, 1.0f);
            var magnitude = moveVector.magnitude;

            if (magnitude < deadZone)
                moveVector = Vector2.zero;


            var direction = moveVector.normalized;

            // clean up input values, avoid the dead zone, and normalize -1 to +1
            magnitude = magnitude * magnitude * (magnitude - deadZone) / (1.0f - deadZone); // use cubic function to achieve more precision at slow speeds

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
        controllerPivot.transform.rotation = character.transform.rotation * GvrController.Orientation;// * character.transform.rotation;
            //Debug.DrawRay(controllerPivot.transform.position, Vector3.forward, Color.white, 0.1f, true);
            //Debug.DrawRay(controllerPivot.transform.position, controllerPivot.transform.rotation * Vector3.forward, Color.green);
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

#endif  // UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
