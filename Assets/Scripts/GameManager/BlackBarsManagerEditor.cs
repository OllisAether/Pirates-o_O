using UnityEngine;
using UnityEditor;

namespace GameManager
{
  [CustomEditor(typeof(BlackBarsManager))]
  public class BlackBarsManagerEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      if (GUILayout.Button("Toggle"))
      {
        ((BlackBarsManager)target).Toggle();
      }
    }
  }
}