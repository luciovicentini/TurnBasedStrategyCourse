using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] private Animator unitAnimator;
    private Vector3 targetPosition;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            targetPosition = MouseWorld.GetPosition();
        }

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance) {
            unitAnimator.SetBool("isWalking", false);
        }
        else {
            MoveAndRotate();
        }
    }

    private void MoveAndRotate() {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        Move(moveDir);
        Rotate(moveDir);
        unitAnimator.SetBool("isWalking", true);
    }

    private void Move(Vector3 moveDir) {
        float moveSpeed = 4f;
        transform.position += moveDir * (Time.deltaTime * moveSpeed);
    }

    private void Rotate(Vector3 direction) {
        float rotateSpeed = 3f;
        Debug.Log($"t = {Time.deltaTime * rotateSpeed}");
        Debug.Log($"transform.forward = {transform.forward}");
        transform.forward = Vector3.Lerp(transform.forward, direction, (Time.deltaTime * rotateSpeed));
    }

}