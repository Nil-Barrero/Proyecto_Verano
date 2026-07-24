using UnityEditor;
using UnityEngine;

/// <summary>
/// Hace que cada elemento de una lista de Zone se muestre en el Inspector
/// como "Zone 1", etc. en vez del "Element 1".
/// </summary>
[CustomPropertyDrawer(typeof(Zone))]
public class ZonesDraw : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // El propertyPath de un elemento de lista tiene esta forma:
        // "_waves.Array.data[2]" — de ahí sacamos el número entre corchetes.
        string path = property.propertyPath;
        int start = path.LastIndexOf('[') + 1;
        int length = path.LastIndexOf(']') - start;
        string indexStr = path.Substring(start, length);
        int index = int.Parse(indexStr) + 1;

        label.text = $"Zone {index}";

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}