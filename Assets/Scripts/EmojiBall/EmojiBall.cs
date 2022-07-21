using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(EmojiGrab))]
[RequireComponent(typeof(EmojiStretch))]
[RequireComponent(typeof(EmojiSubwordTap))]
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
    private EmojiSubwordTap _emojiSubwordTap;
    private PhotonView _photonView;

    // UnityEvents to "bubble up" to manager 
    public UnityAction<GameObject> OnPlaced;
    public UnityAction<GameObject> OnSubwordTapped;
    public UnityAction<GameObject> OnScaled;
    public UnityAction<GameObject> OnGrabbed;
    public UnityAction<GameObject> OnReleased;

    private void Awake() {
        _emojiGrab = GetComponent<EmojiGrab>();
        _emojiTap = GetComponent<EmojiTap>();
        _emojiSubwordTap = GetComponent<EmojiSubwordTap>();
        _emojiStretch = GetComponent<EmojiStretch>();
        _photonView = GetComponent<PhotonView>();
        GetComponent<Renderer>().material = emojiMaterial;

        _emojiGrab.OnGrabbed.AddListener(OnEmojiGrabbed);
        _emojiGrab.OnReleased.AddListener(OnEmojiReleased);
        _emojiGrab.OnPlaced.AddListener(OnEmojiPlaced);
        _emojiTap.OnTap.AddListener(OnEmojiTap);
        _emojiSubwordTap.OnSubwordTap.AddListener(OnEmojiSubwordTapped);
        _emojiStretch.OnRescaled.AddListener(OnEmojiScaled);

        OnGrabbed += EmojiGrabbed;
        OnReleased += EmojiReleased;
        OnPlaced += EmojiPlaced;
        OnSubwordTapped += EmojiSubwordTapped;
        OnScaled += EmojiScaled;

        EnableGrab();
    }

    // Dummy events for our emoji UnityAction events
    private void EmojiGrabbed(GameObject gameObject) {
        Debug.Log($"Emoji {gameObject.name} has been grabbed.");
    }
    private void EmojiReleased(GameObject gameObject) {
        Debug.Log($"Emoji {gameObject.name} has been released.");
    }
    private void EmojiPlaced(GameObject gameObject) {
        Debug.Log($"Emoji {gameObject.name} has been placed.");
    }
    private void EmojiSubwordTapped(GameObject gameObject) {
        Debug.Log($"Emoji {gameObject.name} has material changed.");
    }
    private void EmojiScaled(GameObject gameObject) {
        Debug.Log($"Emoji {gameObject.name} has been scaled.");
    }

    // Functions to "bubble up" to the manager
    private void OnEmojiGrabbed() {
        OnGrabbed.Invoke(gameObject);
    }

    private void OnEmojiReleased() {
        OnReleased.Invoke(gameObject);
    }

    private void OnEmojiPlaced() {
        OnPlaced.Invoke(gameObject);
    }

    private void OnEmojiSubwordTapped() {
        OnSubwordTapped.Invoke(gameObject);

        DisableSubwordTap();
        EnableScale();
    }

    private void OnEmojiScaled() {
        OnScaled.Invoke(gameObject);
    }

    // Emoji Tap does not need to be bubbled up to the manager
    private void OnEmojiTap() {
        EnableSubwordTap();
    }

    // Enablers for the components 
    public void EnableGrab() {
        _emojiGrab.enabled = true;
    }

    public void DisableGrab() {
        _emojiGrab.enabled = false;
    }

    public void EnableEmojiTap() {
        _emojiTap.enabled = true;
    }

    public void DisableEmojiTap() {
        _emojiTap.enabled = false;
    }

    public void EnableSubwordTap() {
        _emojiSubwordTap.enabled = true;
        _emojiSubwordTap.SpawnSubWords(subWordMaterials);
    }

    public void DisableSubwordTap() {
        _emojiSubwordTap.RemoveSubWords();
        _emojiSubwordTap.enabled = false;
    }

    public void EnableScale() {
        _emojiStretch.enabled = true;
    }

    public void DisableScale() {
        _emojiStretch.enabled = false;
    }

    
    public void DestroyEmojiBall() {
        
        if(PhotonNetwork.IsMasterClient) {
            _photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} called DestroyThis on master client that has actor number {PhotonNetwork.MasterClient.ActorNumber}");
            DestroyThis();
        }
        else {
            Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} called DestroyThis on non-master client that has actor number {PhotonNetwork.LocalPlayer.ActorNumber}");
            _photonView.RPC("DestroyThis", RpcTarget.MasterClient);
        }
    }
    [PunRPC]
    private void DestroyThis() {

        _photonView.RequestOwnership();
        if (PhotonNetwork.IsMasterClient) {
            if (_photonView.IsMine) {
                Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} is under ownership of master client that has actor number {PhotonNetwork.MasterClient.ActorNumber}");
            }
            if (_photonView.IsOwnerActive) {
                Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} has master client active that has actor number {PhotonNetwork.MasterClient.ActorNumber}");
            }

            Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} has executed PhotonNetwork.Destroy() on master client that has actor number {PhotonNetwork.MasterClient.ActorNumber}");
            PhotonNetwork.Destroy(gameObject.GetPhotonView());
            Destroy(gameObject);
            Debug.Log("Successfuly destroyed object!");
        }
        else {
            Debug.Log("Calling DestroyThis on non-master client");
        }
    }

    public int GetViewID() {
        return _photonView.ViewID;
    }

}
