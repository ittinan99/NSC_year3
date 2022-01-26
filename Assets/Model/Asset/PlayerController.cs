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
    private float speed = 3.5f;
    
    [SerializeField]
    public float rotationSpeed = 1f;

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
    Rigidbody rigidbody;

    private float layerWeightVelocity;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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

        ClientMoveAndRotate();
        ClientVisuals();
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
            oldInputRotation = inputRotation;
            oldInputPosition = inputPosition;
            UpdateClientPositionAndRotateServerRpc(inputPosition, inputRotation);
        }

        if(forwardInput > 0)
        {
            UpdatePlayerAnimaStateServerRpc(PlayerAnimeState.Walk);
        }
        else if(forwardInput < 0)
        {
            UpdatePlayerAnimaStateServerRpc(PlayerAnimeState.ReverseWalk);
        }
        else
        {
            UpdatePlayerAnimaStateServerRpc(PlayerAnimeState.Idle);
        }
    }
    private void ClientMoveAndRotate()
    {
        if(networkPositionDirection.Value != Vector3.zero)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            input = Vector3.ClampMagnitude(input, 1f);
            Vector3 move = input * speed;

            rigidbody.transform.Translate(move * Time.deltaTime);
        }
        if(networkRotationDirection.Value != Vector3.zero)
        {
            transform.Rotate(networkRotationDirection.Value);
        }
    }
    private void ClientVisuals()
    {
        if(networkPlayerState.Value == PlayerAnimeState.Walk)
        {
            animator.SetFloat("Walk", 1);
        }
        else if(networkPlayerState.Value == PlayerAnimeState.ReverseWalk)
        {
            animator.SetFloat("Walk", -1);
        }
        else
        {
            animator.SetFloat("Walk", 0);
        }
    }
    

    [ServerRpc]
    public void UpdateClientPositionAndRotateServerRpc(Vector3 newPosition, Vector3 newRotation)
    {
        networkPositionDirection.Value = newPosition;
        networkRotationDirection.Value = newRotation;
    }

    [ServerRpc]
    public void UpdatePlayerAnimaStateServerRpc(PlayerAnimeState newState)
    {
        networkPlayerState.Value = newState;
    }
    private void SomeWait()
    {
        if (playerAim == true)
        {
            float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
            animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 0f, ref layerWeightVelocity, 0.2f));
            animator.SetBool("Aim", false);
            speed = 10.0F;
        }
        if (playerAim == false)
        {
            float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
            animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 1f, ref layerWeightVelocity, 0.2f));
            animator.SetBool("Aim", true);
            speed = 2.0F;
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
