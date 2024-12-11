using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Boss enemy;

    protected override void Start()
    {

        base.Start();

        enemy = GetComponent<Boss>();
    }

    public override void TakeDamage(CharacterStats stats, int _damage)
    {
        base.TakeDamage(stats, _damage);
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();
        Destroy(gameObject, 5f);
    }
}
