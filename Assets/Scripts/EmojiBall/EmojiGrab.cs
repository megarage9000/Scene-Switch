using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class EmojiGrab : XRGrabInteractable
{
    private IXRSelectInteractor _interactor;
    private XRInteractionManager _interactionManager;
    private GameObject _placementLocation;
    private Rigidbody _rigidBody;

    public UnityEvent OnPlaced;
    public UnityEvent OnGrabbed;
    public UnityEvent OnReleased;

    protected override void Awake() {
        base.Awake();
        _interactionManager = FindObjectOfType<XRInteractionManager>();
        _rigidBody = GetComponent<Rigidbody>();
        OnPlaced = new UnityEvent();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        AddGrabEvent(args.interactorObject.transform.gameObject);
        _interactor = args.interactorObject;
        _rigidBody.constraints = RigidbodyConstraints.None;
        OnGrabbed.Invoke();
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        RemoveGrabEvent(args.interactorObject.transform.gameObject);
        _interactor = null;
        _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        OnReleased.Invoke();
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
            transform.parent = null;
            transform.position = _placementLocation.transform.position;
            transform.rotation = _placementLocation.transform.rotation;
            _rigidBody.constraints = RigidbodyConstraints.FreezeAll;

            _placementLocation = null;
            // Destroy(_placementLocation);
            OnPlaced.Invoke();
        }
    }
}
