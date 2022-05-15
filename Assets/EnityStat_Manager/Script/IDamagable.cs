using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable<T>
{
    float currentHealth { get; }
    void takeDamage(T damageTaken);
}
public interface IStaminaUsable<T>
{
    float currentStamina { get; }
    void reduceStamina(T amount);
    IEnumerator RegenStamina();
    IEnumerator ReduceStaminaOverTime(T amount);
}
