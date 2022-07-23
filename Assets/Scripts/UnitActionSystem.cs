using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour {

    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private bool _isBusy;
    private BaseAction _selectedAction;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one UnitActionSystem Instance! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        SetSelectedUnit(selectedUnit);
    }

    void Update() {
        if (_isBusy) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (TryHandleUnitSelection()) return;
        
        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) {
                if (raycastHit.transform.TryGetComponent(out Unit unit)) {
                    if (unit == selectedUnit) return false;
                    
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void HandleSelectedAction() {
        if (Input.GetMouseButton(0)) {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (_selectedAction.IsValidActionGridPosition(mouseGridPosition)) {
                SetBusy();
                _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }
    }

    private void SetSelectedUnit(Unit unit) {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetBusy() => _isBusy = true;
    private void ClearBusy() => _isBusy = false;

    public Unit GetSelectedUnit() => selectedUnit;

    public void SetSelectedAction(BaseAction baseAction) => _selectedAction = baseAction;

    public BaseAction GetSelectedAction() => _selectedAction;
}
