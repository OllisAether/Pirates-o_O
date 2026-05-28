using Inventory;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class InventoryUi : MonoBehaviour
{
  [SerializeField]
  private InventoryBehaviour inventory;

  [SerializeField]
  private int activeItemIndex = 0;
  public int ActiveItemIndex { get { return activeItemIndex; } }

  [SerializeField]
  private bool inventoryOpen = false;

  [SerializeField]
  private InputActionReference toggleInventoryAction;
  [SerializeField]
  private InputActionAsset playerActionsToDisableWhenInventoryOpen;
  [SerializeField]
  private string[] actionMapsToDisableWhenInventoryOpen;

  [Space(10)]
  [SerializeField]
  private UnityEvent<int> onActiveItemChanged = new UnityEvent<int>();
  public UnityEvent<int> OnActiveItemChanged { get { return onActiveItemChanged; } }

  [SerializeField]
  private UnityEvent<bool> onInventoryToggled = new UnityEvent<bool>();
  public UnityEvent<bool> OnInventoryToggled { get { return onInventoryToggled; } }

  [SerializeField]
  private VisualTreeAsset itemButtonTemplate;

  private UIDocument uiDocument;
  private VisualElement inventoryRoot;
  private VisualElement itemButtonContainer;
  private Label currentItemLabel;

  void Start()
  {
    uiDocument = GetComponent<UIDocument>();

    if (toggleInventoryAction != null)
    {
      toggleInventoryAction.action.Enable();
      toggleInventoryAction.action.performed += ctx => SetInventoryOpen(!inventoryOpen);
    }

    inventoryRoot = uiDocument.rootVisualElement.Q<VisualElement>("Inventory");
    itemButtonContainer = inventoryRoot.Q<VisualElement>("ButtonContainer");
    currentItemLabel = uiDocument.rootVisualElement.Q<Label>("CurrentItemLabel");

    inventory.OnInventoryChanged.AddListener(UpdateInventoryDisplay);
    inventory.OnCurrentItemChanged.AddListener(SetCurrentItemLabel);

    SetInventoryOpen(inventoryOpen);
    UpdateInventoryDisplay(inventory.InventoryItems);
    SetCurrentItemLabel(inventory.CurrentItem);
  }

  private void SetCurrentItemLabel(InventoryItem item)
  {
    if (item != null)
    {
      currentItemLabel.text = item.DisplayName;
      currentItemLabel.style.display = DisplayStyle.Flex;
    }
    else
    {
      currentItemLabel.style.display = DisplayStyle.None;
    }
  }

  public void SetInventoryOpen(bool open)
  {
    inventoryOpen = open;
    inventoryRoot.style.display = inventoryOpen ? DisplayStyle.Flex : DisplayStyle.None;

    if (playerActionsToDisableWhenInventoryOpen != null)
    {
      foreach (var mapName in actionMapsToDisableWhenInventoryOpen)
      {
        var map = playerActionsToDisableWhenInventoryOpen.FindActionMap(mapName);
        if (map != null)
        {
          if (inventoryOpen)
            map.Disable();
          else
            map.Enable();
        }
      }
    }

    UnityEngine.Cursor.lockState = inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;

    onInventoryToggled.Invoke(inventoryOpen);
  }

  public void SetActiveItemIndex(int index)
  {
    activeItemIndex = index;
    onActiveItemChanged.Invoke(activeItemIndex);
    UpdateActiveItemButton(activeItemIndex);
  }

  private void UpdateActiveItemButton(int index)
  {
    var buttons = itemButtonContainer.Query<Button>().ToList();

    Debug.Log("Updating active item button. Total buttons: " + buttons.Count + ", Active index: " + index);

    for (int i = 0; i < buttons.Count; i++)
    {
      var button = buttons[i];
      if (i == index)
      {
        button.AddToClassList("active");
      }
      else
      {
        button.RemoveFromClassList("active");
      }
    }
  }

  private void UpdateInventoryDisplay(InventoryItem[] items)
  {
    itemButtonContainer.Clear();

    for (int i = 0; i < items.Length; i++)
    {
      var item = items[i];
      var buttonAsset = itemButtonTemplate.Instantiate();
      buttonAsset.name = "ItemButton" + i;
      var button = buttonAsset.Q<Button>();
      button.text = item != null ? item.DisplayName : "Empty";
      int index = i;
      button.clicked += () => SetActiveItemIndex(index);
      itemButtonContainer.Add(buttonAsset);
    }

    UpdateActiveItemButton(activeItemIndex);
  }
}
