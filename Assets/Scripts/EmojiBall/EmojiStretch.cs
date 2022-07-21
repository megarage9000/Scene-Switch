using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using UnityEngine.Events;

public class EmojiStretch : XRBaseInteractable
{

    public XRSimpleInteractable _secondGrabPoint;
    public UnityEvent OnRescaled;

    private GameObject _secondGrabContact;
    private GameObject _primaryGrabContact;
    private PhotonView _photonView;

    protected override void Awake() {
        base.Awake();
        _photonView = GetComponent<PhotonView>();
        _secondGrabPoint.selectEntered.AddListener(OnSecondHandGrab);
        _secondGrabPoint.selectExited.AddListener(OnSecondHandRelease);
        _secondGrabPoint.gameObject.SetActive(false);
    }

    // --- Scaling ---
    private void OnSecondHandGrab(SelectEnterEventArgs args) {
        _secondGrabContact = args.interactorObject.transform.gameObject; ;
    }

    private void OnSecondHandRelease(SelectExitEventArgs args) {
        _secondGrabContact = null;
        OnRescaled.Invoke();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        _photonView.RequestOwnership();
        base.OnSelectEntered(args);
        _primaryGrabContact = args.interactorObject.transform.gameObject;
        _secondGrabPoint.gameObject.SetActive(true);
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        _primaryGrabContact = null;
        _secondGrabPoint.gameObject.SetActive(false);
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
        if (_primaryGrabContact && _secondGrabContact && !_primaryGrabContact.Equals(_secondGrabContact)) {
            Debug.Log($"Primary = {_primaryGrabContact.name}, Secondary = {_secondGrabContact.name}");
            Vector3 primaryGrabPosition = _primaryGrabContact.transform.position;
            Vector3 secondGrabPosition = _secondGrabContact.transform.position;
            Vector3 distanceVec3 = secondGrabPosition - primaryGrabPosition;
            float distance = distanceVec3.magnitude;

            // Vector3 newScale = Vector3.Lerp(transform.localScale, Vector3.one * distance, 0.25f);
            transform.localScale = Vector3.one * distance;
            //Debug.Log(transform.localScale);
        }
    }

}
