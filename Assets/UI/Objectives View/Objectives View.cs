using DG.Tweening;
using ObjectiveSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ObjectivesView : VisualElement
{
  private Label ObjectiveTitle => this.Q<Label>("objective-banner-title");
  private Label ObjectiveContent => this.Q<Label>("objective-banner-content");
  private Label ObjectiveDescription => this.Q<Label>("objective-banner-desc");
  private VisualElement ObjectiveBanner => this.Q<VisualElement>("objective-banner");

  private VisualElement CurrentObjectiveListContainer => this.Q<VisualElement>("current-objective-list");

  private float objectiveBannerDuration = 4f;
  public float ObjectiveBannerDuration => objectiveBannerDuration;

  private VisualTreeAsset currentObjectivesContainerTemplate => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/Objectives View/CurrentObjectiveContainer.uxml");

  public ObjectivesView() {}

  public void OpenObjectiveBanner(string title, string content, string description)
  {
    ObjectiveTitle.text = title;
    ObjectiveContent.text = content;
    ObjectiveDescription.text = description;

    if (description == null || description.Trim() == "")
    {
      ObjectiveDescription.style.display = DisplayStyle.None;
    }
    else
    {
      ObjectiveDescription.style.display = DisplayStyle.Flex;
    }

    ObjectiveBanner.style.display = DisplayStyle.Flex;
    ObjectiveBanner.style.opacity = 0f;

    DOTween.Sequence()
      .Insert(0f, DOTween.To(() => ObjectiveBanner.style.opacity.value, x => ObjectiveBanner.style.opacity = x, 1f, 0.5f))
      .Insert(objectiveBannerDuration - 0.5f, DOTween.To(() => ObjectiveBanner.style.opacity.value, x => ObjectiveBanner.style.opacity = x, 0f, 0.5f))
      .OnComplete(() =>
      {
        ObjectiveBanner.style.display = DisplayStyle.None;
      });
  }

  public void HideObjectiveBanner()
  {
    ObjectiveBanner.style.display = DisplayStyle.None;
  }

  public void SetCurrentObjectives(ActiveObjective[] objectives)
  {
    CurrentObjectiveListContainer.Clear();

    foreach (var objective in objectives)
    {
      var objectiveElement = currentObjectivesContainerTemplate.CloneTree();
      var objectiveNameLabel = objectiveElement.Q<Label>("current-objective-title");
      var objectiveDescriptionLabel = objectiveElement.Q<Label>("current-objective-desc");
      var objectiveStepLabel = objectiveElement.Q<Label>("current-objective-step");

      objectiveNameLabel.text = objective.Objective.DisplayName;
      objectiveDescriptionLabel.text = objective.Objective.Description;
      objectiveStepLabel.text = objective.CurrentStepInfo.StepName;

      CurrentObjectiveListContainer.Add(objectiveElement);
    }
  }
}