using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObjectiveSystem
{
  public class ObjectiveViewHandler : MonoBehaviour
  {
    [SerializeField] private UIDocument objectiveDocument;
    [SerializeField] private string newObjectiveTitle = "NEW OBJECTIVE";
    [SerializeField] private string objectiveCompleteTitle = "OBJECTIVE COMPLETE";
    [SerializeField] private AudioSource newObjectiveAudioSource;
    [SerializeField] private AudioSource objectiveCompleteAudioSource;

    private ObjectivesView ObjectivesView => objectiveDocument.rootVisualElement.Q<ObjectivesView>();

    private void Awake()
    {
      ObjectivesView.HideObjectiveBanner();
    }

    private void Start()
    {
      ObjectiveManager.Instance.OnObjectiveStarted.AddListener(HandleObjectiveStarted);
      ObjectiveManager.Instance.OnObjectiveCompleted.AddListener(HandleObjectiveCompleted);
      ObjectiveManager.Instance.OnObjectiveStepStarted.AddListener(_ => UpdateCurrentObjectives());
    }

    bool isAnimationPlaying = false;
    List<System.Action> animationsQueue = new List<System.Action>();
    public void PlayNewObjectiveAnimation(string objectiveName, string objectiveDescription)
    {
      if (isAnimationPlaying)
      {
        animationsQueue.Add(() => PlayNewObjectiveAnimation(objectiveName, objectiveDescription));
        return;
      }

      isAnimationPlaying = true;
      ObjectivesView.OpenObjectiveBanner(newObjectiveTitle, objectiveName, objectiveDescription);

      if (newObjectiveAudioSource != null)
      {
        newObjectiveAudioSource.Play();
      }

      StartCoroutine(WaitForAnimationToFinish());
    }

    private IEnumerator WaitForAnimationToFinish()
    {
      yield return new WaitForSeconds(ObjectivesView.ObjectiveBannerDuration);
      isAnimationPlaying = false;

      if (animationsQueue.Count > 0)
      {
        var nextAnimation = animationsQueue[0];
        animationsQueue.RemoveAt(0);
        nextAnimation.Invoke();
      }
    }

    public void PlayObjectiveCompleteAnimation(string objectiveName, string objectiveDescription)
    {
      if (isAnimationPlaying)
      {
        animationsQueue.Add(() => PlayObjectiveCompleteAnimation(objectiveName, objectiveDescription));
        return;
      }

      isAnimationPlaying = true;
      ObjectivesView.OpenObjectiveBanner(objectiveCompleteTitle, objectiveName, objectiveDescription);

      if (objectiveCompleteAudioSource != null)
      {
        objectiveCompleteAudioSource.Play();
      }

      StartCoroutine(WaitForAnimationToFinish());
    }

    private void HandleObjectiveStarted(Objective objective)
    {
      if (objective.HiddenObjective)
      {
        return;
      }

      PlayNewObjectiveAnimation(objective.DisplayName, objective.Description);
    }

    private void HandleObjectiveCompleted(Objective objective)
    {
      PlayObjectiveCompleteAnimation(objective.DisplayName, objective.Description);
      UpdateCurrentObjectives();
    }

    private void UpdateCurrentObjectives()
    {
      var currentObjectives = ObjectiveManager.Instance.CurrentObjectives.Values.ToList();

      for (int i = 0; i < currentObjectives.Count; i++)
      {
        var activeObjective = currentObjectives[i];
        if (activeObjective.Objective.HiddenObjective)
        {
          currentObjectives.RemoveAt(i);
          i--;
        }
      }

      ObjectivesView.SetCurrentObjectives(currentObjectives.ToArray());
    }
  }
}