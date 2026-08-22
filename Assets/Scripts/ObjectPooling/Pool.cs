using UnityEngine;

namespace ObjectPooling
{
  public class Pool : MonoBehaviour
  {
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;

    private GameObject[] pool;
    private int currentIndex = 0;

    private void Initialize()
    {
      Debug.Log($"Initializing pool for {prefab.name} with size {initialSize}");

      pool = new GameObject[initialSize];
      for (int i = 0; i < initialSize; i++)
      {
        pool[i] = Instantiate(prefab);
        pool[i].SetActive(false);
      }
    }

    private void Awake()
    {
      Initialize();
    }

    public GameObject Instantiate()
    {
      for (int i = 0; i < pool.Length; i++)
      {
        if (!pool[i].activeInHierarchy)
        {
          pool[i].SetActive(true);
          return pool[i];
        }
      }

      Debug.LogWarning($"Pool of {prefab.name} is exhausted. Expanding the pool.");
      ExpandPool();
      return pool[currentIndex++];
    }

    public void Release(GameObject obj)
    {
      obj.SetActive(false);
    }

    private void ExpandPool()
    {
      int newSize = pool.Length * 2;
      GameObject[] newPool = new GameObject[newSize];

      for (int i = 0; i < pool.Length; i++)
      {
        newPool[i] = pool[i];
      }

      for (int i = pool.Length; i < newSize; i++)
      {
        newPool[i] = Instantiate(prefab);
        newPool[i].SetActive(false);
      }

      pool = newPool;
    }
  }
}