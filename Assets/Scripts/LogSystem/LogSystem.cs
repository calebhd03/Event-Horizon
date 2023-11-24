using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogSystem : MonoBehaviour
{
    [SerializeField] GameObject enemiesButton, memoriesButton, itemButton, returnButton;
    [SerializeField] GameObject enemiesPage, memoriesPage, itemsPage, pauseMenu, LogPage;
    [SerializeField] Button[] enemy, memory, item;
    private Image[] enemyImage, memoryImage, itemImage;
    [SerializeField] Sprite[] enemySprite, memorySprite, itemSprite;
    public static int currentTab;

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
    }

    void Update()
    {
        switch (currentTab)
        {
        case 0:
            enemiesPage.SetActive(true);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(false);
            
        break;
        case 1:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(true);
            itemsPage.SetActive(false);
        break;
        case 2:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(true);
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
        if (objectivesScript.number >= 0 && objectivesScript.number < enemyImage.Length)
        {
        memory[objectivesScript.number].image.sprite = memorySprite[objectivesScript.number];
        memory[objectivesScript.number].interactable = true;
        }
    }
    public void UpdateItemLog()
    {
        ItemsScript itemsScript = FindObjectOfType<ItemsScript>();
        if (itemsScript.number >= 0 && itemsScript.number < enemyImage.Length)
        {
        item[itemsScript.number].image.sprite = itemSprite[itemsScript.number];
        item[itemsScript.number].interactable = true;
        }
    }

    public void TestButton()
    {
        Debug.LogWarning("button works");
    }
}
