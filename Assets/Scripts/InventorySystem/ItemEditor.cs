#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace InventorySystem
{
  [CustomEditor(typeof(Item))]
  public class ItemEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      EditorGUILayout.LabelField("Item Properties", EditorStyles.boldLabel);

      EditorGUILayout.PropertyField(serializedObject.FindProperty("id"), new GUIContent("ID"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("displayName"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("isStackable"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("useCustomStackLimit"));
      if (serializedObject.FindProperty("useCustomStackLimit").boolValue)
      {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("customStackLimit"));
      }

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Item Prefabs", EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("dropPrefab"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("inventoryDisplayPrefab"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("holdable"));
      if (serializedObject.FindProperty("holdable").boolValue)
      {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("holdItemType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("holdPrefab"));
      }

      Item item = (Item)target;

      EditorGUILayout.Space();
      if (item.DropPrefab == null)
      {
        EditorGUILayout.HelpBox("Drop Prefab is not assigned. This item cannot be dropped in the world.", MessageType.Warning);
      }
      if (item.InventoryDisplayPrefab == null)
      {
        EditorGUILayout.HelpBox("Inventory Display Prefab is not assigned. This item will not have a visual representation in the inventory.", MessageType.Warning);
      }
      if (item.HoldPrefab == null && item.Holdable)
      {
        EditorGUILayout.HelpBox("Hold Prefab is not assigned. This item cannot be held by the player.", MessageType.Warning);
      }
      if (item.IsStackable && item.UseCustomStackLimit && item.CustomStackLimit <= 0)
      {
        EditorGUILayout.HelpBox("Custom Stack Limit must be greater than 0 if using custom stack limit.", MessageType.Warning);
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
}
#endif