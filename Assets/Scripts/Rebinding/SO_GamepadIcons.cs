using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Gamepad Icons", menuName = "ScriptableObjects/Gamepad Icons", order = 1)]
public class SO_GamepadIcons : ScriptableObject
{
    public GamepadIcons xboxIcons;
    public GamepadIcons psIcons;
    public PCIcons pcIcons;

    [Serializable]
    public struct GamepadIcons
    {
        public Sprite buttonSouth;
        public Sprite buttonNorth;
        public Sprite buttonEast;
        public Sprite buttonWest;
        public Sprite startButton;
        public Sprite selectButton;
        public Sprite leftTrigger;
        public Sprite rightTrigger;
        public Sprite leftShoulder;
        public Sprite rightShoulder;
        public Sprite dpad;
        public Sprite dpadUp;
        public Sprite dpadDown;
        public Sprite dpadLeft;
        public Sprite dpadRight;
        public Sprite leftStick;
        public Sprite rightStick;
        public Sprite leftStickPress;
        public Sprite rightStickPress;

        public Sprite GetSprite(string controlPath)
        {
            // From the input system, we get the path of the control on device. So we can just
            // map from that to the sprites we have for gamepads.
            switch (controlPath)
            {
                case "buttonSouth": return buttonSouth;
                case "buttonNorth": return buttonNorth;
                case "buttonEast": return buttonEast;
                case "buttonWest": return buttonWest;
                case "start": return startButton;
                case "select": return selectButton;
                case "leftTrigger": return leftTrigger;
                case "rightTrigger": return rightTrigger;
                case "leftShoulder": return leftShoulder;
                case "rightShoulder": return rightShoulder;
                case "dpad": return dpad;
                case "dpad/up": return dpadUp;
                case "dpad/down": return dpadDown;
                case "dpad/left": return dpadLeft;
                case "dpad/right": return dpadRight;
                case "leftStick": return leftStick;
                case "rightStick": return rightStick;
                case "leftStickPress": return leftStickPress;
                case "rightStickPress": return rightStickPress;
            }
            return null;
        }
    }

    [Serializable]
    public struct PCIcons
    {
        public Sprite GetSprite(string controlPath)
        {
            throw new NotImplementedException();
        }
    }

    public Sprite GetSprite(string controlScheme, string controlPath)
    {
        switch(controlScheme)
        {
            case "Gamepad": case "XboxController": return xboxIcons.GetSprite(controlPath);
            case "KeyboardMouse": return pcIcons.GetSprite(controlPath);
            case "PS4Controller": return psIcons.GetSprite(controlPath);
        }
        return null;
    }
}
