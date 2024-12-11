using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    private Rigidbody rb;

    [Header("Major stats")]
    public int strength; // 1 point increase damage by 1 and crit.power by 1%
    public int agility;  // 1 point increase evasion by 1% and crit.chance by 1%
    public int intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public int evasion;
    public int vitality; // 1 point incredase health by 5 points

    [Header("Offensive ints")]
    public int damage;
    public int critChance;
    public int critPower;              // default value 150%

    [Header("Defensive ints")]
    public int maxHealth;
    public int armor;

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower = 150;
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {

    }


    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCorutine(_duration));

    private IEnumerator VulnerableCorutine(float _duartion)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duartion);

        isVulnerable = false;
    }




    public virtual void TakeDamage(CharacterStats stats, int _damage)
    {

        if (isInvincible)
            return;

        if (currentHealth < maxHealth / 2)
        {
            _damage = CheckTargetArmor(stats, _damage);
            DecreaseHealthBy(_damage);
        }
        else
        {
            Debug.Log(_damage);
            DecreaseHealthBy(_damage);
        }

        GetComponent<Entity>().DamageImpact();
        _ = fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }


    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }


    protected virtual void DecreaseHealthBy(int _damage)
    {

        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;


    #region Stat calculations
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor;


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }



    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion + _targetStats.agility;


        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance + agility;

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }


        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower + strength) * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth + vitality * 5;
    }

    #endregion

}