using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAnimationTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    Boss boss => GetComponentInParent<Boss>();
    DeathBringerBoss DeathBringerBoss => GetComponentInParent<DeathBringerBoss>();
    private void AnimationTrigger()
    {
        boss.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(boss.attackCheck.position, boss.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<PlayerMove>() != null)
            {
                PlayerMove target = hit.GetComponent<PlayerMove>();
                target.OnDamaged(30);
            }
        }
    }
    private void SpeicalAttackTrigger()
    {
        boss.AnimationSpecialAttackTrigger();
    }

    private void OpenCounterWindow() => boss.OpenCounterAttackWindow();
    private void CloseCounterWindow() => boss.CloseCounterAttackWindow();
    private void Relocate() => DeathBringerBoss.FindPosition();

    private void MakeInvisible() => DeathBringerBoss.fx.MakeTransprent(true);
    private void MakeVisible() => DeathBringerBoss.fx.MakeTransprent(false);
}
