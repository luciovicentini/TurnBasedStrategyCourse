using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private GridObject _gridObject;
    private TextMeshPro _debugText;
    private void Awake() {
        _debugText = GetComponentInChildren<TextMeshPro>();
    }

    public void SetGridObject(GridObject gridObject) {
        this._gridObject = gridObject;
    }

    private void Update() {
        _debugText.SetText(_gridObject.ToString());
    }
}
