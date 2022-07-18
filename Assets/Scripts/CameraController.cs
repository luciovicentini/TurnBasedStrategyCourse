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
        Vector3 inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) {
            inputMoveDir.z = +1f;
        }

        if (Input.GetKey(KeyCode.S)) {
            inputMoveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.A)) {
            inputMoveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D)) {
            inputMoveDir.x = +1f;
        }

        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDir.z +
                             transform.right * inputMoveDir.x;
        transform.position += moveVector * (moveSpeed * Time.deltaTime);
    }

    private void HandleRotation() {
        
        Vector3 rotationVector = Vector3.zero;
        if (Input.GetKey(KeyCode.Q)) {
            rotationVector.y = +1f;
        }
        if (Input.GetKey(KeyCode.E)) {
            rotationVector.y = -1f;
        }
        float rotateSpeed = 100f;
        transform.eulerAngles += rotationVector * (rotateSpeed * Time.deltaTime);
    }

    private void HandleZoom() {
        
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0) {
            _targetFollowOffset.y -= zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0) {
            _targetFollowOffset.y += zoomAmount;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        float zoomSpeed = 2f;
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime* zoomSpeed);;
    }
}
