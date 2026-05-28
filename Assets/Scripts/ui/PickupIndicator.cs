using Inventory;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PickupIndicator : MonoBehaviour
{
  [SerializeField]
  private PickupBehaviour pickupBehaviour;

  [SerializeField]
  private string pickupPrefix = "Pick up ";

  private UIDocument uiDocument;
  private VisualElement rootElement;
  private Label itemNameLabel;

  void Start()
  {
    uiDocument = GetComponent<UIDocument>();
    rootElement = uiDocument.rootVisualElement;
    itemNameLabel = rootElement.Q<Label>("Label");

    rootElement.style.display = DisplayStyle.None;
  }

  private void OnEnable()
  {
    pickupBehaviour.OnCollectableTargeted.AddListener(UpdateIndicator);
  }

  private void OnDisable()
  {
    pickupBehaviour.OnCollectableTargeted.RemoveListener(UpdateIndicator);
  }

  private void UpdateIndicator(InventoryItem targetedItem)
  {
    if (targetedItem != null)
    {
      itemNameLabel.text = pickupPrefix + targetedItem.DisplayName;
      rootElement.style.display = DisplayStyle.Flex;
    }
    else
    {
      rootElement.style.display = DisplayStyle.None;
    }
  }
}
