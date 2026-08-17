using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace GameManager
{
  public class BlackBarsManager : SingletonBehaviour<BlackBarsManager>
  {
    [SerializeField] private UIDocument blackBarsDocument;

    private BlackBars BlackBars => blackBarsDocument.rootVisualElement.Q<BlackBars>();
    private bool Showing => BlackBars.Showing;

    public void Show()
    {
      BlackBars.Show();
    }

    public void Hide()
    {
      BlackBars.Hide();
    }

    public void Toggle()
    {
      if (Showing) Hide();
      else Show();
    }
  }
}
