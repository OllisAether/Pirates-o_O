using UnityEngine;
using UnityEditor;

namespace GameManager
{
  [CustomEditor(typeof(BlackBarsManager))]
  public class BlackBarsManagerEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      EditorGUILayout.PropertyField(serializedObject.FindProperty("blackBarsDocument"));

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Testing", EditorStyles.boldLabel);

      if (GUILayout.Button("Toggle"))
      {
        ((BlackBarsManager)target).Toggle();
      }
      
      serializedObject.ApplyModifiedProperties();
    }
  }
}