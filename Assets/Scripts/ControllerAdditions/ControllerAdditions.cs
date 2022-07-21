using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class ControllerAdditions : MonoBehaviour
{
    private const string RIGHT_CONTROLLER = "RightHand Controller";
    private const string LEFT_CONTROLLER = "LeftHand Controller";

    private InputDevice _controller;
    private List<InputDevice> devices;

    public UnityEvent PrimaryPress;
    public UnityEvent SecondaryPress;

    private void Awake() {
        PrimaryPress = new UnityEvent();
        SecondaryPress = new UnityEvent();
        devices = new List<InputDevice>();
        StartCoroutine(FindController());
    }

    IEnumerator FindController() {
        var controllerCharacteristics = InputDeviceCharacteristics.Controller;
        string name = gameObject.name;
        if (name.Equals(RIGHT_CONTROLLER)) {
            controllerCharacteristics = InputDeviceCharacteristics.Right;
        }
        else if (name.Equals(LEFT_CONTROLLER)) {
            controllerCharacteristics = InputDeviceCharacteristics.Left;
        }
        while (_controller != null) {

            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
            if(devices.Count > 0) {
                _controller = devices[0];
            }
            yield return null;
        }
    }

    private void Update() {
        if (_controller != null) {
            _controller.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPress);
            if (primaryPress) {
                PrimaryPress.Invoke();
                Debug.Log($"Primary press on {gameObject.name}");
            }

            _controller.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryPress);
            if (secondaryPress) {
                SecondaryPress.Invoke();
                Debug.Log($"Secondary press on {gameObject.name}");
            }
        }
    }
}
