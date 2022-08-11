using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    
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

    private void ShootAction_OnShooting(object sender, ShootAction.OnShootEventArgs e) {
        unitAnimator.SetTrigger("Shoot");

        Transform bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);

        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
        Vector3 targetPosition = e.TargetUnit.GetWorldPosition();
        targetPosition.y = shootPointTransform.position.y;
        
        bulletProjectile.Setup(targetPosition);
    }
}
