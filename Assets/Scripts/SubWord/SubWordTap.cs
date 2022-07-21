using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
public class SubWordTap : MonoBehaviour {

    public UnityAction<Material> OnTap;

    private PhotonView _photonView;

    private void Awake() {
        _photonView = GetComponent<PhotonView>();
    }

    // TODO
    // Do proper detection of hands
    private void OnTriggerEnter(Collider other) {
        Debug.Log($"{gameObject.name} Detected {other.gameObject.name}");
        Material material = GetComponent<Renderer>().material;
        OnTap.Invoke(material);
    }

    public void DestroySubword() {
        Debug.Log($"Destroying {gameObject.name}");
        if (PhotonNetwork.IsMasterClient) {
            Debug.Log("Destroying on Master");
            DestroyThis();
            
        }
        else {
            _photonView.RPC("DestroyThis", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void DestroyThis() {
        Debug.Log("Calling Destroy This! SubwordTap");
        _photonView.RequestOwnership();
        PhotonNetwork.Destroy(_photonView);
        Destroy(gameObject);
    }
}
