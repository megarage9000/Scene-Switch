using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiBallManager : MonoBehaviour
{
    [Header("Material assignment for emoji balls")]
    [SerializeField]
    public List<Material> EmojiMaterials;
    [SerializeField]
    public List<List<Material>> SubwordMaterials;

    // Start is called before the first frame update
    void Start() {
        GenerateSubWords();
    }

    public void GenerateSubWords() {

    }
}
