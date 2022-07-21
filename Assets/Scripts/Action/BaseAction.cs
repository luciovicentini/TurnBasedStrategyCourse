using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {
    protected Unit Unit;
    protected bool IsActive;
    protected Action OnActionComplete;

    protected virtual void Awake() {
        Unit = GetComponent<Unit>();
    }
}
