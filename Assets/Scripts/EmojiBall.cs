using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EmojiBall : XRBaseInteractable
{

    public XRSimpleInteractable _secondGrabPoint;
    private SphereCollider _collider;
    private float _scaleDown = 0.5f;

    protected override void Awake() {
        base.Awake();
        _secondGrabPoint.selectEntered.AddListener(OnSecondHandGrab);
        _secondGrabPoint.selectExited.AddListener(OnSecondHandRelease);
        _collider = GetComponent<SphereCollider>();
    }

    private void OnSecondHandGrab(SelectEnterEventArgs args) {
        Debug.Log("Second Hand grabbed!");
    }

    private void OnSecondHandRelease(SelectExitEventArgs args) {
        Debug.Log("Second Hand released!");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        transform.parent = args.interactorObject.transform;
        _collider.enabled = false;
        // _collider.radius *= _scaleDown;
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        transform.parent = null;
        _collider.enabled = true;
        // _collider.radius *= 1f / _scaleDown;
    }

    /*
    public override bool IsSelectableBy(IXRSelectInteractor interactor) {
        bool isAlreadySelected = interactorsSelecting.Count > 0;
        return base.IsSelectableBy(interactor) && !isAlreadySelected;
    }
    */
}
