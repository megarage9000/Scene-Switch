using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EmojiBallManager : MonoBehaviour
{
    [Header("List of Emoji Balls to instantiate")]
    [SerializeField]
    public List<GameObject> EmojiBallPrefabs;
   
    private List<GameObject> _placedEmojiBalls;
    private Transform[] _transforms;
    private PhotonView _photonView;

    // To track duplicates
    private Dictionary<string, GameObject> _tagToEmojiBall;
    private Dictionary<string, Transform> _emojiBallTransforms;
    private Dictionary<string, GameObject> _instantiatedEmojiBallPrefabs;

    private void Awake() {
        _placedEmojiBalls = new List<GameObject>();
        _tagToEmojiBall = new Dictionary<string, GameObject>();
        _instantiatedEmojiBallPrefabs = new Dictionary<string, GameObject>();
        _emojiBallTransforms = new Dictionary<string, Transform>();
        _transforms = GetComponentsInChildren<Transform>();
        _photonView = GetComponent<PhotonView>();
    }

    // Will be called once
    public void GenerateEmojiBalls() {

        if(PhotonNetwork.IsMasterClient == false) {
            Debug.Log("Balls have been generated already! Exiting function...");
            return;
        }
        Debug.Log("Generating Emoji Balls...");

        int numPositions = _transforms.Length;

        // For some reason, numPositions also includes the parent
        // transform collected from getting children transforms,
        // so add - 1
        for(int i = 0; i < numPositions - 1; i++) {
            Transform transform = _transforms[i];
            GameObject emojiBall = PhotonNetwork.Instantiate(EmojiBallPrefabs[i].name, transform.position, transform.rotation);

            _instantiatedEmojiBallPrefabs[emojiBall.tag] = emojiBall;

            // These will not need to be set again
            _emojiBallTransforms[emojiBall.tag] = transform;
            _tagToEmojiBall[emojiBall.tag] = EmojiBallPrefabs[i];

            HookupEmojiBallEvents(emojiBall);
        }

        
    }

    // Hooks all necessary listeners on the manager onto the emoji balls
    private void HookupEmojiBallEvents(GameObject emojiBall) {
        EmojiBall emojiBallScript = emojiBall.GetComponent<EmojiBall>();
        if(emojiBallScript) {
            emojiBallScript.OnGrabbed += OnEmojiBallGrabbed;
            emojiBallScript.OnPlaced += OnEmojiBallPlaced;
        }
    }

    private void ClearEmojiBalls() {
        foreach(KeyValuePair<string, GameObject> pair in _instantiatedEmojiBallPrefabs) {
            GameObject emojiBall = pair.Value;
            emojiBall.GetComponent<EmojiBall>().DestroyEmojiBall();
            emojiBall = null;
        }
        _instantiatedEmojiBallPrefabs.Clear();
    }

    // Duplicate detection
    /*
     * _tagToEmojiBall = maps the emoji tags to their prefabs
     * _emojiBallTransforms = maps the emoji tags to their spawned transforms
     * _instantiatedEmojiBallPrefabs = tracks which emoji balls are in world, but not placed
     * _placedEmojiBalls = simply stores a list of those emoji balls that are placed
     */

    [PunRPC]
    private void SpawnEmojiBall(string tag) {

        // Get the tag and the transform of the emoji ball
        // to spawn
        GameObject emojiBallPrefab = _tagToEmojiBall[tag];
        Transform emojiTransform = _emojiBallTransforms[tag];

        // Create the spawned ball and hookup manager listeners onto it
        GameObject emojiBall = PhotonNetwork.Instantiate(emojiBallPrefab.name, emojiTransform.position, emojiTransform.rotation);
        HookupEmojiBallEvents(emojiBall);
        
        // Store it into the instantiated balls dictionary
        _instantiatedEmojiBallPrefabs[tag] = emojiBall;
    }

    [PunRPC]
    private void RemoveEmojiBallSpawn(string tag) {

        // Checks if there exists an emoji ball with given tag,
        // deletes it if there is
        GameObject duplicate = _instantiatedEmojiBallPrefabs[tag];
        if (duplicate) {
            duplicate.GetComponent<EmojiBall>().DestroyEmojiBall();
            _instantiatedEmojiBallPrefabs[tag] = null;
        }
    }

    [PunRPC]
    private void AddToPlacedEmojis(string tag) {
        GameObject emojiBall = _instantiatedEmojiBallPrefabs[tag];
        _placedEmojiBalls.Add(emojiBall);
        _instantiatedEmojiBallPrefabs[tag] = null;

        SpawnEmojiBall(tag);
    }

    [PunRPC]
    private void RemoveFromPlacedEmojis(int id) {
        foreach (var emojiBall in _placedEmojiBalls) {
            EmojiBall emojiScript = emojiBall.GetComponent<EmojiBall>();    
            if(emojiScript && emojiScript.GetViewID() == id) {
                _placedEmojiBalls.Remove(emojiBall);
                RemoveEmojiBallSpawn(emojiBall.tag);
                // Reset instaitated emoji ball prefabs at that tag
                // to this ball
                _instantiatedEmojiBallPrefabs[emojiBall.tag] = emojiBall;
                break;
            }
        }
    }
    // ---- Emoji Ball Listeners ---- // 
    private void OnEmojiBallPlaced(GameObject emojiBall) {
        // If that emoji is placed, spawn a duplicate
        if (PhotonNetwork.IsMasterClient) {
            Debug.Log($"{emojiBall.name} with {emojiBall.GetComponent<EmojiBall>().GetViewID()} called OnEmojiBallPlaced on master client");
            AddToPlacedEmojis(emojiBall.tag);
        }
        else {
            Debug.Log($"{emojiBall.name} with {emojiBall.GetComponent<EmojiBall>().GetViewID()} called OnEmojiBallPlaced on non-master client");
            _photonView.RPC("AddToPlacedEmojis", RpcTarget.MasterClient, emojiBall.tag);
        }

        PrintAllPlacedEmojis();
    }

    private void OnEmojiBallGrabbed(GameObject emojiBall) {
        // Remove duplicate that spawned
        if(PhotonNetwork.IsMasterClient) {
            int id = emojiBall.GetComponent<EmojiBall>().GetViewID();
            Debug.Log($"{emojiBall.name} with {emojiBall.GetComponent<EmojiBall>().GetViewID()} called OnEmojiBallGrabbed grab on master client");
            RemoveFromPlacedEmojis(id);
        }
        else {
            int id = emojiBall.GetComponent<EmojiBall>().GetViewID();
            Debug.Log($"{emojiBall.name} with {emojiBall.GetComponent<EmojiBall>().GetViewID()} called OnEmojiBallGrabbed grab on non-master client");
            _photonView.RPC("RemoveFromPlacedEmojis", RpcTarget.MasterClient, id);
        }
    }

    // ---- Emoji Ball State Changes ----- //
    public void EnableEmojiBallTap() {
        if(PhotonNetwork.IsMasterClient) {
            EnableEmojiBallTapNetwork();
        }
        else {
            _photonView.RPC("EnableEmojiBallTapNetwork", RpcTarget.MasterClient);
        }
    }

    public void EnableEmojiBallScale() {
        if (PhotonNetwork.IsMasterClient) {
            EnableEmojiBallScaleNetwork();
        }
        else {
            _photonView.RPC("EnableEmojiBallScaleNetwork", RpcTarget.MasterClient);
        }
    }

    // Master client call to enable emoji tap
    [PunRPC]
    private void EnableEmojiBallTapNetwork() {
        Debug.Log("Enabling Emoji Balls Tap");
        _photonView.RequestOwnership();
        ClearEmojiBalls();

        foreach (GameObject emojiBall in _placedEmojiBalls) {
            EmojiBall emojiBallScript = emojiBall.GetComponent<EmojiBall>();
            int id = emojiBallScript.GetViewID();

            if (emojiBallScript) {
                emojiBallScript.DisableGrab();
                emojiBallScript.EnableEmojiTap();
            }

            // Also call for clients
            _photonView.RPC("EnableEmojiBallTapClient", RpcTarget.All, id);
        }
    }

    // Other client call to enable emoji tap
    [PunRPC]
    private void EnableEmojiBallTapClient(int id) {
        if(PhotonNetwork.IsMasterClient) {
            return;
        }
        Debug.Log($"Calling client side tap on id {id}");
        PhotonView emojiBallView = PhotonView.Find(id);
        if(emojiBallView) {
            Debug.Log($"Found photonview on id {id}");
            EmojiBall emojiBallScript = emojiBallView.gameObject.GetComponent<EmojiBall>();
            if (emojiBallScript) {
                Debug.Log($"Found emojiballscript on id {id}");
                emojiBallScript.DisableGrab();
                // emojiBallScript.EnableEmojiTap();
            }
        }

    }

    [PunRPC]
    private void EnableEmojiBallScaleNetwork() {
        Debug.Log("Enabling Emoji Balls Scale");
        _photonView.RequestOwnership();
        foreach (GameObject emojiBall in _placedEmojiBalls) {

            EmojiBall emojiBallScript = emojiBall.GetComponent<EmojiBall>();
            int id = emojiBallScript.GetViewID();

            if (emojiBallScript) {
                emojiBallScript.DisableEmojiTap();
                emojiBallScript.DisableSubwordTap();
                emojiBallScript.EnableScale();
            }

            _photonView.RPC("EnableEmojiBallScaleClient", RpcTarget.All, id);
        }
    }

    // Other client call to enable emoji scale
    [PunRPC]
    private void EnableEmojiBallScaleClient(int id) {
        if (PhotonNetwork.IsMasterClient) {
            return;
        }
        Debug.Log($"Calling client side scale on id {id}");
        PhotonView emojiBallView = PhotonView.Find(id);
        if (emojiBallView) {
            EmojiBall emojiBallScript = emojiBallView.gameObject.GetComponent<EmojiBall>();
            if (emojiBallScript) {
                // emojiBallScript.DisableEmojiTap();
                // emojiBallScript.DisableSubwordTap();
                emojiBallScript.EnableScale();
            }
        }
    }

    private void PrintAllPlacedEmojis() {
        string placedEmojis = "Placed emojis: ";
        foreach(GameObject emojiBall in _placedEmojiBalls) {
            placedEmojis += emojiBall.tag + " | ";
        }
        Debug.Log(placedEmojis);
    }
    
}
