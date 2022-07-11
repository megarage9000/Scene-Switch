using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkCircling : MonoBehaviour
{
    //variables
    public GameObject sphere; //lets us set what we're orbiting around
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        //empty function for now
    }

    // Update is called once per frame
    void Update()
    {
        OrbitAround(); 
    }

    //make the shark circle around the sphere
    void OrbitAround()
    {
        transform.RotateAround(sphere.transform.position, Vector3.down, speed * Time.deltaTime);
    }
}
