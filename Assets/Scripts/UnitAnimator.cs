using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {
    [SerializeField] private Animator unitAnimator;

    private void Awake() {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction)) {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction)) {
            shootAction.OnShooting += ShootAction_OnShooting;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e) {
        unitAnimator.SetBool("isWalking", true);
    }
    
    private void MoveAction_OnStopMoving(object sender, EventArgs e) {
        unitAnimator.SetBool("isWalking", false);
    }

    private void ShootAction_OnShooting(object sender, EventArgs e) {
        unitAnimator.SetTrigger("Shoot");
    }
}
