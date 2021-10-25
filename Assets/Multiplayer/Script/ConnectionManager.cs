using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Netcode.Transports.UNET
{
    public class ConnectionManager : MonoBehaviour
    {
        [SerializeField] GameObject ConnectionButtonPanel;
        public string ipAddress = "127.0.0.1";
        UNetTransport transport;
        public Camera lobbyCamera;
        // Happen on server
        public void Host()
        {
            ConnectionButtonPanel.SetActive(false);
            lobbyCamera.gameObject.SetActive(false);
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost();
        }
        // Happen on server
        private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
        {
            //Check incoming data
            bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "Password1234";
            callback(true, null, approve, GetRandomSpawn(), Quaternion.identity);
        }
        public void Join()
        {
            transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
            transport.ConnectAddress = ipAddress;
            ConnectionButtonPanel.SetActive(false);
            lobbyCamera.gameObject.SetActive(false);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Password1234");
            NetworkManager.Singleton.StartClient();
        }
        Vector3 GetRandomSpawn()
        {
            float x = UnityEngine.Random.Range(-10f, 10f);
            float y = 2f;
            float z = UnityEngine.Random.Range(-10f, 10f);
            return new Vector3(x, y, z);
        }
        public void IPAddressChanged(string newAddress)
        {
            this.ipAddress = newAddress;
        }
    }
}

