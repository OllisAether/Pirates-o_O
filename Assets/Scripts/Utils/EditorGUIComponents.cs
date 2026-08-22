#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utils
{
  public static class EditorGUIComponents
  {
    public static void MinMaxSlider(ref float min, ref float max, float minLimit, float maxLimit)
    {
      EditorGUILayout.BeginHorizontal();
      min = EditorGUILayout.FloatField(min, GUILayout.MaxWidth(50));
      EditorGUILayout.Space(8);
      EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
      EditorGUILayout.Space(8);
      max = EditorGUILayout.FloatField(max, GUILayout.MaxWidth(50));
      EditorGUILayout.EndHorizontal();
    }
  }
}
#endif