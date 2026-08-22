using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
  public class DialogPlayer : MonoBehaviour
  {
    [SerializeField] private UnityEvent onDialogStarted;
    [SerializeField] private UnityEvent onDialogEnded;
    
    public UnityEvent OnDialogStarted => onDialogStarted;
    public UnityEvent OnDialogEnded => onDialogEnded;

    public void StartDialog(DialogTree dialogTree)
    {
      DialogManager.Instance.StartDialog(dialogTree, () => {
        onDialogEnded?.Invoke();
      });
      onDialogStarted?.Invoke();
    }
  }
}