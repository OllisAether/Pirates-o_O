using UnityEditor;
using UnityEngine;

namespace Inventory
{
  #if UNITY_EDITOR
  [CustomEditor(typeof(UseBehaviour))]
  public class UseBehaviourEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      EditorGUILayout.HelpBox("This component is used to mark an Item as usable. It will broadcast an OnUse message if the item is used.", MessageType.Info);
    }
  }
  #endif
}