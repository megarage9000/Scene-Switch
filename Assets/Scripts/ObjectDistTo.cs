using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistTo : MonoBehaviour
{
    [SerializeField] GameObject distanceFrom;
    static public bool isClose = false;
    private float triggerDist = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(this.gameObject.transform.position, distanceFrom.transform.position); 
        isClose = (dist < triggerDist) ? true : false;
    }
}
