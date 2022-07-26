using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ManagerSpawner : MonoBehaviour
{
    public GameObject EmojiBallManagerPrefab;
    public GameObject MannequinPrefab;

    // For testing only, not needed
    public GameObject ProcTap;
    public GameObject ProcScale;

    public Transform TapLocation;
    public Transform ScaleLocation;
    public Transform MannequinLocation;
   
    public void SpawnEmojiBallManager() {
        Debug.Log("MANAGER: 1");
        GameObject manager = PhotonNetwork.Instantiate(EmojiBallManagerPrefab.name, transform.position, transform.rotation);
        GameObject tapObject = PhotonNetwork.Instantiate(ProcTap.name, TapLocation.position, TapLocation.rotation);
        GameObject scaleObject = PhotonNetwork.Instantiate(ProcScale.name, ScaleLocation.position, ScaleLocation.rotation);
        PhotonNetwork.Instantiate(MannequinPrefab.name, MannequinLocation.position, MannequinLocation.rotation);
        Debug.Log("MANAGER: 1: " + manager);
        Debug.Log("MANAGER: 1: " + tapObject);
        Debug.Log("MANAGER: 1: " + scaleObject);

        Debug.Log("MANAGER: 2");
        EventCaller tapCaller = tapObject.GetComponent<EventCaller>();
        if(tapCaller) {
            Debug.Log("Adding listener tap");
            tapCaller.OnContact.AddListener(manager.GetComponent<EmojiBallManager>().EnableEmojiBallTap);
        }

        Debug.Log("MANAGER: 3");
        EventCaller scaleCaller = scaleObject.GetComponent<EventCaller>();
        if (scaleCaller) {
            Debug.Log("Adding listener scale");
            scaleCaller.OnContact.AddListener(manager.GetComponent<EmojiBallManager>().EnableEmojiBallScale);
        }

        Debug.Log("MANAGER: 4");
        Debug.Log("Calling master client ball generation");
        manager.GetComponent<EmojiBallManager>().GenerateEmojiBalls();
        
        Destroy(gameObject, 2f);
    }
}
