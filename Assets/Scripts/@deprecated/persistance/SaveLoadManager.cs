using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using Utils;

namespace Persistence
{
  [Serializable]
  class DestroyedObjectsData
  {
    public List<string> destroyedObjects;

    public DestroyedObjectsData(List<string> destroyedObjects)
    {
      this.destroyedObjects = destroyedObjects;
    }
  }

  public class SaveLoadManager : SingletonBehaviour<SaveLoadManager>
  {
    [SerializeField]
    private bool loadOnStart = false;
    [SerializeField]
    private bool autoSaveOnSceneChange = true;
    [SerializeField]
    private bool autoSaveOnApplicationQuit = true;

    public List<string> DestroyedObjects { get; private set; } = new List<string>();

    private bool startLoaded = false;
    void Update()
    {
      // Load game on first Update
      if (loadOnStart && !startLoaded)
      {
        LoadGame();
        startLoaded = true;
      }
    }

    void OnEnable()
    {
      if (autoSaveOnSceneChange)
      {
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
      }
    }

    void OnDisable()
    {
      if (autoSaveOnSceneChange)
      {
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
      }
    }

    void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
    {
      SaveGame();
    }

    void OnApplicationQuit()
    {
      if (autoSaveOnApplicationQuit)
      {
        SaveGame();
      }
    }

    public void RegisterDestroyedObject(string id)
    {
      DestroyedObjects.Add(id);
    }

    public void SaveGame()
    {
      onGameSaved.Invoke();
      PlayerPrefs.SetString("DestroyedObjects", JsonUtility.ToJson(new DestroyedObjectsData(DestroyedObjects)));
      Debug.Log(JsonUtility.ToJson(new DestroyedObjectsData(DestroyedObjects)));
    }

    public void LoadGame()
    {
      onGameLoaded.Invoke();
      string destroyedObjectsJson = PlayerPrefs.GetString("DestroyedObjects", "[]");
      DestroyedObjects = JsonUtility.FromJson<DestroyedObjectsData>(destroyedObjectsJson).destroyedObjects;
    }

    [SerializeField]
    private UnityEvent onGameSaved = new UnityEvent();
    public UnityEvent OnGameSaved => onGameSaved;
    [SerializeField]
    private UnityEvent onGameLoaded = new UnityEvent();
    public UnityEvent OnGameLoaded => onGameLoaded;
  }

}