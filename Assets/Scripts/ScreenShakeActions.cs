using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start() {
        ShootAction.OnAnyShooting += ShootAction_OnAnyShooting;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void ShootAction_OnAnyShooting(object sender, ShootAction.OnShootEventArgs e) {
        ScreenShake.Instance.Shake();
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e) {
        ScreenShake.Instance.Shake(5f);
    }
    
    private void SwordAction_OnAnySwordHit(object sender, EventArgs e) {
        ScreenShake.Instance.Shake(2f);
    }
}
