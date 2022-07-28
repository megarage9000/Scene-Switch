using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffOn : MonoBehaviour
{
    void Start(){
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
