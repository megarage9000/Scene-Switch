using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MangerSpawner : MonoBehaviour
{
    public GameObject EmojiBallManagerPrefab;

    // For testing only, not needed
    public GameObject ProcTap;
    public GameObject ProcScale;

    public Transform TapLocation;
    public Transform ScaleLocation;
   
    public void SpawnEmojiBallManager() {
        GameObject manager = PhotonNetwork.Instantiate(EmojiBallManagerPrefab.name, transform.position, transform.rotation);
        GameObject tapObject = PhotonNetwork.Instantiate(ProcTap.name, TapLocation.position, TapLocation.rotation);
        GameObject scaleObject = PhotonNetwork.Instantiate(ProcScale.name, ScaleLocation.position, ScaleLocation.rotation);

        EventCaller tapCaller = tapObject.GetComponent<EventCaller>();
        if(tapCaller) {
            Debug.Log("Adding listener tap");
            tapCaller.OnContact.AddListener(manager.GetComponent<EmojiBallManager>().EnableEmojiBallTap);
        }

        EventCaller scaleCaller = scaleObject.GetComponent<EventCaller>();
        if (scaleCaller) {
            Debug.Log("Adding listener scale");
            scaleCaller.OnContact.AddListener(manager.GetComponent<EmojiBallManager>().EnableEmojiBallScale);
        }

        Debug.Log("Calling master client ball generation");
        manager.GetComponent<EmojiBallManager>().GenerateEmojiBalls();
        
        Destroy(gameObject, 2f);
    }
}
