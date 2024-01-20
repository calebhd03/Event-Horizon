using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class LogSystem : MonoBehaviour
{
    [SerializeField] GameObject enemiesButton, memoriesButton, itemButton, returnButton;
    [SerializeField] GameObject enemiesPage, memoriesPage, itemsPage, pauseMenu, LogPage;
    [SerializeField] Button[] enemy, memory, item;
    [HideInInspector] public Image[] enemyImage, memoryImage, itemImage;
    [SerializeField] Sprite[] enemySprite, memorySprite, itemSprite;
    [SerializeField] TextMeshProUGUI[] enemyText, memoryText, itemText;
    public static int currentTab;
    private int buttonType;
    [SerializeField] GameObject displayInfo;
    public Image setImage;
    public TextMeshProUGUI setText;
    public GameObject scannerCurrentObject;
    public int number;

    void Start()
    {
        LogPage.SetActive(false);
        displayInfo.SetActive(false);
        
        foreach (Button button in enemy)
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }
        foreach (Button button in memory)
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }
        foreach (Button button in item)
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }
        
        enemyImage = new Image[enemy.Length];
        memoryImage = new Image[memory.Length];
        itemImage = new Image[item.Length];

        // Attach the OnClick method to each button's click event
        for (int i = 0; i < enemy.Length; i++)
        {
            int index = i; // Capture the current value of i for the lambda expression
            enemy[i].onClick.AddListener(() => OnButtonClick(index));
        }
        for (int i = 0; i < memory.Length; i++)
        {
            int index = i; // Capture the current value of i for the lambda expression
            memory[i].onClick.AddListener(() => OnButtonClick(index));
        }
        for (int i = 0; i < item.Length; i++)
        {
            int index = i; // Capture the current value of i for the lambda expression
            item[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    void Update()
    {
        switch (currentTab)
        {
        case 0:
            enemiesPage.SetActive(true);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(false);
            buttonType = 0;
            
        break;
        case 1:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(true);
            itemsPage.SetActive(false);
            buttonType = 1;
        break;
        case 2:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(true);
            buttonType = 2;
        break;
        }
    }

    public void EnemiesTab()
    {
        currentTab = 0;
    }

    public void MemoriesTab()
    {
        currentTab = 1;
    }

    public void ItemsTab()
    {
        currentTab = 2;
    }
    
    public void ReturnToPause()
    {
        LogPage.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void UpdateEnemyLog()
    {
        if (number >= 0 && number < enemyImage.Length)
        {
        enemy[number].image.sprite = enemySprite[number];
        enemy[number].interactable = true;
        enemy[number].gameObject.SetActive(true);
        }
    else
    {
        Debug.LogError("Invalid enemy index: " + number);
    }
    }
    public void UpdateMemoryLog()
    {
        if (number >= 0 && number < memoryImage.Length)
        {
        memory[number].image.sprite = memorySprite[number];
        memory[number].interactable = true;
        memory[number].gameObject.SetActive(true);
        }
    }
    public void UpdateItemLog()
    {
        if (number >= 0 && number < itemImage.Length)
        {
        item[number].image.sprite = itemSprite[number];
        item[number].interactable = true;
        item[number].gameObject.SetActive(true);
        }
    }

    public void TestButton()
    {
        Debug.LogWarning("button works");
    }

    public void DisplayInfo()
    {
        displayInfo.SetActive(true);
        LogPage.SetActive(false);
    }

    public void returnToLog()
    {
        displayInfo.SetActive(false);
        LogPage.SetActive(true);
    }
    void UpdateImage(int buttonIndex)
    {
            switch (buttonType)
            {
                case 0:
                        Image sourceImage = enemy[buttonIndex].GetComponent<Image>();

                        if (sourceImage != null)
                        {
                            // Get the sprite from the sourceImage
                            Sprite sourceSprite = sourceImage.sprite;

                            // Set the sprite to the corresponding targetImage
                            setImage.sprite = sourceSprite;
                        }
                        else
                        {
                            Debug.LogError("SourceButton does not have an Image component!");
                        }
                        setText.text = enemyText[number].text;
                break;
                case 1:
                        Image sourceImage1 = memory[buttonIndex].GetComponent<Image>();

                        if (sourceImage1 != null)
                        {
                            // Get the sprite from the sourceImage
                            Sprite sourceSprite1 = sourceImage1.sprite;

                            // Set the sprite to the corresponding targetImage
                            setImage.sprite = sourceSprite1;
                        }
                        else
                        {
                            Debug.LogError("SourceButton does not have an Image component!");
                        }
                        setText.text = memoryText[number].text;
                break;
                case 2:
                        Image sourceImage2 = enemy[buttonIndex].GetComponent<Image>();

                        if (sourceImage2 != null)
                        {
                            // Get the sprite from the sourceImage
                            Sprite sourceSprite2 = sourceImage2.sprite;

                            // Set the sprite to the corresponding targetImage
                            setImage.sprite = sourceSprite2;
                        }
                        else
                        {
                            Debug.LogError("SourceButton does not have an Image component!");
                        }
                        setText.text = itemText[number].text;
                break;
            }


    }

    // This method is called when any button in the array is clicked
    void OnButtonClick(int buttonIndex)
    {
        UpdateImage(buttonIndex);
    }
}
