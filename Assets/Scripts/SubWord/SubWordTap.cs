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
        string tag = other.gameObject.tag;
        if(tag.Equals("GameController")) {
            Material material = GetComponent<Renderer>().material;
            OnTap.Invoke(material);
        }
    }
}
