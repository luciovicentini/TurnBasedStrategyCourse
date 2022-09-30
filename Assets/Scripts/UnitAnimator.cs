using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;
    
    private void Awake() {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction)) {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction)) {
            shootAction.OnShooting += ShootAction_OnShooting;
        }

        if (TryGetComponent(out SwordAction swordAction)) {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e) {
        EquipSword();
        unitAnimator.SetTrigger("SwordSlash");    
    }
    
    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e) {
        EquipRifle();
    }

    private void Start() {
        EquipRifle();
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

    private void EquipRifle() {
        rifleTransform.gameObject.SetActive(true);
        swordTransform.gameObject.SetActive(false);
    }

    private void EquipSword() {
        rifleTransform.gameObject.SetActive(false);
        swordTransform.gameObject.SetActive(true);
    }
}
