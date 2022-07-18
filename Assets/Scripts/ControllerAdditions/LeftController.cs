using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeftController : ButtonInput
{
    private void Awake() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerChars = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerChars, devices);
    }

    private void Update() {
        
    }
}
