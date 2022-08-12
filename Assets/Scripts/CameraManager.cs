using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start() {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    private void ShowActionCamera() {
        actionCameraGameObject.gameObject.SetActive(true);
    }

    private void HideActionCamera() {
        actionCameraGameObject.gameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e) {
        switch (sender) {
            case ShootAction shootAction:
                SetUpActionCamera(shootAction.GetUnit(), shootAction.GetTargetUnit());
                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e) {
        switch (sender) {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void SetUpActionCamera(Unit shooterUnit, Unit targetUnit) {
        float shouldersHeight = 1.7f;
        Vector3 cameraCharacterHeight = Vector3.up * shouldersHeight;

        Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

        float shoulderOffset = .5f;
        Vector3 cameraShoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffset;

        Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() +
                                       cameraCharacterHeight +
                                       cameraShoulderOffset;
                                       // (shootDir * -1);

        actionCameraGameObject.transform.position = actionCameraPosition;
        actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
    }
}
