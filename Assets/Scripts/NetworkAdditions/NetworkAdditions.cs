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
        string name = material.name;
        gameObject.GetPhotonView().RPC("SetMaterialByName", RpcTarget.Others, name); 
    }

    [PunRPC]
    public void SetMaterialByName(string name) {
        GetComponent<Renderer>().material = MaterialsTable.GetMaterialFromName(name);
    }

    public void FreezeRigidbody() {
        FreezeRigidBodyNetwork();
        gameObject.GetPhotonView().RPC("FreezeRigidBodyNetwork", RpcTarget.Others);
    }

    [PunRPC]
    private void FreezeRigidBodyNetwork() {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnFreezeRigidbody() {
        UnFreezeRigidBodyNetwork();
        gameObject.GetPhotonView().RPC("UnFreezeRigidBodyNetwork", RpcTarget.Others);
    }

    [PunRPC]
    private void UnFreezeRigidBodyNetwork() {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
