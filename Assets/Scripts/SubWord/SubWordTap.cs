using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
public class SubWordTap : NetworkAdditions {

    public UnityAction<Material> OnTap;

    private PhotonView _photonView;
    private bool _canDestroy = false;

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
}
