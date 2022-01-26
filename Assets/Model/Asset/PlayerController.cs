//adapted from example script available at
//https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
using UnityEngine;
using System.Collections;
using Unity.Netcode;
using System;

public class PlayerController : NetworkBehaviour 
{
    public enum PlayerAnimeState
    {
        Idle,
        Walk,
        ReverseWalk
    }

    [SerializeField]
    private float walkspeed = 3.5f;
    
    [SerializeField]
    public float rotationSpeed = 1.5F;

    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<Vector3> networkRotationDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<PlayerAnimeState> networkPlayerState = new NetworkVariable<PlayerAnimeState>();

    private Vector3 oldInputPosition;
    private Vector3 oldInputRotation;

    Animator animator;
    public int aimLayer;
    public int hurtLayer;
    bool playerAim = true;

    private float layerWeightVelocity;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        aimLayer = animator.GetLayerIndex("Aiming");
        hurtLayer = animator.GetLayerIndex("hurt");

    }
    // Update is called once per frame
    void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }

        SomeWait();
    }

    

    private void ClientInput()
    {
        Vector3 inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);

        Vector3 direction = transform.TransformDirection(Vector3.forward);
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 inputPosition = direction * forwardInput;

        if(oldInputPosition != inputPosition || oldInputRotation != inputRotation)
        {
            oldInputRotation = oldInputRotation;
            oldInputPosition = oldInputPosition;
        }
    }
    private void ClientMoveAndRotate()
    {

    }
    private void ClientVisuals()
    {
        
    }
    

    [ServerRpc]
    public void UpdateClientPositionAndRotateServerRpc(Vector3 newPosition, Vector3 newRotation)
    {
        networkPositionDirection.Value = newPosition;
        networkRotationDirection.Value = newRotation;
    }
    private void SomeWait()
    {
        float translation = Input.GetAxis("Vertical") * walkspeed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (translation != 0 || rotation != 0)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }
        if (playerAim == true)
        {
            float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
            animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 0f, ref layerWeightVelocity, 0.2f));
            animator.SetBool("Aim", false);
            walkspeed = 10.0F;
        }
        if (playerAim == false)
        {
            float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
            animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 1f, ref layerWeightVelocity, 0.2f));
            animator.SetBool("Aim", true);
            walkspeed = 2.0F;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerAim = !playerAim;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("GetDamage");
        }
    }
}
