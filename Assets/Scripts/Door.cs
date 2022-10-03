using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {
    [SerializeField] private bool isOpen;

    private GridPosition _gridPosition;
    private Animator _animator;
    private bool _isActive;
    private float _timer;
    private Action _onInteractableComplete;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        LevelGrid levelGrid = LevelGrid.Instance;
        _gridPosition = levelGrid.GetGridPosition(transform.position);
        levelGrid.SetInteractableAtGridPosition(_gridPosition, this);
        Pathfinding.Instance.SetIsWalkable(_gridPosition, isOpen);

        if (isOpen) {
            OpenDoor();
        }
    }

    private void Update() {
        if (!_isActive) return;
        _timer -= Time.deltaTime;

        if (_timer <= 0f) {
            _isActive = false;
            _onInteractableComplete();
        }
    }

    private void OpenDoor() {
        isOpen = true;
        _animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkable(_gridPosition, isOpen);
    }

    private void CloseDoor() {
        isOpen = false;
        _animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkable(_gridPosition, isOpen);
    }

    public void Interact(Action onInteractableComplete) {
        _onInteractableComplete = onInteractableComplete;
        _isActive = true;
        _timer = .5f;

        if (!isOpen) {
            OpenDoor();
        }
        else {
            CloseDoor();
        }
    }
}