using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class DialogView : VisualElement
{
  private Label DialogText => this.Q<Label>("dialog");
  private Label SpeakerText => this.Q<Label>("speaker");
  private VisualElement OptionsContainer => this.Q<VisualElement>("options");
  private VisualElement NextIndicator => this.Q<VisualElement>("next-indicator");

  public string Dialog => DialogText.text;
  public string Speaker => SpeakerText.text;

  public DialogView() {}

  public void SetDialog(string dialog)
  {
    DialogText.text = dialog;
  }

  public void SetSpeaker(string speaker)
  {
    SpeakerText.text = speaker;
  }

  public void AddOption(string optionText, System.Action onClick)
  {
    var button = new Button(onClick) { text = optionText };
    OptionsContainer.Add(button);
  }

  public void ShowNextIndicator(bool show)
  {
    if (show) NextIndicator.AddToClassList("visible");
    else NextIndicator.RemoveFromClassList("visible");
  }

  public void Show()
  {
    AddToClassList("visible");
  } 

  public void Hide()
  {
    RemoveFromClassList("visible");
  }

  public void ClearDialog()
  {
    DialogText.text = "";
    SpeakerText.text = "";
    OptionsContainer.Clear();
    ShowNextIndicator(false);
  }
}