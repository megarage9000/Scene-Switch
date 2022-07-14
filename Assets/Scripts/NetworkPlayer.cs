using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;
    private PhotonView photonView;

    private void Start() {
        photonView = GetComponent<PhotonView>();

        XROrigin rig = FindObjectOfType<XROrigin>();
        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");
    
        // Disabling mesh renderers if its the local player
        if(photonView.IsMine) {
            foreach (var mesh in GetComponentsInChildren<Renderer>()) {
                mesh.enabled = false;
            }
        }
    }

    void Update(){
        if(photonView.IsMine){
            // We don't want to be moving the head and hands of other players
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);
        }
    }

    void MapPosition(Transform target, Transform rigTransform) {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
