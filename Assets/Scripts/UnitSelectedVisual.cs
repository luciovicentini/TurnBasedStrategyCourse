using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour {
    [SerializeField] private Unit unit;

    private MeshRenderer _meshRenderer;

    private void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        if (unit == UnitActionSystem.Instance.GetSelectedUnit()) {
            ShowUnitSelectedVisual();
        } else {
            HideUnitSelectedVisual();
        }
    }

    private void ShowUnitSelectedVisual() {
        _meshRenderer.enabled = true;
    }

    private void HideUnitSelectedVisual() {
        _meshRenderer.enabled = false;
    }

    private void OnDestroy() {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }
}
