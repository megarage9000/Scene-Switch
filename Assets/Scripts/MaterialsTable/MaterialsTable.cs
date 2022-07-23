using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MaterialsTable  {

    static private Dictionary<string, Material> _nameToMaterial;
    static private Dictionary<Material, string> _materialToName;

    static private string[] _folderFiter = { "Assets/Models", "Assets/Materials" };

    [RuntimeInitializeOnLoadMethod]
    static private void loadMaterials() {
        _nameToMaterial = new Dictionary<string, Material>();
        _materialToName = new Dictionary<Material, string>();

        Debug.Log("Loading materials");
        string[] guids = AssetDatabase.FindAssets("t:material", _folderFiter);

        foreach (string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            string name = material.name;

            _nameToMaterial[name] = material;
            _materialToName[material] = name;
        }

        printMaterials();
    }

    static private void printMaterials() {
        Debug.Log("ID to Material");
        foreach(KeyValuePair<string, Material> value in _nameToMaterial) {
            Debug.Log($"{value.Key} = {value.Value.name}");
        }

        Debug.Log("Material to ID");
        foreach (KeyValuePair<Material, string> value in _materialToName) {
            Debug.Log($"{value.Key.name} = {value.Value}");
        }
    }

    static public Material GetMaterialFromName(string name) {
        return _nameToMaterial[name];
    }

    static public string GetNameFromMaterial(Material material) {
        return _materialToName[material];
    }
}