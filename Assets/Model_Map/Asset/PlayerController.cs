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
        ReverseWalk,
        Prethrow,
        Throw,
        Hurt
    }

    [SerializeField]
    private float speed = 3.5f;

    [SerializeField]
    public float rotationSpeed = 0.5f;

    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<Vector3> networkRotationDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<PlayerAnimeState> networkPlayerState = new NetworkVariable<PlayerAnimeState>();

    [SerializeField]
    private NetworkVariable<float> networkAimLayerWeight = new NetworkVariable<float>();

    [SerializeField]
    private NetworkVariable<bool> networkPlayerAim = new NetworkVariable<bool>();

    [SerializeField]
    private NetworkVariable<bool> networkPlayerHurt = new NetworkVariable<bool>();

    [SerializeField]
    private NetworkVariable<bool> networkPlayerThrow = new NetworkVariable<bool>();

    private Vector3 oldInputPosition;
    private Vector3 oldInputRotation;
    private float oldAimLayerWeight;

    Animator animator;

    [SerializeField]
    private int aimLayer;

    [SerializeField]
    private int hurtLayer;

    private bool playerAim = false;
    private bool playerHurt = false;
    private bool playerThrow = false;

    Rigidbody rigidbody;

    private float layerWeightVelocity;

    public GameObject camZoomOut;
    public GameObject camZoomIn;

    public Transform cameraTransform = null;
    float pitch = 0f;
    [Range(1f, 90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;
    [Range(0.5f, 5f)]
    public float mouseSensitivity = 2f;
    public GameObject FB;

    [ServerRpc]
    private void PlayerAimServerRpc(float weight)
    {
        PlayerAimClientRpc(weight);
    }
    [ClientRpc]
    private void PlayerAimClientRpc(float weight)
    {
        networkAimLayerWeight.Value = animator.GetLayerWeight(aimLayer);
        animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(networkAimLayerWeight.Value, weight, ref layerWeightVelocity, 0.2f));

        if(weight != 1)
        { 
            if (IsLocalPlayer)
            {
                speed = 10.0F;
                camZoomOut.SetActive(true);
                camZoomIn.SetActive(false);
            }
        }
        else if(weight == 1)
        {
            if (IsLocalPlayer)
            {
                speed = 2.0F;
                camZoomOut.SetActive(false);
                camZoomIn.SetActive(true);
            }
        }
    }

    //private void PlayerNotHurt()
    //{
    //    animator.SetBool("Hurt", false);
    //}
    //private void PlayerHurt()
    //{
    //    animator.SetBool("Hurt", true);
    //}

    //private void PlayerNotThrow()
    //{
    //    animator.SetBool("Throw", false);
    //}
    //private void PlayerThrow()
    //{
    //    animator.SetBool("Throw", true);
    //}
    void Look()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        //get the mouse inpuit axis values
        float xInput = Input.GetAxis("Mouse X") * mouseSensitivity;
        float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
        //turn the whole object based on the x input
        transform.Rotate(0, xInput, 0);
        //now add on y input to pitch, and clamp it
        pitch -= yInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        //create the local rotation value for the camera and set it
        Quaternion rot = Quaternion.Euler(pitch, 0, 0);
        cameraTransform.localRotation = rot;
    }
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        aimLayer = animator.GetLayerIndex("Aiming");
        hurtLayer = animator.GetLayerIndex("hurt");

        cameraTransform = GetComponentInChildren<Camera>().transform;
        if (IsLocalPlayer)
        {
            camZoomIn.SetActive(false);
            camZoomOut.SetActive(true);
        }
        else
        {
            camZoomIn.SetActive(false);
            camZoomOut.SetActive(false);
            cameraTransform.gameObject.GetComponent<Camera>().enabled = false;
            cameraTransform.gameObject.GetComponent<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if (IsClient && IsOwner)
        {
            
        }
        
        ClientVisuals();

        if (IsLocalPlayer)
        {
            Look();
            ClientInput();
            ClientAim();
            ClientMoveAndRotate();
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerAim = !playerAim;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerThrow = !playerThrow;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerHurt = !playerHurt;
            }
        }

        //SomeWait();
    }

    private void ClientAim()
    {
        
        if (oldAimLayerWeight != animator.GetLayerWeight(aimLayer))
        {
            if (playerAim == true)
            {
                UpdateClientAimLayerWeightServerRpc(0);
            }
            if (playerAim == false)
            {
                UpdateClientAimLayerWeightServerRpc(1);
            }
        }
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

        if (forwardInput > 0)
        {
            UpdatePlayerAnimeStateServerRpc(PlayerAnimeState.Walk);
        }
        else if(forwardInput < 0)
        {
            UpdatePlayerAnimeStateServerRpc(PlayerAnimeState.ReverseWalk);
        }
        else if(forwardInput == 0)
        {
            UpdatePlayerAnimeStateServerRpc(PlayerAnimeState.Idle);
        }

        //if (playerAim == true)
        //{
        //    PlayerNotAim();
        //}
        //else if (playerAim == false)
        //{
        //    PlayerAim();
        //}

        //if (playerHurt == true)
        //{
        //    PlayerHurt();
        //}
        //else if (playerHurt == false)
        //{
        //    PlayerNotHurt();
        //}

        //if (playerThrow == true)
        //{
        //    PlayerThrow();
        //}
        //else if (playerThrow == false)
        //{
        //    PlayerNotThrow();
        //}
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
        if(networkAimLayerWeight.Value != 0)
        {
            PlayerAimServerRpc(0);
        }
        if (networkAimLayerWeight.Value == 0)
        {
            PlayerAimServerRpc(1);
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
        else if(networkPlayerState.Value == PlayerAnimeState.Idle)
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
    public void UpdatePlayerAnimeStateServerRpc(PlayerAnimeState newState)
    {
        networkPlayerState.Value = newState;
    }

    [ServerRpc]
    public void UpdateClientAimLayerWeightServerRpc(float newAimWeight)
    {
        networkAimLayerWeight.Value = newAimWeight;
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
