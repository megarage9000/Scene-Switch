using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerDistToMannequin : MonoBehaviour
{
    [SerializeField] GameObject mannequin;
    static public bool isClose = false;
    private float triggerDist = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(this.gameObject.transform.position, mannequin.transform.position); 
        isClose = (dist < triggerDist) ? true : false;
        Debug.Log(dist);
        Debug.Log(isClose);
    }
}
