#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utils
{
  [CustomEditor(typeof(TriggerNode))]
  public class TriggerNodeEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      if (GUILayout.Button("Trigger Debug"))
      {
        ((TriggerNode)target).Trigger();
      }
    }
  }
}
#endif