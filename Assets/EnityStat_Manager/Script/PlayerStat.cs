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

    private bool setParam = false;

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
    [ServerRpc]
    public void currentHealthServerRpc(float value)
    {
        currentHealth = value;
    }
    [ServerRpc]
    public void currentStaminaServerRpc(float value)
    {
        currentStamina = value;
    }
    public void reduceStamina(float amount)
    {
        currentStaminaServerRpc(currentStamina - amount);
        if(staminaRegen != null)
        {
            StopCoroutine(staminaRegen);
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
        if (staminaRegen != null)
        {
            StopCoroutine(staminaRegen);
            staminaRegen = null;
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
                StopCoroutine(staminaRegen);
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
        onStaminaUpDate.Invoke(currentStamina);
        yield return new WaitForSeconds(2f);
        while(currentStamina < maxStamina)
        {
            currentStaminaServerRpc(currentStamina + maxStamina / 150);
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
            currentStaminaServerRpc(currentStamina - (maxStamina / 100)*amount);
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
        if (IsLocalPlayer)
        {
            Debug.Log("SetParam");
            onStaminaUpDate += upDateStaminaUI;
            onHealthUpDate += upDateHealthUI;
            currentHealthServerRpc(maxHealth);
            currentStaminaServerRpc(maxStamina);
            IsReduceStaminaRunning = false;
            UIstat = GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<UIStatControl>();
            UIstat.SetHealthUI(maxHealth);
            UIstat.SetStaminaUI(maxStamina);
            setParam = true;
        }
    }
    private void Update()
    {
       
    }

}
