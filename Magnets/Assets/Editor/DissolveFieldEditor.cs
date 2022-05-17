using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DissolveField))]
[InitializeOnLoad]
class DissolveFieldEditor : Editor
{
    DissolveFieldEditor()
    {
        EditorApplication.update += Update;
    }
    void Update()
    {
        DissolveField field = (DissolveField)target;
        field.UpdateScale();
    }
}