using System.Collections.Generic;
using GameManager;
using UnityEngine;
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
      dialogController.OnDialogStarted(dialogTree);
      PlayDialogTree(dialogTree);
    }

    private DialogTree currentDialogTree;
    private int currentSegmentIndex = 0;
    public void PlayDialogTree(DialogTree dialogTree)
    {
      currentDialogTree = dialogTree;
      
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
      
      if (currentSegmentIndex >= currentDialogTree.DialogSegments.Length && currentDialogTree.Responses != null && currentDialogTree.Responses.Length > 0)
      {
        var responses = currentDialogTree.Responses;
        var options = new List<string>();

        foreach (var response in responses)
        {
          options.Add(response.ResponseText);
        }

        dialogViewHandler.PlayDialogSegment(segment, options, (chosenResponseIndex) =>
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
        dialogViewHandler.PlayDialogSegment(segment);
      }
    }

    public void EndDialog()
    {
      currentDialogTree = null;
      currentSegmentIndex = 0;
      dialogViewHandler.Hide();
      BlackBarsManager.Instance.Hide();
      dialogController.OnDialogEnded();
    }

    private void OnContinue()
    {
      if (currentDialogTree == null) return;

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