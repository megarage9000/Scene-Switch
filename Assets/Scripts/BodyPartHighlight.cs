using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BodyPartHighlight : MonoBehaviour
{

    private Outline _outline;

    void Start()
    {
        _outline = GetComponent<Outline>();
        if(PhotonNetwork.IsMasterClient) {
            StartCoroutine(HighlightSequence());
        }
    }

    [PunRPC]
    public void HighlightBodyPart(bool canHighlight) {

        if(PhotonNetwork.IsMasterClient) {
            HighlightBodyPartNetwork(canHighlight);
            gameObject.GetPhotonView().RPC("HighlightBodyPartNetwork", RpcTarget.Others, canHighlight);
        }
        else {
            gameObject.GetPhotonView().RPC("HighlightBodyPart", RpcTarget.MasterClient, canHighlight);
        }    
    }

    [PunRPC]
    private void HighlightBodyPartNetwork(bool canHighlight) {
        _outline.enabled = canHighlight;
    }

    // Demo purposes only
    IEnumerator HighlightSequence() {
        bool _canHighlight = false;
        while(true) {
            HighlightBodyPart(_canHighlight);
            _canHighlight = !_canHighlight;
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
