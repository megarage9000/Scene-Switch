using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class EventCaller : NetworkAdditions
{

    public UnityEvent OnContact;

    private void OnTriggerEnter(Collider other) {
        GetComponent<Renderer>().material.color = Color.green;
        OnContact.Invoke();
        DestroyNetworkObject();
    }

    private void OnTriggerExit(Collider other) {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
