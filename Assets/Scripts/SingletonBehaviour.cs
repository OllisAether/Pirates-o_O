using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
  public static T Instance { get; private set; }

  protected virtual void Awake()
  {
    Debug.Log("Awake called on " + gameObject.name + " of type " + typeof(T).Name);
    if (Instance == null)
    {
      Instance = this as T;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }
}