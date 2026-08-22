using UnityEngine;
using System.Runtime.InteropServices;


namespace Utils
{
  public class Crash : MonoBehaviour
  {
    [SerializeField] private string crashTitle = "Crash";
    [SerializeField] private string crashMessage = "Crashed";

    public void TriggerCrash()
    {
      SystemDialog.Show(crashTitle, crashMessage);

      #if UNITY_EDITOR
        UnityEditor.EditorApplication.Exit(0);
      #endif
      Application.Quit();
    }
  }

  public static class SystemDialog
  {
      public static void Show(string title, string message)
      {
  #if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
          // Unity Editor on macOS or Windows
          UnityEditor.EditorUtility.DisplayDialog(title, message, "OK");

  #elif UNITY_STANDALONE_OSX
          // macOS standalone build
          ShowMacDialog(title, message);

  #elif UNITY_STANDALONE_WIN
          // Windows standalone build
          System.Windows.Forms.MessageBox.Show(message, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
  #else
          // Other Unity platforms
          Debug.Log($"{title}: {message}");
  #endif
      }

  #if UNITY_STANDALONE_OSX && !UNITY_EDITOR
      [DllImport("__Internal")]
      private static extern void ShowMacDialog(string title, string message);
  #endif
  }
}