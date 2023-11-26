using System.Collections;
using System.Collections.Generic;
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
    public static int currentTab;
    [SerializeField] private int buttonType;
    [SerializeField] GameObject displayInfo;
    public Image setImage;

    void Start()
    {
        LogPage.SetActive(false);
        
        foreach (Button button in enemy)
        {
            button.interactable = false;
        }
        foreach (Button button in memory)
        {
            button.interactable = false;
        }
        foreach (Button button in item)
        {
            button.interactable = false;
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
        EnemiesScanScript enemiesScanScript = FindObjectOfType<EnemiesScanScript>();
        if (enemiesScanScript.number >= 0 && enemiesScanScript.number < enemyImage.Length)
        {
        enemy[enemiesScanScript.number].image.sprite = enemySprite[enemiesScanScript.number];
        enemy[enemiesScanScript.number].interactable = true;
        }
    else
    {
        Debug.LogError("Invalid enemy index: " + enemiesScanScript.number);
    }
    }
    public void UpdateMemoryLog()
    {
        ObjectivesScript objectivesScript = FindObjectOfType<ObjectivesScript>();
        if (objectivesScript.number >= 0 && objectivesScript.number < memoryImage.Length)
        {
        memory[objectivesScript.number].image.sprite = memorySprite[objectivesScript.number];
        memory[objectivesScript.number].interactable = true;
        }
    }
    public void UpdateItemLog()
    {
        ItemsScript itemsScript = FindObjectOfType<ItemsScript>();
        if (itemsScript.number >= 0 && itemsScript.number < itemImage.Length)
        {
        item[itemsScript.number].image.sprite = itemSprite[itemsScript.number];
        item[itemsScript.number].interactable = true;
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
                break;
            }


    }

    // This method is called when any button in the array is clicked
    void OnButtonClick(int buttonIndex)
    {
        UpdateImage(buttonIndex);
    }
}
