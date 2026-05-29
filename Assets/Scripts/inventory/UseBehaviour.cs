using UnityEngine;

namespace Inventory
{
  public class UseBehaviour : MonoBehaviour
  {
    public Transform PlayerTransform { get; private set; }
    public Transform PlayerCameraRoot { get; private set; }

    public void Use()
    {
      BroadcastMessage("OnUse", SendMessageOptions.DontRequireReceiver);
    }

    internal void SetPlayerTransform(Transform playerTransform)
    {
      PlayerTransform = playerTransform;
    }

    internal void SetPlayerCameraRoot(Transform cameraRoot)
    {
      PlayerCameraRoot = cameraRoot;
    }
  }
}