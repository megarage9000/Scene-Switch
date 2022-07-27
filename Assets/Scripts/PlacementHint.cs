using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlacementHint : MonoBehaviour
{
    private Outline outline;
    private MeshRenderer renderer;

    private List<string> _emojiBallTags;

    private void Awake() {
        outline = GetComponent<Outline>();
        renderer = GetComponent<MeshRenderer>();
        _emojiBallTags = new List<string>( new string[] { "AngryEmoji", "DisgustedEmoji", "HappyEmoji", "SadEmoji", "ScaredEmoji", "SurprisedEmoji" });
        HideHint();
    }

    public void ShowHint() {
        SetHint(true);
    }

    public void HideHint() {
        SetHint(false);
    }

    public void ShowPlacement() {
        SetPlacement(true);
    }

    public void HidePlacement() {
        SetPlacement(false);
    }

    private void SetPlacement(bool val) {
        SetPlacementNetwork(val);
        gameObject.GetPhotonView().RPC("SetPlacementNetwork", RpcTarget.Others, val);

    }

    private void SetHint(bool val) {
        SetHintNetwork(val);
        gameObject.GetPhotonView().RPC("SetHintNetwork", RpcTarget.Others, val);
    }

    [PunRPC]
    private void SetPlacementNetwork(bool val) {
        renderer.enabled = val;
    }

    [PunRPC]
    private void SetHintNetwork(bool val) {
        outline.enabled = val;
    }

    private void OnTriggerEnter(Collider other) {
        string otherTag = other.gameObject.tag;
        if(_emojiBallTags.Contains(otherTag)){
            ShowHint();
        }

    }

    private void OnTriggerExit(Collider other) {
        string otherTag = other.gameObject.tag;
        if (_emojiBallTags.Contains(otherTag)) {
            HideHint();
        }
    }
}
