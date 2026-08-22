using UnityEngine;
using Utils;

namespace GameManager
{
  public class PlayerManager : SingletonBehaviour<PlayerManager>
  {
    [SerializeField] private GameObject currentPlayer;
    public GameObject CurrentPlayer { get { return currentPlayer; } set { currentPlayer = value; } }
    [SerializeField] private GameObject currentPlayerCameraRoot;
    public GameObject CurrentPlayerCameraRoot { get { return currentPlayerCameraRoot; } set { currentPlayerCameraRoot = value; } }
  }
}