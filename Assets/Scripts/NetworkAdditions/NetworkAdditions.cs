using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class NetworkAdditions : MonoBehaviour {

    bool _canDestroy = false;
    
    public void DestroyNetworkObject() {
        if (PhotonNetwork.IsMasterClient) {
            DestroyThis();
        }
        else {
            gameObject.GetPhotonView().RPC("DestroyThis", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void DestroyThis() {
        PhotonView photonView = gameObject.GetPhotonView();
        photonView.RequestOwnership();
        if(photonView.IsMine) {
            PhotonNetwork.Destroy(photonView);
        }
        else {
            photonView.TransferOwnership(PhotonNetwork.MasterClient);
            _canDestroy = true;
            StartCoroutine(DestroyClientObjectOnMaster());
        }

    }

    IEnumerator DestroyClientObjectOnMaster() {
        PhotonView photonView = gameObject.GetPhotonView();
        while (!PhotonNetwork.IsMasterClient || !photonView.IsMine || !_canDestroy) {
            yield return null;
        }
        PhotonNetwork.Destroy(photonView);
    }

    public void SetMaterial(Material material) {
        PhotonView photonView = gameObject.GetPhotonView();
        GetComponent<Renderer>().material = material;
        int id = MaterialsTable.GetIdFromMaterial(material);
        Debug.Log("Calling SetMaterialByName");
        gameObject.GetPhotonView().RPC("SetMaterialByName", RpcTarget.All, id); 
    }

    [PunRPC]
    public void SetMaterialByName(int id) {
        GetComponent<Renderer>().material = MaterialsTable.GetMaterialFromId(id);
    }
}
