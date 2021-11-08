using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

public class Rifle : RangedWeapon
{
    /*#region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(Rifle))]
    public class RangeWeaponEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RangedWeapon weap = (Rifle)target;
            // Serialize --
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Special");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();

            if (weap.special == Special_Type.Mortar)
            {
                weap.Sp_Name = EditorGUILayout.TextField(weap.Sp_Name);
                weap.Sp_Damage = EditorGUILayout.IntField(weap.Sp_Damage);
                weap.Sp_Range = EditorGUILayout.FloatField(weap.Sp_Range);
            }
        }
    }
#endif
    #endregion*/
}
