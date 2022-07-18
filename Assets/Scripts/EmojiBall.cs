using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EmojiBall : XRBaseInteractable
{

    public XRSimpleInteractable _secondGrabPoint;
    private SphereCollider _collider;

    private GameObject _secondGrabContact;
    private GameObject _primaryGrabContact;

    protected override void Awake() {
        base.Awake();
        _secondGrabPoint.selectEntered.AddListener(OnSecondHandGrab);
        _secondGrabPoint.selectExited.AddListener(OnSecondHandRelease);
        _collider = GetComponent<SphereCollider>();
    }

    private void OnSecondHandGrab(SelectEnterEventArgs args) {
        _secondGrabContact = args.interactorObject.transform.gameObject;
    }

    private void OnSecondHandRelease(SelectExitEventArgs args) {
        _secondGrabContact = null;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        transform.parent = args.interactorObject.transform;
        _primaryGrabContact = args.interactorObject.transform.gameObject;
        _collider.enabled = false;
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        transform.parent = null;
        _primaryGrabContact = null;
        _collider.enabled = true;
    }

    private void FixedUpdate() {
        if(_primaryGrabContact && _secondGrabContact) {
            Vector3 primaryGrabPosition = _primaryGrabContact.transform.position;
            Vector3 secondGrabPosition = _secondGrabContact.transform.position;

            Vector3 scaleDirection = secondGrabPosition - primaryGrabPosition;
            float amount = scaleDirection.magnitude;

            transform.localScale = Vector3.one * amount;
        }
    }
}
