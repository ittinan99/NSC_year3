using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
public class PlayerStat : NetworkBehaviour,IDamagable<float>,IStaminaUsable<float>
{
    public float maxHealth;
    public float maxStamina;
    [SerializeField]
    NetworkVariable<float> NetworkcurrentHealth = new NetworkVariable<float>();
    [SerializeField]
    NetworkVariable<float> NetworkcurrentStamina = new NetworkVariable<float>();
    public UnityAction<float> onHealthUpDate;
    public UnityAction<float> onStaminaUpDate;
    [SerializeField]
    private UIStatControl UIstat;

    public  Coroutine staminaRegen;

    public  Coroutine staminaReduceOverTime;
    [SerializeField]
    private bool IsReduceStaminaRunning;
    public float currentHealth
    {
        get { return NetworkcurrentHealth.Value; }
        set { NetworkcurrentHealth.Value = value; }
    }
    public float currentStamina
    {
        get { return NetworkcurrentStamina.Value; }
        set { NetworkcurrentStamina.Value = value; }
    }
    public void reduceStamina(float amount)
    {
        currentStamina -= amount;
        if(staminaRegen != null)
        {
            StopCoroutine(RegenStamina());
        }
        staminaRegen = StartCoroutine(RegenStamina());
        onStaminaUpDate.Invoke(currentStamina);
    }
    public void reduceStaminaAmountOverTime(float amount)
    {
        if (!IsReduceStaminaRunning)
        {
            staminaReduceOverTime = StartCoroutine(ReduceStaminaOverTime(amount));
        }
    }
    public void stopReduceStamina()
    {
        if (staminaReduceOverTime != null)
        {
            StopCoroutine(staminaReduceOverTime);
            IsReduceStaminaRunning = false;
            if (staminaRegen != null)
            {
                StopCoroutine(RegenStamina());
            }
            staminaRegen = StartCoroutine(RegenStamina());
        }
    }
    public void takeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
    }
    private void upDateHealthUI(float currentHealth)
    {

    }
    public IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2f);
        while(currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 150;
            onStaminaUpDate.Invoke(currentStamina);
            yield return new WaitForSeconds(0.1f);
        }
        staminaRegen = null;
    }
    public IEnumerator ReduceStaminaOverTime(float amount)
    {
        IsReduceStaminaRunning = true;
        while (currentStamina >= 0)
        {
            currentStamina -= (maxStamina / 100)*amount;
            onStaminaUpDate.Invoke(currentStamina);
            yield return new WaitForSeconds(0.1f);
        }
        staminaReduceOverTime = null;
        IsReduceStaminaRunning = false;
    }
    private void upDateStaminaUI(float currentStamina)
    {
        UIstat.UpdateStaminaUI(currentStamina);
    }
    void Start()
    {
        if (!IsLocalPlayer) { return; }
        onStaminaUpDate += upDateStaminaUI;
        onHealthUpDate += upDateHealthUI;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        IsReduceStaminaRunning = false;
        UIstat = GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<UIStatControl>();
        UIstat.SetHealthUI(currentHealth);
        UIstat.SetStaminaUI(currentStamina);
    }

}
