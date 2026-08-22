using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
  public class OnPlayerEnter : MonoBehaviour
  {
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private UnityEvent onPlayerEnter;
    public UnityEvent OnPlayerEnterEvent => onPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag(playerTag))
      {
        onPlayerEnter?.Invoke();
      }
    }
  }
}