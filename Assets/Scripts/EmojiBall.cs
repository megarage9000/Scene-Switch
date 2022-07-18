using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EmojiBall : XRBaseInteractable { 
    public enum EmojiBallState {
        Grabbed,
        Released,
        Stretching,
        Placed
    }

    public XRSimpleInteractable _secondGrabPoint;
    private SphereCollider _collider;

    private GameObject _secondGrabContact;
    private GameObject _primaryGrabContact;
    private GameObject _placementLocation;

    private EmojiBallState _state;
    

    protected override void Awake() {
        base.Awake();
        _secondGrabPoint.selectEntered.AddListener(OnSecondHandGrab);
        _secondGrabPoint.selectExited.AddListener(OnSecondHandRelease);
        _collider = GetComponent<SphereCollider>();
        _state = EmojiBallState.Released;
    }

    private void Start() {
        _secondGrabPoint.gameObject.SetActive(false);
    }

    // --- Scaling ---
    private void OnSecondHandGrab(SelectEnterEventArgs args) {
        _secondGrabContact = args.interactorObject.transform.gameObject;
        _state = EmojiBallState.Stretching;
    }

    private void OnSecondHandRelease(SelectExitEventArgs args) {
        _secondGrabContact = null;
        if(_primaryGrabContact) {
            _state = EmojiBallState.Grabbed;
        }
        else {
            _state = EmojiBallState.Released;
        }
    }

    private void FixedUpdate() {
        if (_primaryGrabContact && _secondGrabContact) {
            Vector3 primaryGrabPosition = _primaryGrabContact.transform.position;
            Vector3 secondGrabPosition = _secondGrabContact.transform.position;

            Vector3 scaleDirection = secondGrabPosition - primaryGrabPosition;
            float amount = scaleDirection.magnitude;

            transform.localScale = Vector3.one * amount;
        }
    }

    // --- Grabbing ---
    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        if(_state == EmojiBallState.Released) {
            transform.parent = args.interactorObject.transform;
            _state = EmojiBallState.Grabbed;
        }
        _primaryGrabContact = args.interactorObject.transform.gameObject;
        _secondGrabPoint.gameObject.SetActive(true);
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        if(_state == EmojiBallState.Grabbed) {
            transform.parent = null;
            _state = EmojiBallState.Released;
        }
        _primaryGrabContact = null;
        _secondGrabPoint.gameObject.SetActive(false);
    }

    // --- Placement / Interaction --- 

    private void OnCollisionEnter(Collision collision) {
        GameObject detected = collision.gameObject;
        PlacementHint hint = collision.gameObject.GetComponent<PlacementHint>();

        Debug.Log($"detected {detected.name}");

        if(hint) {
            if(_placementLocation) {
                _placementLocation.GetComponent<PlacementHint>().HideHint();
                
            }
            _placementLocation = detected;
            hint.ShowHint();
        }

    }

    private void OnCollisionExit(Collision collision) {
        GameObject leaving = collision.gameObject;
        PlacementHint hint = collision.gameObject.GetComponent<PlacementHint>();

        if (leaving) {
            _placementLocation = null;
            hint.HideHint();
        }
    }

    public void PlaceEmojiBall() {
        if(_placementLocation) {
            transform.position = _placementLocation.transform.position;
            _state = EmojiBallState.Placed;
        }
    }

}
