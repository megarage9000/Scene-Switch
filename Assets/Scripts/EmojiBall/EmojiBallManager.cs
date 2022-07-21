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
            Destroy(emojiBall);
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

    private void RemoveEmojiBallSpawn(string tag) {

        // Checks if there exists an emoji ball with given tag,
        // deletes it if there is
        GameObject duplicate = _instantiatedEmojiBallPrefabs[tag];
        if (duplicate) {
            Destroy(duplicate);
            _instantiatedEmojiBallPrefabs[tag] = null;
        }
    }

    // ---- Emoji Ball Listeners ---- // 
    private void OnEmojiBallPlaced(GameObject emojiBall) {

        // Add emoji ball on list of placed emoji balls
        _placedEmojiBalls.Add(emojiBall);

        // Since its placed, we need to set the instatiated emoji ball prefabs
        // on its tag to be null to spawn its replacement
        _instantiatedEmojiBallPrefabs[emojiBall.tag] = null;

        // If that emoji is placed, spawn a duplicate
        SpawnEmojiBall(emojiBall.tag);

        PrintAllPlacedEmojis();
    }

    private void OnEmojiBallGrabbed(GameObject emojiBall) {

        // Only applies to balls placed
        if(_placedEmojiBalls.Contains(emojiBall)) {

            // Remove emoji ball if it is in placed ball list
            _placedEmojiBalls.Remove(emojiBall);

            // Remove duplicate that spawned
            RemoveEmojiBallSpawn(emojiBall.tag);

            // Reset instaitated emoji ball prefabs at that tag
            // to this ball
            _instantiatedEmojiBallPrefabs[emojiBall.tag] = emojiBall;
        }

        PrintAllPlacedEmojis();
    }

    // ---- Emoji Ball State Changes ----- //

    public void EnableEmojiBallTap() {

        ClearEmojiBalls();

        foreach (GameObject emojiBall in _placedEmojiBalls) {
            EmojiBall emojiBallScript = emojiBall.GetComponent<EmojiBall>();

            if(emojiBallScript) {
                emojiBallScript.DisableGrab();
                emojiBallScript.EnableEmojiTap();
            }
        }
    }

    public void EnableEmojiBallScale() {
        foreach (GameObject emojiBall in _placedEmojiBalls) {
            EmojiBall emojiBallScript = emojiBall.GetComponent<EmojiBall>();

            if (emojiBallScript) {
                emojiBallScript.DisableEmojiTap();
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
