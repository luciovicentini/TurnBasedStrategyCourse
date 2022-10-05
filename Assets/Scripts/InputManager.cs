using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one InputManager Instance! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Vector2 GetMouseScreenPosition() {
        return Input.mousePosition;
    }

    public bool IsMouseLeftButtonDown() {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetCameraMoveDirection() {
        Vector2 inputMoveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) {
            inputMoveDir.y = +1f;
        }

        if (Input.GetKey(KeyCode.S)) {
            inputMoveDir.y = -1f;
        }

        if (Input.GetKey(KeyCode.A)) {
            inputMoveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D)) {
            inputMoveDir.x = +1f;
        }

        return inputMoveDir;
    }

    public float GetCameraRotationAmount() {
        float rotation = 0;
        if (Input.GetKey(KeyCode.Q)) {
            rotation = +1f;
        }
        if (Input.GetKey(KeyCode.E)) {
            rotation = -1f;
        }

        return rotation;
    }

    public float GetCameraZoomAmount() {
        float zoomAmount = 0;
        if (Input.mouseScrollDelta.y > 0) {
            zoomAmount = 1f;
        }

        if (Input.mouseScrollDelta.y < 0) {
            zoomAmount = -1f;
        }

        return zoomAmount;
    }
}
