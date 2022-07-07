using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    private Vector3 targetPosition;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Move(MouseWorld.GetPosition());
        }
        
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance) return;
        
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 4f;
        transform.position += moveDir * (Time.deltaTime * moveSpeed);
    }

    private void Move(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }
}