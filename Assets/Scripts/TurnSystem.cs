using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {
    public static TurnSystem Instance { get; private set; }
    
    public event EventHandler OnTurnChanged;

    private int _turnNumber = 1;
    private bool _isPlayerTurn = true;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one TurnSystem Instance! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void NextTurn() {
        _turnNumber++;
        _isPlayerTurn = !_isPlayerTurn;
        
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber() => _turnNumber;
    public bool IsPlayerTurn() => _isPlayerTurn;
}
