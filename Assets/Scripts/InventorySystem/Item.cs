using UnityEngine;

namespace InventorySystem
{
  public enum HoldItemType
  {
    Default,
    Lantern,
    Plushy
  }

  [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
  public class Item : ScriptableObject
  {
    [SerializeField]
    private string id;
    [SerializeField]
    private string displayName;

    [SerializeField]
    private bool isStackable = false;
    [SerializeField]
    private bool useCustomStackLimit = false;
    [SerializeField]
    private int customStackLimit;

    [SerializeField]
    private GameObject dropPrefab;
    [SerializeField]
    private GameObject inventoryDisplayPrefab;
    [SerializeField]
    private bool holdable = false;
    [SerializeField]
    private HoldItemType holdItemType = HoldItemType.Default;
    [SerializeField]
    private GameObject holdPrefab;

    public string Id { get => id; }
    public string DisplayName { get => displayName; }
    public bool IsStackable { get => isStackable; }
    public bool UseCustomStackLimit { get => useCustomStackLimit; }
    public int CustomStackLimit { get => customStackLimit; }
    public GameObject DropPrefab { get => dropPrefab; }
    public GameObject InventoryDisplayPrefab { get => inventoryDisplayPrefab; }
    public bool Holdable { get => holdable; }
    public HoldItemType HoldItemType { get => holdItemType; }
    public GameObject HoldPrefab { get => holdPrefab; }
  }
}