using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selected;

    private BaseAction _baseAction;
    
    public void SetBaseAction(BaseAction baseAction) {
        _baseAction = baseAction;
        textMeshPro.text = _baseAction.GetActionName().ToUpper();
        
        button.onClick.AddListener(() => {
            UnitActionSystem.Instance.SetSelectedAction(_baseAction);
        });
        
        UpdateSelectedVisual();
    }

    public void UpdateSelectedVisual() {
        bool isSelected = UnitActionSystem.Instance.GetSelectedAction() == _baseAction;
        selected.SetActive(isSelected);
    }

}
