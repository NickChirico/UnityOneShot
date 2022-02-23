using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableDictionary : MonoBehaviour, ISerializationCallbackReceiver
{
    public List<int> _keys = new List<int>(10);
    public List<Weapon> _weapons = new List<Weapon>(10);

    //Unity doesn't know how to serialize a Dictionary
    public Dictionary<int, Weapon>  _myDictionary = new Dictionary<int, Weapon>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _weapons.Clear();

        foreach (var kvp in _myDictionary)
        {
            _keys.Add(kvp.Key);
            _weapons.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        _myDictionary = new Dictionary<int, Weapon>();

        for (int i = 0; i != Math.Min(_keys.Count, _weapons.Count); i++)
            _myDictionary.Add(_keys[i], _weapons[i]);
    }

    void OnGUI()
    {
        foreach (var kvp in _myDictionary)
            GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
    }
}
