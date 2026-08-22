using GameManager;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
  public class ShootItem : MonoBehaviour
  {
    [SerializeField] private Pool pool;
    [SerializeField] private float offsetDistance = 1.5f;
    [SerializeField] private float shootForce = 10f;
    [SerializeField] private float lifetime = 5f;

    public void Shoot()
    {
      Debug.Log($"Shooting item from pool: {pool.name}");

      var transform = PlayerManager.Instance.CurrentPlayerCameraRoot.transform;
      var forward = transform.forward;

      var item = pool.Instantiate();
      item.transform.position = transform.position + forward * offsetDistance;
      item.transform.rotation = transform.rotation;

      var rigidbody = item.GetComponent<Rigidbody>();
      if (rigidbody != null)
      {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        rigidbody.AddForce(forward * shootForce, ForceMode.Impulse);
      }

      StartCoroutine(ReleaseAfterLifetime(item));
    }

    private System.Collections.IEnumerator ReleaseAfterLifetime(GameObject item)
    {
      yield return new WaitForSeconds(lifetime);
      pool.Release(item);
    }
  }
}