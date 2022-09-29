using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour {
    public static event EventHandler OnAnyDestroyed;
    
    private GridPosition _gridPosition;

    private void Start() {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage() {
        Destroy(gameObject);
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition() => _gridPosition;
}
