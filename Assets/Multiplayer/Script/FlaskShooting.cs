using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Netcode
{
    public class FlaskShooting : NetworkBehaviour
    {
        public TrailRenderer T_Renderer;
        public TrailRenderer Scanner;
        public GameObject FlaskBarrel;

        [SerializeReference]
        private AmmoPanel AP = null;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(AP == null)
            {
                AP = GameObject.FindObjectOfType<AmmoPanel>();
            }
            if (IsLocalPlayer && GameSystem.gamePhase == GameSystem.GamePhase.AttackState)
            {
                if (Input.GetButtonDown("Fire1") && AP.CurrentAmmo.AmmoAmount > 0)
                {
                    AP.CurrentAmmo.AmmoAmount--;
                    AP.DisplayAmmo.SetVar();
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
            if (AP.CurrentAmmo.E_Card.Scanable)
            {
                var flask = Instantiate(Scanner, FlaskBarrel.transform.position, Quaternion.identity);
                flask.GetComponent<Rigidbody>().AddForce(transform.forward * 50, ForceMode.Impulse);
            }
            else
            {
                var flask = Instantiate(T_Renderer, FlaskBarrel.transform.position, Quaternion.identity);
                flask.GetComponent<Rigidbody>().AddForce(transform.forward * 50, ForceMode.Impulse);
            }
        }
    }
}

