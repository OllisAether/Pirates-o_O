namespace DialogSystem
{
  public class OnDialogEvent : UnityEngine.MonoBehaviour
  {
    [UnityEngine.SerializeField] private string eventName;
    [UnityEngine.SerializeField] private UnityEngine.Events.UnityEvent onEvent;
    public string EventName => eventName;
    public UnityEngine.Events.UnityEvent OnEvent => onEvent;

    public void Start()
    {
      DialogManager.Instance.OnDialogEvent.AddListener(HandleDialogEvent);
    }

    private void HandleDialogEvent(string dialogEventName)
    {
      if (dialogEventName == eventName)
      {
        onEvent?.Invoke();
      }
    }
  }
}