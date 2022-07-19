using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EmojiBall : XRBaseInteractable { 
    internal enum EmojiBallState {
        Grabbed,
        Released,
        Stretching,
        StretchingInPlace,
        Placed,
    }

    public XRSimpleInteractable _secondGrabPoint;
    private IXRSelectInteractor _interactor;
    private XRInteractionManager _interactionManager;

    private GameObject _secondGrabContact;
    private GameObject _primaryGrabContact;
    private GameObject _placementLocation;

    private EmojiBallState _state;

    private Vector3 _theoreticalScale;
    

    protected override void Awake() {
        base.Awake();
        _interactionManager = FindObjectOfType<XRInteractionManager>();
        _secondGrabPoint.selectEntered.AddListener(OnSecondHandGrab);
        _secondGrabPoint.selectExited.AddListener(OnSecondHandRelease);
        _state = EmojiBallState.Released;
    }

    private void Start() {
        _secondGrabPoint.gameObject.SetActive(false);
    }

    // --- Scaling ---
    private void OnSecondHandGrab(SelectEnterEventArgs args) {
        _secondGrabContact = args.interactorObject.transform.gameObject;
        _state = EmojiBallState.Stretching;
        _theoreticalScale = transform.localScale;
    }

    private void OnSecondHandRelease(SelectExitEventArgs args) {
        _secondGrabContact = null;
        _state = EmojiBallState.Placed;
    }


    /**
     * TODO:
     * 
     * Try scaling by measuring distance between center point and second grab position.
     * 
     * distance = second grab - center point
     * mag = distance.mag
     * scale = mag / (localScale.mag / 2);
     * 
     * - Consider the fact that radius is about half the diameter, so we need to divide by 2
     */
    private void FixedUpdate() {
        if (_primaryGrabContact && _secondGrabContact) {
            Vector3 primaryGrabPosition = _primaryGrabContact.transform.position;
            Vector3 secondGrabPosition = _secondGrabContact.transform.position;

            Vector3 scaleDirection = secondGrabPosition - primaryGrabPosition;
            float amount = scaleDirection.magnitude;
            amount = Mathf.Lerp(-0.1f, 0.1f, Mathf.InverseLerp(0, 2f, amount));
            Debug.Log(amount);

            transform.localScale += Vector3.one * amount;
        }
    }


    // --- Grabbing ---
    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        if(_state == EmojiBallState.Released) {
            transform.parent = args.interactorObject.transform;
            _state = EmojiBallState.Grabbed;
            AddGrabEvent(args.interactorObject.transform.gameObject);
        }
        else if(_state == EmojiBallState.Placed) {
            _primaryGrabContact = args.interactorObject.transform.gameObject;
            _secondGrabPoint.gameObject.SetActive(true);
        }
        _interactor = args.interactorObject;
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        if(_state == EmojiBallState.Grabbed) {
            transform.parent = null;
            _state = EmojiBallState.Released;
            RemoveGrabEvent(args.interactorObject.transform.gameObject);
        }
        else if(_state == EmojiBallState.Stretching) {
            _primaryGrabContact = null;
            _secondGrabPoint.gameObject.SetActive(false);
        }
        _interactor = null;
    }

    private void AddGrabEvent(GameObject controller) {
        ControllerAdditions controllerEvents = controller.GetComponent<ControllerAdditions>();
        if(controllerEvents) {
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


    // --- Placement / Interaction --- 
    private void OnTriggerEnter(Collider collision) {
        GameObject detected = collision.gameObject;
        PlacementHint hint = collision.gameObject.GetComponent<PlacementHint>();

        if(hint) {
            if(_placementLocation) {
                _placementLocation.GetComponent<PlacementHint>().HideHint();
                
            }
            _placementLocation = detected;
            hint.ShowHint();
        }

    }

    private void OnTriggerExit(Collider collision) {
        GameObject leaving = collision.gameObject;
        PlacementHint hint = collision.gameObject.GetComponent<PlacementHint>();

        if (leaving) {
            _placementLocation = null;
            hint.HideHint();
        }
    }

    private void PlaceEmojiBall() {
        if(_placementLocation) {
            _interactionManager.CancelInteractorSelection(_interactor);
            transform.parent = null;
            transform.position = _placementLocation.transform.position;
            _state = EmojiBallState.Placed;
        }
    }
}
