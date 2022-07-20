using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiBallManager : MonoBehaviour
{
    [Header("List of Emoji Balls to instantiate")]
    [SerializeField]
    public List<GameObject> EmojiBallPrefabs;
    private List<GameObject> _instantiatedEmojiBallPrefabs;
    private Transform[] _transforms;
   
    private void Awake() {
        _instantiatedEmojiBallPrefabs = new List<GameObject>();
        _transforms = GetComponentsInChildren<Transform>();
    }

    private void Start() {
        GenerateEmojiBalls();
    }

    private void GenerateEmojiBalls() {
        int numPositions = _transforms.Length;
        for(int i = 0; i < numPositions - 1; i++) {
            Debug.Log(i);
            Transform transform = _transforms[i];
            GameObject emojiBall = Instantiate(EmojiBallPrefabs[i], transform);
            _instantiatedEmojiBallPrefabs.Add(emojiBall);
        }
    }

    private void ClearEmojiBalls() {
        foreach(GameObject emojiBall in _instantiatedEmojiBallPrefabs) {
            GameObject temp = emojiBall;
            Destroy(emojiBall);
            temp = null;
        }
        _instantiatedEmojiBallPrefabs.Clear();
    }

    
}
