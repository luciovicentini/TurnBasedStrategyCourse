#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }
    private PlayerInputActions _playerInputAction;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one InputManager Instance! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _playerInputAction = new PlayerInputActions();
        _playerInputAction.Player.Enable();
    }

    public Vector2 GetMouseScreenPosition() {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseLeftButtonPressedThisFrame() {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputAction.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    public Vector2 GetCameraMoveDirection() {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputAction.Player.CameraMovement.ReadValue<Vector2>();
#else
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
#endif
    }

    public float GetCameraRotationAmount() {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputAction.Player.CameraRotate.ReadValue<float>();
#else
        float rotation = 0;
        if (Input.GetKey(KeyCode.Q)) {
            rotation = +1f;
        }

        if (Input.GetKey(KeyCode.E)) {
            rotation = -1f;
        }

        return rotation;
#endif
    }

    public float GetCameraZoomAmount() {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputAction.Player.CameraZoom.ReadValue<float>();
#else
        float zoomAmount = 0;
        if (Input.mouseScrollDelta.y > 0) {
            zoomAmount = 1f;
        }

        if (Input.mouseScrollDelta.y < 0) {
            zoomAmount = -1f;
        }

        return zoomAmount;
#endif
    }
}