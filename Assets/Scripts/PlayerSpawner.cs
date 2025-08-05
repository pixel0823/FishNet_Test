using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class PlayerSpawner : NetworkBehaviour
{
    public NetworkObject playerPrefab;

    // public override void OnServerAddPlayer(NetworkConnection conn)
    // {
    //     Vector3 pos = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0);
    //     NetworkObject player = Instantiate(playerPrefab, pos, Quaternion.identity);
    //     base.OnServerAddPlayer(conn, player);
    // }


}
