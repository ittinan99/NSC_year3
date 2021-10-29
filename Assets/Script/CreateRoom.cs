using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using Unity.Netcode;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

namespace Netcode.Transports.PhotonRealtime
{
    public class CreateRoom : MonoBehaviour
    {
        public string roomName = "Test";
        public string nickName = "TestName";
        public string sceneName;
        PhotonRealtimeTransport transport;
        // Happen on server
        public void Host()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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
            transport = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>();
            transport.RoomName = roomName;
            transport.NickName = nickName;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Password1234");
            NetworkManager.Singleton.StartClient();
        }
        Vector3 GetRandomSpawn()
        {
            float x = UnityEngine.Random.Range(-10f, 10f);
            float y = 4f;
            float z = UnityEngine.Random.Range(-10f, 10f);
            return new Vector3(x, y, z);
        }
        public void RoomNameChanged(string newRoomName)
        {
            this.roomName = newRoomName;
        }
    }
}
