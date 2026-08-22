using System.Collections.Generic;
using GameManager;
using ObjectiveSystem;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace DialogSystem
{
  public class DialogManager : SingletonBehaviour<DialogManager>
  {
    [SerializeField] private DialogController dialogController;
    [SerializeField] private DialogViewHandler dialogViewHandler;

    public DialogController DialogController => dialogController;
    public DialogViewHandler DialogViewHandler => dialogViewHandler;

    [SerializeField] private DialogTree startingDialogTree;
    private UnityEvent<string> onDialogEvent = new UnityEvent<string>();
    public UnityEvent<string> OnDialogEvent => onDialogEvent;

    private void Start()
    {
      if (startingDialogTree != null)
      {
        StartDialog(startingDialogTree);
      }

      dialogController.OnContinueEvent.AddListener(OnContinue);
    }

    public void StartDialog(DialogTree dialogTree)
    {
      StartDialog(dialogTree, null);
    }
    public void StartDialog(DialogTree dialogTree, System.Action onDialogEndCallback = null)
    {
      dialogController.OnDialogStarted(dialogTree);
      currentDialogEndCallback = onDialogEndCallback;
      PlayDialogTree(dialogTree);
    }

    private DialogTree currentDialogTree;
    private int currentSegmentIndex = 0;
    private System.Action currentDialogEndCallback;
    private void PlayDialogTree(DialogTree dialogTree)
    {
      currentDialogTree = dialogTree;
      currentSegmentIndex = 0;
      
      if (dialogTree.ShowBlackBars)
      {
        BlackBarsManager.Instance.Show();
      }

      PlayNextDialogSegment();
    }

    private void PlayNextDialogSegment()
    {
      if (currentDialogTree == null || currentSegmentIndex >= currentDialogTree.DialogSegments.Length)
      {
        EndDialog();
        return;
      }

      var segment = currentDialogTree.DialogSegments[currentSegmentIndex];
      currentSegmentIndex++;

      for (int i = 0; i < segment.DialogEvents.Length; i++)
      {
        Debug.Log($"Invoking dialog event: {segment.DialogEvents[i]}");
        onDialogEvent?.Invoke(segment.DialogEvents[i]);
      }
      
      if (currentSegmentIndex >= currentDialogTree.DialogSegments.Length && currentDialogTree.Responses != null && currentDialogTree.Responses.Length > 0)
      {
        var responses = currentDialogTree.Responses;
        var options = new List<string>();

        foreach (var response in responses)
        {
          options.Add(response.ResponseText);
        }

        dialogViewHandler.PlayDialogSegment(segment, null, options, (chosenResponseIndex) =>
        {
          var chosenResponse = responses[chosenResponseIndex];

          if (chosenResponse.NextDialogTree != null)
          {
            if (!chosenResponse.NextDialogTree.ShowBlackBars)
            {
              BlackBarsManager.Instance.Hide();
            }

            PlayDialogTree(chosenResponse.NextDialogTree);
          }
          else
          {
            EndDialog();
          }
        });
      } else
      {
        dialogViewHandler.PlayDialogSegment(segment, () => {
          if (segment.AutoAdvance)
          {
            PlayNextDialogSegment();
          }
        });
      }
    }

    private void EndDialog()
    {
      dialogViewHandler.Hide();
      BlackBarsManager.Instance.Hide();
      dialogController.OnDialogEnded();
      currentDialogEndCallback?.Invoke();

      if (currentDialogTree?.ObjectiveToStart != null)
      {
        ObjectiveManager.Instance.StartObjective(currentDialogTree.ObjectiveToStart);
      }
      
      currentSegmentIndex = 0;
      currentDialogTree = null;
      currentDialogEndCallback = null;
    }

    private void OnContinue()
    {
      if (currentDialogTree == null) return;
      var currentSegment = currentDialogTree.DialogSegments[currentSegmentIndex - 1];
      if (currentSegment == null) return;

      if (currentSegment.AutoAdvance)
      {
        return;
      }

      if (dialogViewHandler.TypewriterIsPlaying)
      {
        dialogViewHandler.SkipTypewriter();
      }
      else if (currentSegmentIndex < currentDialogTree.DialogSegments.Length || currentDialogTree.Responses == null || currentDialogTree.Responses.Length <= 0)
      {
        PlayNextDialogSegment();
      }
    }
  }
}