using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class ButtonInput : MonoBehaviour
{
    public UnityEvent PrimaryPress;
    public UnityEvent SecondaryPress;
}
