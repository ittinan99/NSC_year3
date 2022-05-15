using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Cinemachine;

public class PlayerRpgMovement : NetworkBehaviour
{
    //private weapon weaponHeld      Inprogress**
    [SerializeField]
    private CharacterController controller;

    [Range(0f, 10f)]
    public float movementSpeed;
    [Range(100f, 1000f)]
    public float speedMultiplier;

    [Range(0f, 1f)]
    public float turnSmoothTime;
    float turnSmoothVelocity;

    [SerializeField]
    private CinemachineFreeLook Vcam;
    [SerializeField]
    private Transform mainCam;
    public bool canMove;
    private Rigidbody rb;
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (!rb == null) return;
            rb = this.GetComponent<Rigidbody>();

            if (!mainCam == null) return;
            mainCam = Camera.main.transform;
            if (!Vcam == null) return;
            Vcam = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineFreeLook>();

            Vcam.Follow = this.gameObject.transform;
            Vcam.LookAt = this.gameObject.transform;

            if (!canMove) return;
            Movement();
        }
    }
    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
            rb.AddForce(moveDir.normalized * movementSpeed*speedMultiplier * Time.deltaTime);
        }
    }
}
