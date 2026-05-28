using UnityEngine;

namespace Persistence
{
  public class PersistentObject : MonoBehaviour
  {
    [SerializeField]
    private bool useCustomID = false;
    [SerializeField]
    private string customID;

    public string ID
    {
      get
      {
        if (useCustomID) { return customID; }
        else { return GetUniqueGameObjectName(gameObject); }
      }
    }

    [Space(10)]
    [SerializeField]
    private bool savePosition = true;
    [SerializeField]
    private bool saveRotation = true;
    [SerializeField]
    private bool saveScale = true;

    [Space(10)]
    [SerializeField]
    private bool saveActiveState = true;
    [SerializeField]
    private bool saveDestroyedState = false;

    [Space(10)]
    [Header("Components")]
    [SerializeField]
    private Component[] componentsToSave;


    private const string transformPath = "transform";
    private const string gameObjectPath = "gameObject";
    private const string positionPath = "position";
    private const string rotationPath = "rotation";
    private const string scalePath = "scale";
    private const string activePath = "active";

    private const string pathSeparator = "_";

    void Start()
    {
      SaveLoadManager.Instance.OnGameSaved.AddListener(SaveState);
      SaveLoadManager.Instance.OnGameLoaded.AddListener(LoadState);
    }

    void OnDestroy()
    {
      Debug.Log("Destroying " + gameObject.name + " with ID: " + ID);

      if (SaveLoadManager.Instance != null)
      {
        SaveLoadManager.Instance.RegisterDestroyedObject(ID);
      }
    }

    private void SaveState()
    {
      Debug.Log("Saving state for " + gameObject.name + " with ID: " + ID);

      if (savePosition)
      {
        Vector3 position = transform.position;
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, positionPath, "x"}), position.x);
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, positionPath, "y"}), position.y);
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, positionPath, "z"}), position.z);
      }

      if (saveRotation)
      {
        Quaternion rotation = transform.rotation;
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, rotationPath, "x"}), rotation.x);
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, rotationPath, "y"}), rotation.y);
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, rotationPath, "z"}), rotation.z);
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, rotationPath, "w"}), rotation.w);
      }

      if (saveScale)
      {
        Vector3 scale = transform.localScale;
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, scalePath, "x"}), scale.x);
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, scalePath, "y"}), scale.y);
        PlayerPrefs.SetFloat(GetPath(new[] {ID, transformPath, scalePath, "z"}), scale.z);
      }

      if (saveActiveState)
      {
        PlayerPrefs.SetInt(GetPath(new[] {ID, gameObjectPath, activePath}), gameObject.activeSelf ? 1 : 0);
      }

      foreach (Component component in componentsToSave)
      {
        if (component != null && component is ISerializable)
        {
          SaveComponentData(component as ISerializable);
        }
      }
    }

    private void LoadState()
    {
      Debug.Log("Loading state for " + gameObject.name + " with ID: " + ID);

      if (saveDestroyedState && SaveLoadManager.Instance.DestroyedObjects.Contains(ID))
      {
        Destroy(gameObject);
        return;
      }

      if (savePosition)
      {
        float x = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, positionPath, "x"}), transform.position.x);
        float y = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, positionPath, "y"}), transform.position.y);
        float z = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, positionPath, "z"}), transform.position.z);
        transform.position = new Vector3(x, y, z);
      }

      if (saveRotation)
      {
        float x = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, rotationPath, "x"}), transform.rotation.x);
        float y = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, rotationPath, "y"}), transform.rotation.y);
        float z = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, rotationPath, "z"}), transform.rotation.z);
        float w = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, rotationPath, "w"}), transform.rotation.w);
        transform.rotation = new Quaternion(x, y, z, w);
      }

      if (saveScale)
      {
        float x = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, scalePath, "x"}), transform.localScale.x);
        float y = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, scalePath, "y"}), transform.localScale.y);
        float z = PlayerPrefs.GetFloat(GetPath(new[] {ID, transformPath, scalePath, "z"}), transform.localScale.z);
        transform.localScale = new Vector3(x, y, z);
      }

      if (saveActiveState)
      {
        int activeInt = PlayerPrefs.GetInt(GetPath(new[] {ID, gameObjectPath, activePath}), gameObject.activeSelf ? 1 : 0);
        gameObject.SetActive(activeInt == 1);
      }

      foreach (Component component in componentsToSave)
      {
        if (component != null && component is ISerializable)
        {
          LoadComponentData(component as ISerializable);
        }
      }
    }

    private void SaveComponentData(ISerializable component)
    {
      string jsonData = component.Serialize();
      PlayerPrefs.SetString(GetPath(new[] { ID, component.GetType().Name }), jsonData);
    }

    private void LoadComponentData(ISerializable component)
    {
      string jsonData = PlayerPrefs.GetString(GetPath(new[] { ID, component.GetType().Name }), null);
      if (!string.IsNullOrEmpty(jsonData))
      {
        component.Deserialize(jsonData);
      }
    }

    public static string GetPath(string[] path)
    {
      return string.Join(pathSeparator, path);
    }

    public static string GetUniqueGameObjectName(GameObject obj)
    {
      return obj.scene.name + pathSeparator + GetGameObjectParentPathName(obj);
    }
    public static string GetGameObjectParentPathName(GameObject obj)
    {
      if (obj.transform.parent != null)
      {
        return GetGameObjectParentPathName(obj.transform.parent.gameObject) + pathSeparator + obj.name;
      }

      return obj.name;
    }
  }
}