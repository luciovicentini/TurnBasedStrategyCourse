using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour {
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;
    
    private Vector3 _targetPosition;
    private Unit _unit;
    private void Awake() {
        _targetPosition = transform.position;
        _unit = GetComponent<Unit>();
    }

    private void Update() {
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, _targetPosition) < stoppingDistance) {
            unitAnimator.SetBool("isWalking", false);
        }
        else {
            unitAnimator.SetBool("isWalking", true);
            MoveAndRotate();
        }
    }

    private void MoveAndRotate() {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        Move(moveDir);
        Rotate(moveDir);
    }

    private void Move(Vector3 moveDir) {
        float moveSpeed = 4f;
        transform.position += moveDir * (Time.deltaTime * moveSpeed);
    }

    private void Rotate(Vector3 direction) {
        float rotateSpeed = 3f;
        transform.forward = Vector3.Lerp(transform.forward, direction, (Time.deltaTime * rotateSpeed));
    }
    
    public void MoveTo(GridPosition target) => _targetPosition = LevelGrid.Instance.GetWorldPosition(target);
    
    private List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        
        for (int x = -maxMoveDistance; x < maxMoveDistance; x++) {
            for (int z = -maxMoveDistance; z < maxMoveDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                if (unitGridPosition == testGridPosition) continue;
                
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition) =>
        GetValidActionGridPositionList().Contains(gridPosition);


}
