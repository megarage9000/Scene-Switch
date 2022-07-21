using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class EventCaller : MonoBehaviour
{

    public UnityEvent OnContact;

    private PhotonView _photonView;
    private bool _canDestroy = false;

    private void Awake() {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other) {
        GetComponent<Renderer>().material.color = Color.green;
        

        if(PhotonNetwork.IsMasterClient) {
            Debug.Log("Destroying on master client side");
            CallEvent();
        }
        else {
            Debug.Log("Destroying on client side");
            _photonView.RPC("CallEvent", RpcTarget.MasterClient);
        }
    }

    private void OnTriggerExit(Collider other) {
        GetComponent<Renderer>().material.color = Color.white;
    }


    [PunRPC]
    private void CallEvent() {
        OnContact.Invoke();
        _photonView.RequestOwnership();
        if(PhotonNetwork.IsMasterClient) {
            if(_photonView.IsMine) {
                PhotonNetwork.Destroy(gameObject.GetPhotonView());
            }
            else {
                gameObject.GetPhotonView().TransferOwnership(PhotonNetwork.MasterClient);
                _canDestroy = true;
            }
        }
    }

    private void Update() {
        if(_photonView.IsMine && _canDestroy && PhotonNetwork.IsMasterClient) {
            PhotonNetwork.Destroy(gameObject.GetPhotonView());
        }
    }

}
