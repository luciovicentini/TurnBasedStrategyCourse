using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private Vector3 _targetFollowOffset;
    private CinemachineTransposer _cinemachineTransposer;

    private void Start() {
        _cinemachineTransposer =
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update() {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement() {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveDirection();

        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDir.y +
                             transform.right * inputMoveDir.x;
        transform.position += moveVector * (moveSpeed * Time.deltaTime);
    }

    private void HandleRotation() {
        Vector3 rotationVector = Vector3.zero;
        rotationVector.y = InputManager.Instance.GetCameraRotationAmount();

        float rotateSpeed = 100f;
        transform.eulerAngles += rotationVector * (rotateSpeed * Time.deltaTime);
    }

    private void HandleZoom() {
        float zoomMultiplier = 1f;
        _targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomMultiplier;

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        float zoomSpeed = 2f;
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset,
            Time.deltaTime * zoomSpeed);
        ;
    }
}