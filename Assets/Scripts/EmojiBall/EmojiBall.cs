using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(EmojiGrab))]
[RequireComponent(typeof(EmojiStretch))]

public class EmojiBall : MonoBehaviour {

    private EmojiGrab _emojiGrab;
    private EmojiStretch _emojiStretch;

    private void Awake() {
        _emojiGrab = GetComponent<EmojiGrab>(); 
        _emojiStretch = GetComponent<EmojiStretch>();
        _emojiStretch.enabled = false;
    }

    private void Start() {
        _emojiGrab.OnPlaced.AddListener(OnEmojiPlaced);
    }

    private void OnEmojiPlaced() {
        _emojiGrab.enabled = false;
        _emojiStretch.enabled = true;
    }
}
