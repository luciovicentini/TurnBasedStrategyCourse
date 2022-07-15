using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    private GridSystem _gridSystem;
    
    void Start() {
        _gridSystem = new GridSystem(10, 10, 2f);
    }
    
    void Update()
    {
        Debug.Log(_gridSystem.GetPosition(MouseWorld.GetPosition()));    
    }
}
