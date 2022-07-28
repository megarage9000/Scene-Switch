using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHandColor : MonoBehaviour
{
    [SerializeField] Material greenMat;
    [SerializeField] Material purpleMat;

    void Update(){
        if(NetworkPlayer.changePlayerHandColor){
            SetPlayerHandColor();
            this.enabled = false;
        }
    }

    public void SetPlayerHandColor(){
        foreach (var mesh in GetComponentsInChildren<Renderer>()) {
            if(PhotonNetwork.IsMasterClient){
                mesh.material = greenMat;
            }
            else{
                mesh.material = purpleMat;
            }
        }
    }
}
