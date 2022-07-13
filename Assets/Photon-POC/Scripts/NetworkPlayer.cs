using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviour
{

    public Transform Head;
    public Transform LeftHand;
    public Transform RightHand;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start() {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update() {
        // Hide your objects, but still move them for the others on Network
        if(photonView.IsMine) {
            RightHand.gameObject.SetActive(false);
            LeftHand.gameObject.SetActive(false);
            Head.gameObject.SetActive(false);

            // We don't want to be moving the head and hands of other players
            MapPosition(Head, XRNode.Head);
            MapPosition(LeftHand, XRNode.LeftHand);
            MapPosition(RightHand, XRNode.RightHand);
        }

    }

    void MapPosition(Transform target, XRNode node) {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.position = position;
        target.rotation = rotation;
    }
}
