using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start() {
        GenerateEmojiBalls();
    }

    // Will be called once
    private void GenerateEmojiBalls() {
        int numPositions = _transforms.Length;
        for(int i = 0; i < numPositions - 1; i++) {
            Transform transform = _transforms[i];
            GameObject emojiBall = Instantiate(EmojiBallPrefabs[i], transform);

            _instantiatedEmojiBallPrefabs[emojiBall.tag] = emojiBall;

            // These will not need to be set again
            _emojiBallTransforms[emojiBall.tag] = transform;
            _tagToEmojiBall[emojiBall.tag] = EmojiBallPrefabs[i];

            HookupEmojiBallEvents(emojiBall);
        }
    }

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
        GameObject emojiBallPrefab = _tagToEmojiBall[tag];
        Transform emojiTransform = _emojiBallTransforms[tag];

        GameObject emojiBall = Instantiate(emojiBallPrefab, emojiTransform);
        _instantiatedEmojiBallPrefabs[tag] = emojiBall;
        HookupEmojiBallEvents(emojiBall);
    }

    private void RemoveEmojiBallSpawn(string tag) {
        GameObject duplicate = _instantiatedEmojiBallPrefabs[tag];
        if (duplicate) {
            Destroy(duplicate);
            _instantiatedEmojiBallPrefabs[tag] = null;
        }
    }

    private void OnEmojiBallPlaced(GameObject emojiBall) {
        _placedEmojiBalls.Add(emojiBall);
        _instantiatedEmojiBallPrefabs[emojiBall.tag] = null;

        // If that emoji is placed, spawn a duplicate
        SpawnEmojiBall(emojiBall.tag);

        PrintAllPlacedEmojis();
    }

    private void OnEmojiBallGrabbed(GameObject emojiBall) {

        if(_placedEmojiBalls.Contains(emojiBall)) {
            _placedEmojiBalls.Remove(emojiBall);

            // Remove duplicate that spawned
            RemoveEmojiBallSpawn(emojiBall.tag);

            _instantiatedEmojiBallPrefabs[emojiBall.tag] = emojiBall;
        }

        PrintAllPlacedEmojis();
    }

    private void PrintAllPlacedEmojis() {

        string placedEmojis = "Placed emojis: ";
        foreach(GameObject emojiBall in _placedEmojiBalls) {
            placedEmojis += emojiBall.tag + " | ";
        }
        Debug.Log(placedEmojis);
    }
    
}
