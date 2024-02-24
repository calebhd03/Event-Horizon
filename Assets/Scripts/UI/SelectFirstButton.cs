using UnityEngine.UI;
using UnityEngine;

public class SelectFirstButton : MonoBehaviour
{
    Button button;

    void OnAwake()
    {
        button = GetComponent<Button>();
        button.Select();
    }
}
