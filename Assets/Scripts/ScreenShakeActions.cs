using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start() {
        ShootAction.OnAnyShooting += ShootAction_OnAnyShooting;
    }

    private void ShootAction_OnAnyShooting(object sender, ShootAction.OnShootEventArgs e) {
        ScreenShake.Instance.Shake();
    }
}
