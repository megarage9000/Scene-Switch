using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{

    private GameObject spawnedPlayerPrefab;


    public override void OnJoinedRoom() {
        Debug.Log("Player has joined the room!");
        base.OnJoinedRoom();
        Vector3 position = transform.position;
        position.x = position.x + Random.Range(-5, 5);
        position.z = position.z + Random.Range(-5, 5);

        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", position, transform.rotation);
    }

    public override void OnLeftRoom() {
        Debug.Log("Player has left the room!");
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
