#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DialogSystem
{
  [CustomEditor(typeof(DialogViewHandler))]
  public class DialogViewHandlerEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogDocument"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("meepGenerator"));

      // #region Defaults
      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Default Values", EditorStyles.boldLabel);

      EditorGUILayout.PropertyField(serializedObject.FindProperty("typewriterDelay"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("characterOverrideDelays"));
      // #endregion

      // #region Testing
      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Testing", EditorStyles.boldLabel);

      var handler = (DialogViewHandler)target;
      EditorGUILayout.PropertyField(serializedObject.FindProperty("testTree"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("testSegmentIndex"));
      if (GUILayout.Button("Test Typewriter"))
      {
        handler.TestTypewriter();
      }
      if (GUILayout.Button("Skip Typewriter"))
      {
        handler.SkipTypewriter();
      }
      EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button("Show"))
      {
        handler.Show();
      }
      if (GUILayout.Button("Hide"))
      {
        handler.Hide();
      }
      EditorGUILayout.EndHorizontal();
      // #endregion

      serializedObject.ApplyModifiedProperties();
    }
  }
}
#endif