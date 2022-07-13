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
        } else {
            Move();
        }
    }

    private void Move() {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 4f;
        transform.position += moveDir * (Time.deltaTime * moveSpeed);
        
        unitAnimator.SetBool("isWalking", true);
    }
}