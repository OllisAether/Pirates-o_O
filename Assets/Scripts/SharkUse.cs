using Inventory;
using UnityEngine;

[RequireComponent(typeof(UseBehaviour))]
public class SharkUse : MonoBehaviour
{
  [SerializeField]
  private GameObject sharkObject;
  [SerializeField]
  private float pushForce = 50f;

  private UseBehaviour u;

  [SerializeField]
  private float forwardOffset = 1f;

  public void Start()
  {
    u = GetComponent<UseBehaviour>();
  }

  public void OnUse()
  {
    Debug.Log("Shark used!");

    if (sharkObject != null)
    {
      var shark = Instantiate(sharkObject, u.PlayerCameraRoot.position + u.PlayerCameraRoot.forward * forwardOffset, Quaternion.identity);
      var rb = shark.GetComponent<Rigidbody>();
      if (rb != null)
      {
        rb.AddForce(u.PlayerCameraRoot.forward * pushForce, ForceMode.Impulse);
      }
    }
  }
}
