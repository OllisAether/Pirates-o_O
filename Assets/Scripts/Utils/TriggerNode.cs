using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Utils
{
  public class TriggerNode : MonoBehaviour
  {
    [SerializeField] private UnityEvent onTrigger;
    public UnityEvent OnTrigger => onTrigger;

    public void Trigger()
    {
      onTrigger?.Invoke();
    }
  }
}