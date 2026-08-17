using System;
using UnityEngine;

namespace DialogSystem
{
  [Serializable]
  public class DialogResponse
  {
    [SerializeField]
    private string responseText;
    public string ResponseText => responseText;
    [SerializeField]
    private DialogTree nextDialogTree;
    public DialogTree NextDialogTree => nextDialogTree;
  }
}