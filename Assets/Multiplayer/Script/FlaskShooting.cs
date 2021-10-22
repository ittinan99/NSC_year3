using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Netcode
{
    public class FlaskShooting : NetworkBehaviour
    {
        public TrailRenderer T_Renderer;
        public GameObject FlaskBarrel;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (IsLocalPlayer)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    ThrowServerRpc();
                }
            }
        }
        [ServerRpc]
        void ThrowServerRpc()
        {
            ThrowClientRpc();
        }
        [ClientRpc]
        void ThrowClientRpc()
        {
            var flask = Instantiate(T_Renderer, FlaskBarrel.transform.position, Quaternion.identity);
            flask.GetComponent<Rigidbody>().AddForce(transform.forward * 50, ForceMode.Impulse);
        }
    }
}

