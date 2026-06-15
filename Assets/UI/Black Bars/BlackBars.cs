using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BlackBars : VisualElement
{
  private VisualElement topBar => this.Q<VisualElement>("top");
  private VisualElement bottomBar => this.Q<VisualElement>("bottom");
  private const string activeClass = "bar--active";

  private bool showing = false;
  public bool Showing => showing;

  public BlackBars() {}

  public void Show()
  {
    topBar.AddToClassList(activeClass);
    bottomBar.AddToClassList(activeClass);
    showing = true;
  }

  public void Hide()
  {
    topBar.RemoveFromClassList(activeClass);
    bottomBar.RemoveFromClassList(activeClass);
    showing = false;
  }
}