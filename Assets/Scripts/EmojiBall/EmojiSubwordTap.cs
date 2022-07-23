using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class EmojiSubwordTap : NetworkAdditions {

    public GameObject subwordPrefab;
    public UnityEvent OnSubwordTap;

    private List<GameObject> _subwordObjects;

    private void Awake() {
        _subwordObjects = new List<GameObject>();
    }

    // Thanks to
    // https://answers.unity.com/questions/1661755/how-to-instantiate-objects-in-a-circle-formation-a.html
    public void SpawnSubWords(List<Material> subWords) {
        if (_subwordObjects.Count > 0) {
            return;
        }
        int num = subWords.Count;
        float radius = 2f;
        Vector3 center = Vector3.zero;

        for (int i = 0; i < num; i++) {

            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(0, horizontal, vertical);

            /* Get the spawn position */
            var spawnPos = center + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var subword = PhotonNetwork.Instantiate(subwordPrefab.name, Vector3.zero, Quaternion.identity) as GameObject;
            subword.transform.parent = gameObject.transform;
            subword.transform.localPosition = spawnPos;
            Debug.Log("Calling SetMaterial hereerr?");
            subword.GetComponent<NetworkAdditions>().SetMaterial(subWords[i]);

            /* Rotate the enemy to face towards player */
            subword.transform.rotation = Quaternion.LookRotation(transform.TransformDirection(Vector3.forward));

            /* Adjust height */
            // subword.transform.Translate(new Vector3(0, subword.transform.localScale.y / 2, 0));

            SubWordTap tapScript = subword.GetComponent<SubWordTap>();
            if (tapScript) {
                tapScript.OnTap += ChangeMaterial;
            }

            _subwordObjects.Add(subword);
        }
    }

    public void RemoveSubWords() {
        foreach (GameObject word in _subwordObjects) {
            GameObject temp = word;
            word.GetComponent<SubWordTap>().DestroyNetworkObject();
            temp = null;
        }
        _subwordObjects.Clear();
    }
    private void ChangeMaterial(Material material) {
        SetMaterial(material);
        RemoveSubWords();
        OnSubwordTap.Invoke();
    }
}


