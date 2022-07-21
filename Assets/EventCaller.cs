using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventCaller : MonoBehaviour
{

    public UnityEvent OnContact;

    private void OnTriggerEnter(Collider other) {
        GetComponent<Renderer>().material.color = Color.green;
        OnContact.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
