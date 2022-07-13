using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviour
{

    public Transform Head;
    public Transform LeftHand;
    public Transform RightHand;

    private Transform HeadRig;
    private Transform LeftHandRig;
    private Transform RightHandRig;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start() {
        photonView = GetComponent<PhotonView>();

        XROrigin rig = FindObjectOfType<XROrigin>();
        HeadRig = rig.transform.Find("Camera Offset/Main Camera");
        LeftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        RightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");
    
        // Disabling mesh renderers if its the local player
        if(photonView.IsMine) {
            foreach (var mesh in GetComponentsInChildren<Renderer>()) {
                mesh.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if(photonView.IsMine) {
            // We don't want to be moving the head and hands of other players
            MapPosition(Head, HeadRig);
            MapPosition(LeftHand, LeftHandRig);
            MapPosition(RightHand, RightHandRig);
        }

    }

    void MapPosition(Transform target, Transform rigTransform) {

        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
