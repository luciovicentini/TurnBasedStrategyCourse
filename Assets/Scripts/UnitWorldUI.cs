using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start() {
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
        healthSystem.OnDamage += HealthSystem_OnDamage;
        
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e) {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText() {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void HealthSystem_OnDamage(object sender, EventArgs e) {
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
