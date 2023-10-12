using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChatServer1 : NetworkBehaviour
{

    public ChatUi chatUi;
    const ulong SYSTEM_ID = ulong.MaxValue;

    void Start()
    {
        chatUi.printEnteredText = false;
        chatUi.MessageEntered += OnChatUiMessageEntered;

        if (IsServer) {
            NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
            if (IsHost)
            {
                DisplayMessageLocally(SYSTEM_ID, $"You are the host AND client {NetworkManager.LocalClientId}");
            }
            else
            {
                DisplayMessageLocally(SYSTEM_ID, "You are the server");
            }
            } else {
                DisplayMessageLocally(SYSTEM_ID, $"You are a client{NetworkManager.LocalClientId}");
            }
        }



    private void ServerOnClientConnected(ulong clientId)
    {
        ServerSendDirectMessage(
            $"I ({NetworkManager.LocalClientId}) see you ({clientId}) have connected to the server, well done" , 
            NetworkManager.LocalClientId, 
            clientId);
    }


        private void DisplayMessageLocally(ulong from, string message)
        {
            string fromStr = $"Player {from}";
            Color textColor = chatUi.defaultTextColor;

            if (from == NetworkManager.LocalClientId)
            {
                fromStr = "you";
                textColor = Color.magenta;
            } else if (from == SYSTEM_ID)
            {
                fromStr = "SYS";
                textColor = Color.green;
            }





            chatUi.addEntry(fromStr, message);
        }

        private void OnChatUiMessageEntered(string message)
        {
            SendChatMessageServerRpc(message);


        }

        [ServerRpc(RequireOwnership = false)]
        public void SendChatMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
        {
            ReceiveChatMessageClientRpc(message, serverRpcParams.Receive.SenderClientId);
        }


        [ClientRpc]
        public void ReceiveChatMessageClientRpc(string message, ulong from, ClientRpcParams clientRpcParams = default)
        {
            DisplayMessageLocally(from, message);
        }



    private void ServerSendDirectMessage(string message, ulong from, ulong to)
    {
        ulong[] clientIds = new ulong[2]
        {
            from, to
        };
        ClientRpcParams rpcParams = default;
        rpcParams.Send.TargetClientIds = clientIds;

        //clientIds[0] = from;
       // ReceiveChatMessageClientRpc($"<whisper> {message}", from, rpcParams);

       // clientIds[0] = to;
        //ReceiveChatMessageClientRpc($"<whisper> {message}", from, rpcParams);
    }



    }

