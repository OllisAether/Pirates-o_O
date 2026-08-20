using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectiveSystem
{
  public class TriggeredStep : ObjectiveStep
  {
    [SerializeField]
    private string triggerTag;
    public override void OnStepStart()
    {
      ObjectiveManager.Instance.OnTriggerEvent.AddListener(OnTriggerEvent);
    }

    public override void OnStepComplete()
    {
      ObjectiveManager.Instance.OnTriggerEvent.RemoveListener(OnTriggerEvent);
    }

    private void OnTriggerEvent(string eventName)
    {
      if (eventName == triggerTag)
      {
        CompleteStep();
      }
    }
  }
}