using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour {
    
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    
    private List<ActionButtonUI> _actionButtonUIList;

    private void Awake() {
        _actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start() {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPointsText();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPointsText();
    }
    
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e) {
        UpdateActionPointsText();
    }

    private void CreateUnitActionButtons() {
        foreach (Transform buttons in actionButtonContainerTransform) {
            Destroy(buttons.gameObject);
        }
        
        _actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActions()) {
            Transform buttonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = buttonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            _actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UpdateSelectedVisual() {
        foreach (ActionButtonUI actionButtonUI in _actionButtonUIList) {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPointsText() {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointsText.text = $"Action Points: {selectedUnit.GetActionPoints()}";
    }
}
