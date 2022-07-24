using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using UnityEngine.Events;

public class EmojiGrab : XRGrabInteractable
{
    private IXRSelectInteractor _interactor;
    private XRInteractionManager _interactionManager;
    private GameObject _placementLocation;
    private PhotonView _photonView;

    public UnityEvent OnPlaced;
    public UnityEvent OnGrabbed;
    public UnityEvent OnReleased;

    protected override void Awake() {
        base.Awake();
        _interactionManager = FindObjectOfType<XRInteractionManager>();
        _photonView = GetComponent<PhotonView>();
        OnPlaced = new UnityEvent();
        OnGrabbed = new UnityEvent();
        OnReleased = new UnityEvent();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} requested ownership by master client that has actor number {PhotonNetwork.MasterClient.ActorNumber}");
        _photonView.RequestOwnership();
        base.OnSelectEntered(args);
        AddGrabEvent(args.interactorObject.transform.gameObject);
        _interactor = args.interactorObject;

        if(PhotonNetwork.IsMasterClient) {
            Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} called grab on master client that has actor number {PhotonNetwork.MasterClient.ActorNumber}");
            OnGrabbedNetwork();
        }
        else {
            Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} called grab on non-master client that has actor number {PhotonNetwork.LocalPlayer.ActorNumber}");
            _photonView.RPC("OnGrabbedNetwork", RpcTarget.MasterClient);
        }
        UnFreezeRigidbody();
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        RemoveGrabEvent(args.interactorObject.transform.gameObject);
        _interactor = null;
   
        if (PhotonNetwork.IsMasterClient) {
            Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} called release on master client that has actor number {PhotonNetwork.MasterClient.ActorNumber}");
            OnReleasedNetwork();
        }
        else {
            Debug.Log($"{gameObject.name} with {_photonView.ViewID} and owner actor number {_photonView.Owner.ActorNumber} called grab on non-master client that has actor number {PhotonNetwork.LocalPlayer.ActorNumber}"); ;
            _photonView.RPC("OnReleasedNetwork", RpcTarget.MasterClient);
        }
        FreezeRigidbody();
    }

    private void AddGrabEvent(GameObject controller) {
        ControllerAdditions controllerEvents = controller.GetComponent<ControllerAdditions>();
        if (controllerEvents) {
            controllerEvents.PrimaryPress.AddListener(PlaceEmojiBall);
            controllerEvents.SecondaryPress.AddListener(PlaceEmojiBall);
        }
    }

    private void RemoveGrabEvent(GameObject controller) {
        ControllerAdditions controllerEvents = controller.GetComponent<ControllerAdditions>();
        if (controllerEvents) {
            controllerEvents.PrimaryPress.RemoveListener(PlaceEmojiBall);
            controllerEvents.SecondaryPress.RemoveListener(PlaceEmojiBall);
        }
    }

    private void OnTriggerEnter(Collider collision) {
        GameObject detected = collision.gameObject;
        PlacementHint hint = collision.gameObject.GetComponent<PlacementHint>();

        if (detected && hint) {
            if (_placementLocation) {
                _placementLocation.GetComponent<PlacementHint>().HideHint();

            }
            _placementLocation = detected;
            hint.ShowHint();
        }

    }

    private void OnTriggerExit(Collider collision) {
        GameObject leaving = collision.gameObject;
        PlacementHint hint = collision.gameObject.GetComponent<PlacementHint>();

        if (leaving && hint) {
            _placementLocation = null;
            hint.HideHint();
        }
    }

    private void PlaceEmojiBall() {
        if (_placementLocation) {
            _interactionManager.CancelInteractorSelection(_interactor);

            transform.position = _placementLocation.transform.position;
            transform.rotation = _placementLocation.transform.rotation;

            // Destroy(_placementLocation);
            if (PhotonNetwork.IsMasterClient) {
                Debug.Log($"{gameObject.name} with {_photonView.ViewID} called release on master client");
                OnPlacedNetwork();
            }
            else {
                Debug.Log($"{gameObject.name} with {_photonView.ViewID} called release on non-master client");
                _photonView.RPC("OnPlacedNetwork", RpcTarget.MasterClient);
            }
            FreezeRigidbody();
        }
    }

    // Networking RPC calls 
    [PunRPC]
    private void OnPlacedNetwork() {
        Debug.Log($"{gameObject.name} with {_photonView.ViewID} has been placed");
        OnPlaced.Invoke();
        _placementLocation = null;
    }

    [PunRPC]
    private void OnGrabbedNetwork() {
        Debug.Log($"{gameObject.name} with {_photonView.ViewID} has been grabbed");
        OnGrabbed.Invoke();
    }

    [PunRPC]
    private void OnReleasedNetwork() {
        Debug.Log($"{gameObject.name} with {_photonView.ViewID} has been released");
        OnReleased.Invoke();
    }

    public void FreezeRigidbody() {
        FreezeRigidBodyNetwork();
        gameObject.GetPhotonView().RPC("FreezeRigidBodyNetwork", RpcTarget.Others);
    }

    [PunRPC]
    private void FreezeRigidBodyNetwork() {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnFreezeRigidbody() {
        UnFreezeRigidBodyNetwork();
        gameObject.GetPhotonView().RPC("UnFreezeRigidBodyNetwork", RpcTarget.Others);
    }

    [PunRPC]
    private void UnFreezeRigidBodyNetwork() {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
