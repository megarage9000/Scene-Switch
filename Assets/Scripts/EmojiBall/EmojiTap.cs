using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmojiTap : MonoBehaviour
{
    public UnityEvent OnTap;
    private bool _canDetectTap;

    private void Start() {
        _canDetectTap = true;
    }

    private void OnEnable() {
        _canDetectTap = true;
    }

    private void OnDisable() {
        _canDetectTap = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(_canDetectTap && other.gameObject.tag.Equals("GameController")) {
            Debug.Log($"{gameObject.name} tap Detected {other.gameObject.name}");
            OnTap.Invoke();
        }
    }
}
