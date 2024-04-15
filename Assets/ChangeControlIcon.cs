using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputBinding;

public class ChangeControlIcon : MonoBehaviour
{
    [SerializeField] string actionName;
    SpriteRenderer spriteRenderer;
    public SO_GamepadIcons icons;
    Image imageComponent;
    TextMeshProUGUI textComponent;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  
        imageComponent = GetComponent<Image>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        StarterAssetsInputs.ChangedControlSchemeEvent += OnUpdateBindingDisplay;
    }
    private void OnDisable()
    {
        StarterAssetsInputs.ChangedControlSchemeEvent -= OnUpdateBindingDisplay;
    }
    private void Start()
    {
        StarterAssetsInputs.ChangedControlSchemeEvent += OnUpdateBindingDisplay;
        OnUpdateBindingDisplay(StarterAssetsInputs.Instance.currentControlScheme);
    }

    protected void OnUpdateBindingDisplay(string deviceLayoutName)
    {
        if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(actionName)) return;

        InputActionAsset actions = StarterAssetsInputs.Instance.playerInput.actions; // Current action of the player

        InputAction a = actions.FindAction(actionName); // Input action that we are looking to find

        if(a == null)
        {
            Debug.LogError("Action name " + actionName + " not found");
            return;
        }

        // Find the binding index of the current control scheme
        int bindingIndex;
        for (bindingIndex = 0; bindingIndex < a.bindings.Count-1; bindingIndex++)
        {
            if (a.bindings[bindingIndex].groups.Contains(deviceLayoutName)) break;
        }

        // Get the control path name
        string controlPath; DisplayStringOptions displayStringOptions = default; 
        a.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, displayStringOptions);

        // Returns the corresponding icon sprite for the linked control path
        var icon = default(Sprite);
        if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad"))
            icon = icons.psIcons.GetSprite(controlPath);
        else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
            icon = icons.xboxIcons.GetSprite(controlPath);
        else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "KeyboardMouse"))
            icon = icons.pcIcons.GetSprite(controlPath);


        // Display the icon
        if (icon != null)
        {
            imageComponent.sprite = icon;
            imageComponent.enabled = true;
            if(textComponent != null) textComponent.enabled = false;
        }
        else
        {
            imageComponent.sprite = null;
            imageComponent.enabled = false;

            if (textComponent != null) textComponent.enabled = true;
            if (textComponent != null) textComponent.text = controlPath.ToUpper();
        }
    }
}
