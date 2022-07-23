using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
public class SubWordTap : NetworkAdditions {

    public UnityAction<Material> OnTap;



    // TODO
    // Do proper detection of hands
    private void OnTriggerEnter(Collider other) {
        if(PhotonNetwork.IsMasterClient) {
            Debug.Log($"{gameObject.name} Detected {other.gameObject.name}");
            Material material = GetComponent<Renderer>().material;
            OnTap.Invoke(material);
        }
    }
}
