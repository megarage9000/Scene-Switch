using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MaterialsTable  {

    static private Dictionary<int, Material> _IdToMaterial;
    static private Dictionary<Material, int> _materialToId;

    static private string[] _folderFiter = { "Assets/Models", "Assets/Materials" };

    [RuntimeInitializeOnLoadMethod]
    static private void loadMaterials() {
        _IdToMaterial = new Dictionary<int, Material>();
        _materialToId = new Dictionary<Material, int>();

        Debug.Log("Loading materials");
        string[] guids = AssetDatabase.FindAssets("t:material", _folderFiter);

        int generatedIds = 0;

        foreach (string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            int Id = generatedIds++;

            _IdToMaterial[Id] = material;
            _materialToId[material] = Id;
        }

        printMaterials();
    }

    static private void printMaterials() {
        Debug.Log("ID to Material");
        foreach(KeyValuePair<int, Material> value in _IdToMaterial) {
            Debug.Log($"{value.Key} = {value.Value.name}");
        }

        Debug.Log("Material to ID");
        foreach (KeyValuePair<Material, int> value in _materialToId) {
            Debug.Log($"{value.Key.name} = {value.Value}");
        }
    }

    static public Material GetMaterialFromId(int Id) {
        return _IdToMaterial[Id];
    }

    static public int GetIdFromMaterial(Material material) {
        return _materialToId[material];
    }
}
