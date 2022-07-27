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
    [SerializeField] Animator leftHandAnimator;
    [SerializeField] Animator rightHandAnimator;
    [SerializeField] Material purpleMat;
    [SerializeField] Material greenMat;

    static public bool changePlayerHandColor = false;

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

        foreach (var mesh in GetComponentsInChildren<Renderer>()) {
            if(PhotonNetwork.IsMasterClient){
                mesh.material = (photonView.IsMine) ? greenMat : purpleMat;
            }
            else{
                mesh.material = (photonView.IsMine) ? purpleMat : greenMat;
            }
        }

        
        // foreach (var mesh in GetComponentsInChildren<Renderer>()) {
        //     mesh.material = (PhotonNetwork.LocalPlayer.ActorNumber == 1) ? purpleMat : greenMat;
        // }

        // Disabling mesh renderers if its the local player
        if(photonView.IsMine) {
            changePlayerHandColor = true;
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

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }
    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator){
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)){
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else{
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue)){
            handAnimator.SetFloat("Grip", gripValue);
        }
        else{
            handAnimator.SetFloat("Grip", 0);
        }
    }

    void MapPosition(Transform target, Transform rigTransform) {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
