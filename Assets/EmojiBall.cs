using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EmojiBall : XRBaseInteractable
{

    private List<IXRSelectInteractor> _contactedInteractors;

    protected override void Awake() {
        base.Awake();
        _contactedInteractors = new List<IXRSelectInteractor>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);

        // Get the selector detected 
        _contactedInteractors.Add(args.interactorObject);

        int numInteractors = _contactedInteractors.Count;

        // Follow the interactor
        if(numInteractors == 1) {

        }

        // Scale the object
        else if(numInteractors == 2) {

        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);

        _contactedInteractors.Remove(args.interactorObject);

        int numInteractors = _contactedInteractors.Count;
        // Follow the interactor
        if (numInteractors == 1) {

        }
    }

    private void FixedUpdate() {
        
    }
}
