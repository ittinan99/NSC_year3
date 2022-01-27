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
                camZoomIn.SetActive(false);
                camZoomOut.SetActive(true);
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
       [ServerRpc]
       public void SetTriggerServerRpc(string triggername)
        {
            SetTriggerClientRpc(triggername);
        }
       [ClientRpc]
        public void SetTriggerClientRpc(string triggername)
        {
            animator.SetTrigger(triggername);
        }
        [ServerRpc]
        public void SetWeightServerRpc(Layer data)
        {
            SetWeightClientRpc(data);
        }
        [ClientRpc]
        public void SetWeightClientRpc(Layer data)
        {
            animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(data.LayerWeight, data.target, ref layerWeightVelocity, 0.2f));
        }
        void Move()
        {
            ////update speed based onn the input
            //Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            ////Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ////Physics.Raycast(FB.transform.position, FB.transform.forward

            //input = Vector3.ClampMagnitude(input, 1f);

            ////transofrm it based off the player transform and scale it by movement speed

            //Vector3 move = input * movementSpeed;

            //rigidbody.transform.Translate(move * Time.deltaTime);

            //ของอิทเอง5555
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            input = Vector3.ClampMagnitude(input, 1f);
            Vector3 move = input * movementSpeed;

            rigidbody.transform.Translate(move * Time.deltaTime);

            Vector3 inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);

            Vector3 direction = transform.TransformDirection(Vector3.forward);
            float forwardInput = Input.GetAxis("Vertical");
            Vector3 inputPosition = direction * forwardInput;
   
            if (forwardInput > 0)
            {
                animator.SetFloat("Walk", 1);
            }
            else if (forwardInput < 0)
            {
                animator.SetFloat("Walk", -1);
            }
            else
            {
                animator.SetFloat("Walk", 0);            
            }

            if (playerAim == true)
            {
                float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
                //animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 0f, ref layerWeightVelocity, 0.2f));
                SetWeightServerRpc(new Layer { LayerWeight = currentAimLayerWeight, target = 0f });
                animator.SetBool("Aim", false);
                movementSpeed = 10.0F;

                camZoomOut.SetActive(true);
                camZoomIn.SetActive(false);
            }
            if (playerAim == false)
            {
                float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
                //animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 1f, ref layerWeightVelocity, 0.2f));
                SetWeightServerRpc(new Layer { LayerWeight = currentAimLayerWeight, target = 1f });
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
                SetTriggerServerRpc("Attack");
                //animator.SetTrigger("Attack");
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SetTriggerServerRpc("GetDamage");
                //animator.SetTrigger("GetDamage");
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

