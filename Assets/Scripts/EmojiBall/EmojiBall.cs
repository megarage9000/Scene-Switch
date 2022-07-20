using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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


    private void Awake() {
        _emojiStretch = GetComponent<EmojiStretch>();
        _emojiGrab = GetComponent<EmojiGrab>();
        _emojiTap = GetComponent<EmojiTap>();
        GetComponent<Renderer>().material = emojiMaterial;
    }

    private void Start() {
        _emojiGrab.OnPlaced.AddListener(OnEmojiPlaced);
    }

    private void OnEmojiPlaced() {
        _emojiGrab.enabled = false;
        _emojiTap.enabled = true;
    }

    private void SpawnSubWords() {
        _emojiTap.SpawnSubWords(subWordMaterials);
    }
}
