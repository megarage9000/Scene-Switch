using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SceneTeleporter : EventCaller
{
    public enum EnvironmentScene {
        Stage,
        Island,
        Castle
    };

    public EnvironmentScene EnvironmentToTeleportTo;

    [PunRPC]
    public void TeleportToNewScene() {
        // Make touching player load faster
        LoadNewScene((int)EnvironmentToTeleportTo, 2f);

    }

    [PunRPC]
    public void DisconnectAllPlayers() {
        if(PhotonNetwork.IsMasterClient) {
            gameObject.GetPhotonView().RPC("Disconnect", RpcTarget.Others);
            Disconnect();
        }
        else {
            gameObject.GetPhotonView().RPC("DisconnectAllPlayers", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    public void Disconnect() {
        PhotonNetwork.Disconnect();
    }

    IEnumerator LoadNewScene(int stage, float delay) {
        
        yield return new WaitForSeconds(delay);

    }


}
