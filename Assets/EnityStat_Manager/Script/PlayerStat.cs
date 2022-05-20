using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class PlayerStat : AttackTarget,IDamagable<float>,IStaminaUsable<float>
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
    public UIStatControl UIstat;

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
    [ServerRpc(RequireOwnership = false)]
    public void currentHealthServerRpc(float value)
    {
        currentHealth = value;
    }
    [ServerRpc(RequireOwnership = false)]
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
  
    public override void receiveAttack(float damage)
    {
        receiveAttackServerRpc(damage);
    }
    [ServerRpc]
    public void receiveAttackServerRpc(float damage)
    {
        currentHealth -= damage;
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
            //UIstat = GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<UIStatControl>();
            UIstat.SetHealthUI(maxHealth);
            UIstat.SetStaminaUI(maxStamina);
            setParam = true;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        if (!IsLocalPlayer)
        {
            NetworkcurrentStamina.OnValueChanged += StaminaChange;
            NetworkcurrentHealth.OnValueChanged += HealthChange;
            UIstat.SetHealthUI(maxHealth);
            UIstat.SetStaminaUI(maxStamina);
            GameObject Canvas = GameObject.FindGameObjectWithTag("OtherBar");
            UIstat.transform.SetParent(Canvas.transform);
            UIstat.tag = "OtherPlayerBar";
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (IsLocalPlayer)
        {
            onStaminaUpDate += upDateStaminaUI;
            onHealthUpDate += upDateHealthUI;
            currentHealthServerRpc(maxHealth);
            currentStaminaServerRpc(maxStamina);
            IsReduceStaminaRunning = false;
            //UIstat = GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<UIStatControl>();
            UIstat.SetHealthUI(maxHealth);
            UIstat.SetStaminaUI(maxStamina);
            setParam = true;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        if (!IsLocalPlayer)
        {
            NetworkcurrentStamina.OnValueChanged += StaminaChange;
            NetworkcurrentHealth.OnValueChanged += HealthChange;
            UIstat.SetHealthUI(maxHealth);
            UIstat.SetStaminaUI(maxStamina);
            GameObject Canvas = GameObject.FindGameObjectWithTag("OtherBar");
            UIstat.transform.SetParent(Canvas.transform);
            UIstat.tag = "OtherPlayerBar";
        }
    }

    private void HealthChange(float previousValue, float newValue)
    {
        UIstat.UpdateHealthUI(newValue);
    }

    private void StaminaChange(float previousValue, float newValue)
    {
        UIstat.UpdateStaminaUI(newValue);
    }



    private void Update()
    {
        
    }

  
}
