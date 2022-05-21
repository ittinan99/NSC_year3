using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private  Coroutine staminaRegen;
    private  Coroutine staminaReduceOverTime;
    [SerializeField]
    private bool IsReduceStaminaRunning;

    private bool setParam = false;
    [SerializeField]
    private PlayerRpgMovement playerMovement;

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
        if(currentStamina >= 0)
        {
            StopCoroutine(staminaReduceOverTime);
        }
        staminaReduceOverTime = null;
        IsReduceStaminaRunning = false;
        if (staminaRegen != null)
        {
            StopCoroutine(staminaRegen);
        }
        staminaRegen = StartCoroutine(RegenStamina());
        Debug.Log("Stop Run");
    }
  
    public override void receiveAttack(float damage)
    {
        if (!IsLocalPlayer) { return; }
        if (this.GetComponent<PlayerRpgMovement>().isDodging) { Debug.Log("Dodge"); return; }
        currentHealthServerRpc(currentHealth - damage);
        onHealthUpDate.Invoke(currentHealth);
        if (currentHealth <= 0) { playerMovement.playerDie(); return; }
    }
    //[ServerRpc(RequireOwnership = false)]
    //public void receiveAttackServerRpc(float damage)
    //{
    //    currentHealthServerRpc(currentHealth - damage);
    //    if (currentHealth <= 0 ) { playerMovement.playerDie(); return; }
    //    onHealthUpDate.Invoke(currentHealth);
    //}
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
        IsReduceStaminaRunning = false;
    }
    private void upDateHealthUI(float currentHealth)
    {
        UIstat.UpdateHealthUI(currentHealth);
    }
    private void upDateStaminaUI(float currentStamina)
    {
        UIstat.UpdateStaminaUI(currentStamina);
    }
    public void respawnResetHealth()
    {
        UIstat.SetHealthUI(maxHealth);
        currentHealthServerRpc(maxHealth);
    }
    private void HealthChange(float previousValue, float newValue)
    {
        UIstat.UpdateHealthUI(newValue);
    }

    private void StaminaChange(float previousValue, float newValue)
    {
        UIstat.UpdateStaminaUI(newValue);
    }

    void Start()
    {
        if (IsLocalPlayer)
        {
            Debug.Log("isLocal");
            onStaminaUpDate += upDateStaminaUI;
            onHealthUpDate += upDateHealthUI;
            currentHealthServerRpc(maxHealth);
            currentStaminaServerRpc(maxStamina);
            IsReduceStaminaRunning = false;
            UIstat.SetHealthUI(maxHealth);
            UIstat.SetStaminaUI(maxStamina);
            setParam = true;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
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
            UIstat.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.8f);
        }
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        //if (IsLocalPlayer)
        //{
        //    onStaminaUpDate += upDateStaminaUI;
        //    onHealthUpDate += upDateHealthUI;
        //    currentHealthServerRpc(maxHealth);
        //    currentStaminaServerRpc(maxStamina);
        //    IsReduceStaminaRunning = false;
        //    UIstat.SetHealthUI(maxHealth);
        //    UIstat.SetStaminaUI(maxStamina);
        //    setParam = true;
        //    SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        //}
        NetworkcurrentStamina.OnValueChanged += StaminaChange;
        NetworkcurrentHealth.OnValueChanged += HealthChange;
        UIstat.SetHealthUI(maxHealth);
        UIstat.SetStaminaUI(maxStamina);
        if (!IsLocalPlayer)
        {
            GameObject Canvas = GameObject.FindGameObjectWithTag("OtherBar");
            UIstat.transform.SetParent(Canvas.transform);
            UIstat.tag = "OtherPlayerBar";
            UIstat.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.8f);
        }
    }
   


    private void Update()
    {
        
    }

  
}
