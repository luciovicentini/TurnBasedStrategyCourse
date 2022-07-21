using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour {
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private void Start() {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnOnSelectedUnitChanged;
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnOnSelectedUnitChanged(object sender, EventArgs e) {
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons() {
        foreach (Transform buttons in actionButtonContainerTransform) {
            Destroy(buttons.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActions()) {
            Transform buttonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            buttonTransform.GetComponent<ActionButtonUI>().SetBaseAction(baseAction);
        }
    }
}
