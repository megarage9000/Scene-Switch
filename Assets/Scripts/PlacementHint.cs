using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementHint : MonoBehaviour
{
    private Outline outline;
    private MeshRenderer renderer;
    private void Awake() {
        outline = GetComponent<Outline>();
        renderer = GetComponent<MeshRenderer>();

        HideHint();
    }

    public void ShowHint() {
        Debug.Log("Showing Hint");
        outline.enabled = true;
    }

    public void HideHint() {
        Debug.Log("Hiding Hint");
        outline.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
       // Debug.Log($"Placement detected {other.gameObject.name}");
       ShowHint();
    }

    private void OnTriggerExit(Collider other) {
       HideHint(); 
    }
}
