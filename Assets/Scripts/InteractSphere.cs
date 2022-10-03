using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable {
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;

    private MeshRenderer _meshRenderer;
    private bool _isGreen;
    private bool _isActive;
    private float _timer;
    private Action _onInteractableComplete;

    private void Awake() {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start() {
        LevelGrid levelGrid = LevelGrid.Instance;
        GridPosition gridPosition = levelGrid.GetGridPosition(transform.position);
        levelGrid.SetInteractableAtGridPosition(gridPosition, this);
        
        if (_isGreen) {
            SetGreen();
        }
        else {
            SetRed();
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

    private void SetGreen() {
        _isGreen = true;
        _meshRenderer.material = greenMaterial;
    }

    private void SetRed() {
        _isGreen = false;
        _meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractableComplete) {
        _isActive = true;
        _timer = .5f;
        _onInteractableComplete = onInteractableComplete;

        if (_isGreen) SetRed();
        else SetGreen();
    }
}