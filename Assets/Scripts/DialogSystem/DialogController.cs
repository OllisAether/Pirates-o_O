using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utils;

namespace DialogSystem
{
  [RequireComponent(typeof(PlayerInput))]
  public class DialogController : MonoBehaviour
  {
    private PlayerInput playerInput;
    [SerializeField] private UnityEvent onContinueEvent = new UnityEvent();
    public UnityEvent OnContinueEvent => onContinueEvent;

    [SerializeField] private string dialogActionMapName = "Dialog";

    private void Awake()
    {
      playerInput = GetComponent<PlayerInput>();
    }

    private string initalActionMap;
    public void OnDialogStarted(DialogTree dialogTree)
    {
      if (playerInput.currentActionMap.name == dialogActionMapName) return;

      initalActionMap = playerInput.currentActionMap.name;
      playerInput.SwitchCurrentActionMap(dialogActionMapName);
      Debug.Log($"Switched to {dialogActionMapName} action map for dialog");
    }

    public void OnDialogEnded()
    {
      playerInput.SwitchCurrentActionMap(initalActionMap);
      Debug.Log($"Switched back to {initalActionMap} action map after dialog");
    }

    public void OnContinue(InputValue value)
    {
      Debug.Log("Continue input received");
      if (value.isPressed)
      {
        onContinueEvent?.Invoke();
      }
    }
  }
}