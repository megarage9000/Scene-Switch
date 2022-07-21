using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class EventCaller : MonoBehaviour
{

    public UnityEvent OnContact;

    private PhotonView _photonView;
    private void Awake() {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other) {
        _photonView.RequestOwnership();
        GetComponent<Renderer>().material.color = Color.green;
        OnContact.Invoke();
        Destroy(gameObject, 3f);
    }

    private void OnTriggerExit(Collider other) {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
