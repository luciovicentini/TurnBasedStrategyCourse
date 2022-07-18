using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Update() {
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
}