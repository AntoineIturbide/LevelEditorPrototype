using UnityEngine;
using System.Collections;
using UnityEditor;

/*

//[CustomPropertyDrawer(typeof(Tileset))]
public class TilesetEditor : PropertyDrawer {

    bool _imported = false;

    Texture2D _texture;

    Tileset _tileset {
        get { return (Tileset)attribute; }
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        
        //Tileset tileset = (Tileset)property.;

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        Rect textureRect = new Rect(position.x, position.y, position.width/2 - 2.5f, position.height);
        //var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        Rect resolutionLabelRect = new Rect(position.x + position.width/2 +2.5f, position.y, position.width / 2 - 77.5f, position.height);
        Rect resolutionRect = new Rect(position.x + position.width - 75, position.y, 75, position.height);



        EditorGUI.ObjectField(textureRect, property.FindPropertyRelative("_texture"), GUIContent.none);
        //EditorGUI.IntField(resolutionRect, property.FindPropertyRelative("_tileResolution"))
        //EditorGUI.PropertyField(resolutionRect, property.FindPropertyRelative("_tileResolution"), GUIContent.none);
        EditorGUI.LabelField(resolutionLabelRect, "Tile Resolution :");
        property.FindPropertyRelative("_tileResolution").intValue =
            EditorGUI.IntField(resolutionRect, property.FindPropertyRelative("_tileResolution").intValue);
        
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
        
        if (GUI.changed) {
            Debug.Log(_tileset);
        }
    }
}

    */