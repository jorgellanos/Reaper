using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float CurrentHealth = 100;
    protected float CurrentSpeed;
    protected enum EnemyType { small, medium, heavy};
    
    public event Action OnDamageReceived;
    
    public void Chase(Transform target)
    {
        if (CanChase())
        {
            DoChase(target);
        }
    }

    public void Attack(Transform target)
    {
        if (CanAttack(target))
        {
            DoAttack();
        }
    }

    public void ReceiveDamage(float damage, Transform enemyPosition)
    {
        var isDead = ApplyDamage(damage);
        DamageReceived(isDead, enemyPosition);
        NotifyDamageReceived();
    }

    private bool ApplyDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth > 0)
        {
            return false;
        }

        CurrentHealth = 0;
        return true;
    }
    
    private void NotifyDamageReceived()
    {
        OnDamageReceived?.Invoke();
    }

    protected abstract void Patroling();

    protected abstract bool CanChase();

    protected abstract void DoChase(Transform target);

    protected abstract bool CanAttack(Transform target);

    protected abstract void DoAttack();

    protected abstract void DamageReceived(bool isDead, Transform enemyPosition);
}
