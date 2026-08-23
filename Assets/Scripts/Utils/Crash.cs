using UnityEngine;

namespace Utils
{
  public class Crash : MonoBehaviour
  {
    public void TriggerCrash()
    {
      #if UNITY_EDITOR
        UnityEditor.EditorApplication.Exit(0);
      #else
        Application.Quit();
      #endif
    }
  }
}