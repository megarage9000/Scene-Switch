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
        outline.enabled = true;
    }

    public void HideHint() {
        outline.enabled = false;
    }

    public void ShowPlacement() {
        renderer.enabled = true;
    }

    public void HidePlacement() {
        renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
       // Debug.Log($"Placement detected {other.gameObject.name}");
       ShowHint();
    }

    private void OnTriggerExit(Collider other) {
       HideHint(); 
    }
}
