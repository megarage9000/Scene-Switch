using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SubWordTap : MonoBehaviour
{

    public UnityAction<Material> OnTap;
    private Material material;

    private void Awake() {
        material = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("subword detected someting");
        OnTap.Invoke(material);
    }
}
