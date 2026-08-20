using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem
{
  [RequireComponent(typeof(UIDocument))]
  public class DialogViewHandler : MonoBehaviour
  {
    [SerializeField] private UIDocument dialogDocument;
    private DialogView dialogView;
    public DialogView DialogView => dialogView;

    [SerializeField] private MeepGenerator meepGenerator;

    [SerializeField] private float typewriterDelay = .1f;
    [SerializeField] private CharacterOverrideDelay[] characterOverrideDelays;
    private Dictionary<char, float> characterDelayLookup;

    [SerializeField] private DialogTree testTree;
    [SerializeField] private int testSegmentIndex = 0;

    public bool TypewriterIsPlaying { get; private set; }
    private DialogSegment currentSegment;
    private List<string> currentOptions;
    private System.Action<int> currentOnOptionChosen;

    private void Awake()
    {
      dialogView = dialogDocument.rootVisualElement.Q<DialogView>();
      DialogView.ClearDialog();
      DialogView.Hide();
    }

    private void Start()
    {
      CreateCharacterDelayLookup();
    }

    private void OnValidate()
    {
      if (dialogDocument != null)
      {
        dialogView = dialogDocument.rootVisualElement.Q<DialogView>();
      }
    }

    private void CreateCharacterDelayLookup()
    {
      characterDelayLookup = new Dictionary<char, float>();
      foreach (var overrideDelay in characterOverrideDelays)
      {
        characterDelayLookup[overrideDelay.Character] = overrideDelay.Delay;
      }
    }

    private IEnumerator TypewriterCoroutine(DialogSegment segment)
    {
      foreach (var typewriterSegment in segment.TypewriterSegments)
      {
        {
          foreach (var c in typewriterSegment.Text)
          {
            DialogView.SetDialog(DialogView.Dialog + c);

            if (meepGenerator != null)
            {
              float pitch = segment.Speaker.Pitch;
              if (segment.SpeakerOverride.OverridePitch)
              {
                pitch = segment.SpeakerOverride.Pitch;
              }
              if (typewriterSegment.TimingOverrides.OverridePitch)
              {
                pitch = typewriterSegment.TimingOverrides.PitchOverride;
              }
              meepGenerator.PlayMeep(DialogView.Dialog, c, pitch);
            }

            var delay = GetDelayForCharacter(c, typewriterSegment);
            if (delay < 0) continue;

            yield return new WaitForSeconds(delay);
          }
        }

        yield return new WaitForSeconds(typewriterSegment.TimingOverrides.PostDelay);
      }
    }

    private float GetDelayForCharacter(char character, TypewriterSegment segment)
    {
      if (segment.TimingOverrides.CharacterOverrides != null)
      {
        foreach (var characterOverride in segment.TimingOverrides.CharacterOverrides)
        {
          if (characterOverride.Character == character)
          {
            return characterOverride.Delay;
          }
        }
      }
      
      if (characterDelayLookup == null) CreateCharacterDelayLookup();
      if (characterDelayLookup.TryGetValue(character, out float globalOverrideDelay))
      {
        return globalOverrideDelay;
      }

      if (segment.TimingOverrides.OverrideDelay)
      {
        return segment.TimingOverrides.DelayOverride;
      }

      return typewriterDelay;
    }
  
    public void TestTypewriter()
    {
      CreateCharacterDelayLookup();

      var segment = testTree.DialogSegments[testSegmentIndex];

      if (segment == null)
      {
        Debug.LogError("Test segment is null. Check your test tree and index.");
        return;
      }

      if (testSegmentIndex == testTree.DialogSegments.Length - 1)
      { 
        var options = new List<string>();

        foreach (var response in testTree.Responses)
        {
          options.Add(response.ResponseText);
        }

        PlayDialogSegment(segment, null, options);
      }
      else
      {
        PlayDialogSegment(segment);
      }

    }

    public void Show()
    {
      DialogView.Show();
    }

    public void Hide()
    {
      DialogView.Hide();
    }
    
    public void PlayDialogSegment(DialogSegment segment, System.Action onDialogEnded = null, List<string> options = null, System.Action<int> onOptionChosen = null)
    {
      StopAllCoroutines();

      currentSegment = segment;
      currentOptions = options;
      currentOnOptionChosen = onOptionChosen;
      TypewriterIsPlaying = true;

      DialogView.ClearDialog();
      DialogView.Show();

      var speakerName = segment.Speaker.Name;
      if (segment.SpeakerOverride.OverrideName)
      {
        speakerName = segment.SpeakerOverride.Name;
      }

      DialogView.SetSpeaker(speakerName);

      StartCoroutine(PlayDialogSequenceCoroutine(segment, onDialogEnded));
    }

    private IEnumerator PlayDialogSequenceCoroutine(DialogSegment segment, System.Action onDialogEnded)
    {
      yield return TypewriterCoroutine(segment);
      OnTypewriterEnd();
      onDialogEnded?.Invoke();
    }

    public void SkipTypewriter()
    {
      StopAllCoroutines();
      DialogView.SetDialog(GetComposedText(currentSegment.TypewriterSegments));
      OnTypewriterEnd();
    }

    private void OnTypewriterEnd()
    {
      if (currentOptions != null && currentOptions.Count > 0)
      {
        var previousLockState = UnityEngine.Cursor.lockState;
        var previousVisibility = UnityEngine.Cursor.visible;

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        for (int i = 0; i < currentOptions.Count; i++)
        {
          var index = i;
          var option = currentOptions[i];

          DialogView.AddOption(option, () => {
            currentOnOptionChosen?.Invoke(index);
            currentOptions = null;
            currentOnOptionChosen = null;

            UnityEngine.Cursor.lockState = previousLockState;
            UnityEngine.Cursor.visible = previousVisibility;
          });
        }
      } else
      {
        DialogView.ShowNextIndicator(true);
      }

      TypewriterIsPlaying = false;
    }

    private string GetComposedText(TypewriterSegment[] segments)
    {
      string composed = "";
      foreach (var segment in segments)
      {
        composed += segment.Text;
      }

      return composed;
    }
  }
}