using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

[RequireComponent(typeof(EmojiGrab))]
[RequireComponent(typeof(EmojiStretch))]
[RequireComponent(typeof(EmojiTap))]

public class EmojiBall : MonoBehaviour {

    public Material emojiMaterial;
    public List<Material> subWordMaterials;
    /*
     * NOTE
     * 
     * - The Emoji Ball needs to have the SecondGrabPoint child object inactive in the inspector! This avoids
     * XRBaseInteractable issues present between the _emojiGrab and _emojiStreth scripts
     * 
     * - The Emoji Ball also needs the EmojiStretch component disabled on start!
     */
    private EmojiGrab _emojiGrab;
    private EmojiStretch _emojiStretch;
    private EmojiTap _emojiTap;

    // UnityEvents to "bubble up" to manager 
    public UnityAction<GameObject> OnPlaced;
    public UnityAction<GameObject> OnTapped;
    public UnityAction<GameObject> OnScaled;

    private void Awake() {
        _emojiStretch = GetComponent<EmojiStretch>();
        _emojiGrab = GetComponent<EmojiGrab>();
        _emojiTap = GetComponent<EmojiTap>();
        GetComponent<Renderer>().material = emojiMaterial;

        _emojiGrab.OnPlaced.AddListener(OnEmojiPlaced);
        _emojiTap.OnTap.AddListener(OnEmojiTapped);
        _emojiStretch.OnRescaled.AddListener(OnEmojiScaled);

        OnPlaced += EmojiPlaced;
        OnTapped += EmojiTapped;
        OnScaled += EmojiScaled;

        EnableGrab();
    }

    // Dummy events for our emoji UnityAction events
    private void EmojiPlaced(GameObject gameObject) {
        Debug.Log($"Emoji {gameObject.name} has been placed.");
    }
    private void EmojiTapped(GameObject gameObject) {
        Debug.Log($"Emoji {gameObject.name} has material changed.");
    }
    private void EmojiScaled(GameObject gameObject) {
        Debug.Log($"Emoji {gameObject.name} has been scaled.");
    }


    // Functions to "bubble up" to the manager
    private void OnEmojiPlaced() {
        OnPlaced.Invoke(gameObject);
        // Demo purposes
        DisableGrab();
        EnableTap();
    
    }

    private void OnEmojiTapped() {     
        OnTapped.Invoke(gameObject);   
        // Demo purposes
        DisableTap();
        EnableScale();
    }


    private void OnEmojiScaled() {
        OnScaled.Invoke(gameObject);
    }

    // Enablers for the components 
    public void EnableGrab() {
        _emojiGrab.enabled = true;
    }

    public void DisableGrab() {
        _emojiGrab.enabled = false;
    }

    public void EnableTap() {
        _emojiTap.enabled = true;
        _emojiTap.SpawnSubWords(subWordMaterials);
    }

    public void DisableTap() {
        _emojiTap.RemoveSubWords();
        _emojiTap.enabled = false;
    }

    public void EnableScale() {
        _emojiStretch.enabled = true;
    }

    public void DisableScale() {
        _emojiStretch.enabled = false;
    }
}
