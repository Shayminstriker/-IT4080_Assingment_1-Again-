using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Arena1Game : NetworkBehaviour
{

    public Player playerPrefab;
    public Player hostPrefab;
    public Camera arenaCamera;

    private int positionIndex = 0;
    private Vector3[] startPositions = new Vector3[]
    {
        new Vector3(4, 2, 0),
        new Vector3(-4, 2, 0),
        new Vector3(0, 2, 4),
        new Vector3(0, 2, -4)

    };

    private int colorIndex = 0;
    private Color[] playerColors = new Color[] {
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta,
    };



    // Start is called before the first frame update
    void Start()
    {

        arenaCamera.enabled = !IsClient;
        arenaCamera.GetComponent<AudioListener>().enabled = !IsClient;
        if (IsServer)
        {
            SpawnPlayers();
        }
    }

    private Vector3 NextPosition()
    {
        Vector3 pos = startPositions[positionIndex];
        positionIndex += 1;
        if (positionIndex > startPositions.Length - 1)
        {
            positionIndex = 0;
        }
        return pos;
    }


    private Color NextColor()
    {
        Color newColor = playerColors[colorIndex];
        colorIndex += 1;
        if (colorIndex > playerColors.Length - 1)
        {
            colorIndex = 0;
        }
        return newColor;
    }


    private void SpawnPlayers()
    {

        foreach (ulong clientId in NetworkManager.ConnectedClientsIds)
        {
            if (clientId == NetworkManager.LocalClientId)
            {

                Player playerSpawn = Instantiate(hostPrefab, NextPosition(), Quaternion.identity);
                playerSpawn.playerColorNetVar.Value = NextColor();
                playerSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            }
            else
            {
                Player playerSpawn = Instantiate(playerPrefab, NextPosition(), Quaternion.identity);
                playerSpawn.playerColorNetVar.Value = NextColor();
                playerSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

            }
        }
    }
}
