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
		public bool aim;
		public bool shoot;
		public bool crouch;
		public bool flashlight;

		public Vector2 scroll;
		public bool scan;
		public bool scanaim;
		public bool scanobj;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header ("Game Save System Testing")] //Save System Test inputs
		public bool save;
		public bool load;
		public bool value;

		[Header ("Menu Systems")]
		public bool pause;

		public bool blackHoleGun;
		public bool switchWeapon;
		public bool blaster;
		public bool shotgun;

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

		public void OnCrouch(InputValue value)
		{
			CrouchInput(value.isPressed);
		}

		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			if( aim == true)
			{
			ShootInput(value.isPressed);
			}
		}
		public void OnScan(InputValue value)
		{
			ScanInput(value.isPressed);
		}
		public void OnScanaim(InputValue value)
		{
			ScanaimInput(value.isPressed);
		}
		public void OnScanobj(InputValue value)
		{
			OnScanobjInput(value.isPressed);
		}

		public void OnScroll(InputValue value)
		{
			ScrollInput(value.Get<Vector2>());
		}

		public void OnSave(InputValue value) //Save System Test input
		{
			SaveInput(value.isPressed);
		}

		public void OnLoad(InputValue value) //Save System Test input
		{
			LoadInput(value.isPressed);
		}

		public void OnValue(InputValue value) //Save System Test input
		{
			ValueInput(value.isPressed);
		}

		public void OnPause(InputValue value)
		{
			PauseInput(value.isPressed);
		}
		public void OnFlashlight(InputValue value)
        {
			FlashlightInput(value.isPressed);
        }
		public void OnBlackHoleGun(InputValue value)
		{
			BlackHoleGunInput(value.isPressed);
		}
		public void OnSwitchWeapon(InputValue value)
		{
			SwitchWeaponInput(value.isPressed);
		}
		public void OnBlaster(InputValue value)
		{
			BlasterInput(value.isPressed);
		}
		public void OnShotgun(InputValue value)
		{
			ShotgunInput(value.isPressed);
		}

		
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void CrouchInput(bool newCrouchState)
		{
			crouch = newCrouchState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

			public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}
		public void ScanInput(bool newScanState)
		{
			scan = newScanState;
		}
		public void ScanaimInput(bool newScanaimState)
		{
			scanaim = newScanaimState;
		}
		public void OnScanobjInput(bool newScanobjState)
		{
			scanobj = newScanobjState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		public void ScrollInput (Vector2 newScrollState)
		{
			scroll = newScrollState;
		}

		public void SaveInput(bool newSaveState) //Save System Test input
		{
			save = newSaveState;
		}

		public void LoadInput(bool newLoadState) //Save System Test input
		{
			load = newLoadState;
		}

		public void ValueInput(bool newValueState) //Save System Test input
		{
			value = newValueState;
		}

		public void PauseInput(bool newPauseState)
		{
			pause =! pause;
		}

		public void FlashlightInput(bool newFlashlightState)
        {
			if (newFlashlightState)
			{
				flashlight = !flashlight;
			}
		}
		
		public void BlackHoleGunInput(bool newBlackHoleGunState)
		{
			blackHoleGun = newBlackHoleGunState;
		}
		public void SwitchWeaponInput(bool newSwitchWeaponState)
		{
			switchWeapon = newSwitchWeaponState;
		}
		public void BlasterInput(bool newBlasterState)
		{
			blaster = newBlasterState;
		}
		public void ShotgunInput(bool newShotgunState)
		{
			shotgun = newShotgunState;
		}
	}
	
}
