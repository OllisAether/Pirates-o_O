using UnityEditor;
using UnityEngine;

namespace ObjectiveSystem
{
  #if UNITY_EDITOR
  [CustomEditor(typeof(ObjectiveViewHandler))]
  public class ObjectiveViewHandlerEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      if (GUILayout.Button("Play Complete Animation"))
      {
        var handler = (ObjectiveViewHandler)target;
        handler.PlayObjectiveCompleteAnimation("Test Objective", "Test Description");
      }
    }
  }
  #endif
}