using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
  public class SequentialTrigger : MonoBehaviour
  {
    [SerializeField] private UnityEvent[] triggerEvents;
    [SerializeField] private UnityEvent triggeredWithAllEventsCompleted;

    private int currentIndex = 0;

    public void TriggerNext()
    {
      if (currentIndex < triggerEvents.Length)
      {
        triggerEvents[currentIndex]?.Invoke();
        currentIndex++;
      }
      else
      {
        triggeredWithAllEventsCompleted?.Invoke();
      }
    }
  }
}