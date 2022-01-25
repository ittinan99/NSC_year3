using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Unity.Netcode
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : NetworkBehaviour
    {
        /// <summary>
        /// Move the player charactercontroller based on horizontal and vertical axis input
        /// </summary>

        float yVelocity = 0f;
        [Range(5f, 25f)]
        public float gravity = 15f;
        //the speed of the player movement
        [Range(5f, 15f)]
        public float movementSpeed = 10f;
        //jump speed
        [Range(5f, 15f)]
        public float jumpSpeed = 10f;

        //now the camera so we can move it up and down
        public Transform cameraTransform = null;
        float pitch = 0f;
        [Range(1f, 90f)]
        public float maxPitch = 85f;
        [Range(-1f, -90f)]
        public float minPitch = -85f;
        [Range(0.5f, 5f)]
        public float mouseSensitivity = 2f;
        public GameObject FB;
        //the charachtercompononet for moving us
        //CharacterController cc;

        Rigidbody rigidbody;

        Animator animator;
        int aimLayer;
        int hurtLayer;
        bool playerAim = true;
        private float layerWeightVelocity;

        public GameObject camZoomOut;
        public GameObject camZoomIn;

        private void Start()
        {

            animator = GetComponentInChildren<Animator>();
            aimLayer = animator.GetLayerIndex("Aiming");
            hurtLayer = animator.GetLayerIndex("hurt");

            cameraTransform = GetComponentInChildren<Camera>().transform;
            if (IsLocalPlayer)
            {
                rigidbody = GetComponent<Rigidbody>();
            }
            else
            {
                cameraTransform.gameObject.GetComponent<Camera>().enabled = false;
                cameraTransform.gameObject.GetComponent<AudioListener>().enabled = false;
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (IsLocalPlayer)
            {
                Look();
                Move();
            }
        }
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
       
        void Move()
        {
            //update speed based onn the input
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Physics.Raycast(FB.transform.position, FB.transform.forward

            input = Vector3.ClampMagnitude(input, 1f);

            //transofrm it based off the player transform and scale it by movement speed

            Vector3 move = input * movementSpeed;

            rigidbody.transform.Translate(move * Time.deltaTime);

            //ของอิทเอง5555

            if(input.x != 0 || input.z != 0)
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
                movementSpeed = 10.0F;

                camZoomOut.SetActive(true);
                camZoomIn.SetActive(false);
            }
            if (playerAim == false)
            {
                float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
                animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 1f, ref layerWeightVelocity, 0.2f));
                animator.SetBool("Aim", true);
                movementSpeed = 2.0F;

                camZoomOut.SetActive(false);
                camZoomIn.SetActive(true);
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





            //transform.position += move;
            //is it on the ground
            //if (cc.isGrounded)
            //{
            //    yVelocity = -gravity * Time.deltaTime;
            //    //check for jump here
            //    if (Input.GetButtonDown("Jump"))
            //    {
            //        yVelocity = jumpSpeed;
            //    }
            //}
            ////now add the gravity to the yvelocity
            //yVelocity -= gravity * Time.deltaTime;
            //move.y = yVelocity;
            //and finally move
            //cc.Move(move * Time.deltaTime);
        }
    }
}

