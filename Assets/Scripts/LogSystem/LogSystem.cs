using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogSystem : MonoBehaviour
{
    [SerializeField] GameObject enemiesButton, memoriesButton, itemButton, returnButton;
    [SerializeField] GameObject enemiesPage, memoriesPage, itemsPage, pauseMenu, LogPage;
    [SerializeField] Button[] enemy, memory, item;
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
        enemy[enemiesScanScript.number].interactable = true;
    }
    public void UpdateMemoryLog()
    {
        ObjectivesScript objectivesScript = FindObjectOfType<ObjectivesScript>();
        memory[objectivesScript.number].interactable = true;
    }
    public void UpdateItemLog()
    {
        ItemsScript itemsScript = FindObjectOfType<ItemsScript>();
        item[itemsScript.number].interactable = true;
    }

    public void TestButton()
    {
        Debug.LogWarning("button works");
    }
}
