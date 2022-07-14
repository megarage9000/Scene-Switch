using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjects : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;

    void Update()
    {
        if (objectToFollow == null)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.transform.position = objectToFollow.transform.position;
        }
        
    }
}
