using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[SerializeField] private bool enablePlayerInput = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			if (!enablePlayerInput) return;

			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			if (!enablePlayerInput) return;

			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			if (!enablePlayerInput) return;
		
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			if (!enablePlayerInput) return;

			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			// SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void Start()
		{
			SetCursorState(cursorLocked);
		}

		private void Update()
		{
			if (!enablePlayerInput) {
				look = Vector2.zero;
				move = Vector2.zero;

				jump = false;
				sprint = false;
			}
		}
	}
}