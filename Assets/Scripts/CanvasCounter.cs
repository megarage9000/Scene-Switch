using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasCounter : MonoBehaviour
{
    public int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            i++;
            i.ToString();
        }
    }
}
