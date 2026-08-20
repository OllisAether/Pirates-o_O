

using UnityEngine;

public class DayNightController : MonoBehaviour
{
  [SerializeField] private GameObject[] activeDayObjects;
  [SerializeField] private GameObject[] activeNightObjects;

  [SerializeField] private bool isDayTime = true;

  private void Start()
  {
    UpdateActiveObjects();
  }

  public void ToggleDayNight()
  {
    isDayTime = !isDayTime;
    UpdateActiveObjects();
  }

  public void SetDayTime(bool isDay)
  {
    isDayTime = isDay;
    UpdateActiveObjects();
  }

  private void UpdateActiveObjects()
  {
    foreach (var obj in activeDayObjects)
    {
      if (obj == null) continue;
      obj.SetActive(isDayTime);
    }

    foreach (var obj in activeNightObjects)
    {
      if (obj == null) continue;
      obj.SetActive(!isDayTime);
    }
  }

  private void OnValidate()
  {
    UpdateActiveObjects();
  }
}