using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SubWordTap : MonoBehaviour {

    public UnityAction<Material> OnTap;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("subword detected someting");
        Material material = GetComponent<Renderer>().material;
        OnTap.Invoke(material);
    }
}
