using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SubWordTap : MonoBehaviour {

    public UnityAction<Material> OnTap;

    // TODO
    // Do proper detection of hands
    private void OnTriggerEnter(Collider other) {
        Material material = GetComponent<Renderer>().material;
        OnTap.Invoke(material);
    }
}
