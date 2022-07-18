using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] private Animator unitAnimator;
    private Vector3 targetPosition;
    
    private GridPosition _gridPosition;

    private void Awake() {
        targetPosition = transform.position;
    }

    private void Start() {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    private void Update() {
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance) {
            unitAnimator.SetBool("isWalking", false);
        }
        else {
            MoveAndRotate();
        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition) {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
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
        transform.forward = Vector3.Lerp(transform.forward, direction, (Time.deltaTime * rotateSpeed));
    }

    public void SetMoveTarget(Vector3 target) => targetPosition = target;
}