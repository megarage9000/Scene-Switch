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

   
    // Generates all the given emoji balls. Ideally this should be called once,
    // and will execute properly if under the master client
    public void GenerateEmojiBalls() {

        if(PhotonNetwork.IsMasterClient == false) {
            Debug.Log("Balls have been generated already! Exiting function...");
            // Destroy self if not in master client
            Destroy(gameObject);
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
            emojiBallScript.OnSubwordTapped += OnEmojiBallMaterialChanged;
            emojiBallScript.OnScaled += OnEmojiBallScaled;
        }
    }

    // Clears all the emoji balls in the _instantiatedEmojiBallPrefabs
    private void ClearEmojiBalls() {
        foreach(KeyValuePair<string, GameObject> pair in _instantiatedEmojiBallPrefabs) {
            GameObject emojiBall = pair.Value;
            emojiBall.GetComponent<EmojiBall>().DestroyNetworkObject();
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

    // Creates a duplicate of the emoji ball with its given tag,
    // and places it in the _instantiatedEmojiBallPrefabs dictionary
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

    // Removes the duplicate found in the _instantiatedEmojiBallPrefabs dictionary,
    // sets the value at the given tag in the dictionary to null as well.
    [PunRPC]
    private void RemoveEmojiBallSpawn(string tag) {

        // Checks if there exists an emoji ball with given tag,
        // deletes it if there is
        GameObject duplicate = _instantiatedEmojiBallPrefabs[tag];
        if (duplicate) {
            duplicate.GetComponent<EmojiBall>().DestroyNetworkObject();
            _instantiatedEmojiBallPrefabs[tag] = null;
        }
    }

    // Simply looks through the _instantiatedEmojiBallPrefabs for the 
    // emojiBall that just got placed, and adds it to the _placedEmojiBalls
    // list. Also calls SpawnEmojiBall(tag)
    [PunRPC]
    private void AddToPlacedEmojis(string tag) {
        // Get the placed emojiBall, and put it in 
        // the _placeEmojiBalls
        GameObject emojiBall = _instantiatedEmojiBallPrefabs[tag];
        _placedEmojiBalls.Add(emojiBall);

        // Spawn a duplicate of the placed ball
        _instantiatedEmojiBallPrefabs[tag] = null;
        SpawnEmojiBall(tag);
    }

    // Looks for the the emoji ball with a given view id
    // and removes it from the _placeEmojiBalls list. Also
    // calls RemoveEmojiBallSpawn(emojiBall.tag)
    [PunRPC]
    private void RemoveFromPlacedEmojis(int id) {
        foreach (var emojiBall in _placedEmojiBalls) {

            // Get the emojiBall script, and make sure it matches the given
            // Photon View Id
            EmojiBall emojiScript = emojiBall.GetComponent<EmojiBall>();    
            if(emojiScript && emojiScript.GetViewID() == id) {

                // Remove the emojiBall from the placed emoji balls,
                // and remove the duplicate spawn
                _placedEmojiBalls.Remove(emojiBall);
                RemoveEmojiBallSpawn(emojiBall.tag);

                // Reset instaitated emoji ball prefabs at that tag
                // to this ball
                _instantiatedEmojiBallPrefabs[emojiBall.tag] = emojiBall;
                break;
            }
        }
    }
    // ------------------------------------------------------------------ Emoji Ball Listeners ------------------------------------------------------------------ //

    // Called everytime any one emojiBall gets placed
    private void OnEmojiBallPlaced(GameObject emojiBall) {
        // If that emoji is placed, spawn a duplicate

        // Again, we want to make sure we update the master client if the event
        // is not triggered there.
        if (PhotonNetwork.IsMasterClient) {
            Debug.Log($"{emojiBall.name} with {emojiBall.GetComponent<EmojiBall>().GetViewID()} called OnEmojiBallPlaced on master client");
            AddToPlacedEmojis(emojiBall.tag);
        }
        else {
            // If on a remote client, perform the RPC call to the master client
            Debug.Log($"{emojiBall.name} with {emojiBall.GetComponent<EmojiBall>().GetViewID()} called OnEmojiBallPlaced on non-master client");
            _photonView.RPC("AddToPlacedEmojis", RpcTarget.MasterClient, emojiBall.tag);
        }

        PrintAllPlacedEmojis();
    }

    // Called everytime any one emojiBall gets grabbed
    private void OnEmojiBallGrabbed(GameObject emojiBall) {
        // Remove duplicate that spawned

        // Again, we want to make sure we update the master client if the event
        // is not triggered there.
        if (PhotonNetwork.IsMasterClient) {
            int id = emojiBall.GetComponent<EmojiBall>().GetViewID();
            Debug.Log($"{emojiBall.name} with {emojiBall.GetComponent<EmojiBall>().GetViewID()} called OnEmojiBallGrabbed grab on master client");
            RemoveFromPlacedEmojis(id);
        }
        else {
            // If on a remote client, perform the RPC call to the master client
            int id = emojiBall.GetComponent<EmojiBall>().GetViewID();
            Debug.Log($"{emojiBall.name} with {emojiBall.GetComponent<EmojiBall>().GetViewID()} called OnEmojiBallGrabbed grab on non-master client");
            _photonView.RPC("RemoveFromPlacedEmojis", RpcTarget.MasterClient, id);
        }
    }

    // Called everytime any one emojiBall gets their material changed
    private void OnEmojiBallMaterialChanged(GameObject emojiBall) {

        if (PhotonNetwork.IsMasterClient) {

        }

    }

    // Called everytime any one emojiBall is rescaled
    private void OnEmojiBallScaled(GameObject emojiBall) {

        if (PhotonNetwork.IsMasterClient) {

        }
    }


    // ------------------------------------------------------------------ Emoji Ball State Changes ------------------------------------------------------------------ //

    // Enables all placed emoji balls to be tappable by users
    public void EnableEmojiBallTap() {
        
        if(PhotonNetwork.IsMasterClient) {
            EnableEmojiBallTapNetwork();
        }
        else {
            _photonView.RPC("EnableEmojiBallTapNetwork", RpcTarget.MasterClient);
        }
    }

    // Enables all placed emoji balls to be scaled by users
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
    public void EnableEmojiBallTapNetwork() {
        Debug.Log("Enabling tap");
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
            _photonView.RPC("EnableEmojiBallTapClient", RpcTarget.Others, id);
        }
    }

    // Non-master client call to enable emoji tap
    [PunRPC]
    public void EnableEmojiBallTapClient(int id) {
        if(PhotonNetwork.IsMasterClient) {
            return;
        }
        PhotonView emojiBallView = PhotonView.Find(id);
        if(emojiBallView) {
            EmojiBall emojiBallScript = emojiBallView.gameObject.GetComponent<EmojiBall>();
            if (emojiBallScript) {
                emojiBallScript.DisableGrab();

                // Note that we don't need to call EnableEmojiTap() for all the emojiBalls
                // on other clients, since that only needs to be done once for the master
                // and all NetworkPlayers have the necessary colliders to tap.

                // If tap was enabled on the client side, the subwords generation would happen
                // on all clients, causing many networked objects to be pooled.
            }
        }

    }

    // Master client call to enable emoji scale
    [PunRPC]
    public void EnableEmojiBallScaleNetwork() {
        Debug.Log("Enabling scale");
        _photonView.RequestOwnership();
        foreach (GameObject emojiBall in _placedEmojiBalls) {

            EmojiBall emojiBallScript = emojiBall.GetComponent<EmojiBall>();
            int id = emojiBallScript.GetViewID();

            if (emojiBallScript) {
                emojiBallScript.DisableEmojiTap();
                emojiBallScript.DisableSubwordTap();
                emojiBallScript.EnableScale();
            }

            _photonView.RPC("EnableEmojiBallScaleClient", RpcTarget.Others, id);
        }
    }

    // Non-master client call to enable emoji scale
    [PunRPC]
    public void EnableEmojiBallScaleClient(int id) {
        if (PhotonNetwork.IsMasterClient) {
            return;
        }
        Debug.Log($"Calling client side scale on id {id}");
        PhotonView emojiBallView = PhotonView.Find(id);
        if (emojiBallView) {
            EmojiBall emojiBallScript = emojiBallView.gameObject.GetComponent<EmojiBall>();
            if (emojiBallScript) {

                // As mentioned before, we don't need clients to have tap enabled on their end, since
                // NetworkPlayers have the correct colliders on there controllers to simulate tap
                // on the Master Client.

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
