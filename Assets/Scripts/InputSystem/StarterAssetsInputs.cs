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
		public bool reload;

		public Vector2 scroll;
		public bool scan;
		public bool scanaim;
		public bool scanobj;
		public bool log;

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
		public bool quit;
		public bool Confirm;

		public bool blackHoleGun;
		public bool switchWeapon;
		public bool blaster;
		public bool shotgun;
		public bool swapBHG;

		[Header ("Dev Controls")]
		public bool teleport1;
		public bool teleport2;
		public bool teleport3;
		public bool ammo;
        public bool outerTP;
        public bool innerTP;
        public bool centerTP;


        private void Start()
        {
			Debug.LogWarning("START CURSOR LOCK");
            SetCursorState(cursorLocked);
        }

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
				Debug.Log("Look input: " + value.Get<Vector2>());
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
			//if( aim == true)
			//{
			ShootInput(value.isPressed);
			//}
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
		public void OnLog(InputValue value)
		{
			LogInput(value.isPressed);
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
		public void OnConfirm(InputValue value)
		{
			ConfirmInput(value.isPressed);
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
		public void OnQuit(InputValue value)
		{
			QuitInput(value.isPressed);
		}
		public void OnReload(InputValue value)
		{
			ReloadInput(value.isPressed);
		}
		public void OnTeleport1(InputValue value) //Dev control
		{
			Teleport1Input(value.isPressed);
        }
        public void OnTeleport2(InputValue value) //Dev control
        {
            Teleport2Input(value.isPressed);
        }
        public void OnOuterTP(InputValue value) //Dev control
        {
			OuterTPInput(value.isPressed);
        }
        public void OnInnerTP(InputValue value) //Dev control
        {
            InnerTPInput(value.isPressed);
        }
        public void OnCenterTP(InputValue value) //Dev control
        {
            CenterTPInput(value.isPressed);
        }
        public void OnTeleport3(InputValue value) //Dev control
		{
			Teleport3Input(value.isPressed);
		}
		public void OnAmmo(InputValue value) //Dev control
		{
			AmmoInput(value.isPressed);
		}
		public void OnSwapBHG(InputValue value)
		{
			SwapBHGInput(value.isPressed);
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
			if (newSprintState)
			{
				sprint = !sprint;
			}
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
		public void LogInput(bool newLogState)
		{
			log = newLogState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			Debug.LogWarning("LockedCursoir");
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
		public void QuitInput(bool newQuitState)
		{
			quit = newQuitState;
		}

		public void ConfirmInput(bool newConfrimState)
		{
			Confirm = newConfrimState;
		}

		public void ReloadInput(bool newReloadState)
		{
			reload = newReloadState;
		}
		public void Teleport1Input(bool newTeleport1State) //Dev Controls
		{
			teleport1 = newTeleport1State;
		}
		public void Teleport2Input(bool newTeleport2State) //Dev Controls
		{
			teleport2 = newTeleport2State;
        }
        public void Teleport3Input(bool newTeleport3State) //Dev Controls
        {
            teleport3 = newTeleport3State;
        }
        public void OuterTPInput(bool state) //Dev Controls
        {
            outerTP = state;
        }
        public void InnerTPInput(bool state) //Dev Controls
        {
            innerTP = state;
        }
        public void CenterTPInput(bool state) //Dev Controls
        {
            centerTP = state;
        }
        public void AmmoInput(bool newAmmoState) //Dev Controls
		{
			ammo = newAmmoState;
		}
		public void SwapBHGInput(bool newSwapBHGState)
		{
			if (newSwapBHGState)
			{
				swapBHG = !swapBHG;
			}
		}
	}
}
