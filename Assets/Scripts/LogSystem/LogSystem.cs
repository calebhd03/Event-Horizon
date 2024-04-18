using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using JetBrains.Annotations;
using System;

public class LogSystem : MonoBehaviour
{
    [SerializeField] public GameObject enemiesButton, memoriesButton, itemButton, skillsButton, journalButton, returnButton;
    [SerializeField] public GameObject enemiesPage, memoriesPage, itemsPage, pauseMenu, LogPage, skillsPage, journalPage;
    [SerializeField] public Button[] enemy, memory, item, journal, skillExit, skills;
    [HideInInspector] public Image[] enemyImage, memoryImage, itemImage, skillsImage;
    [SerializeField] Sprite[] enemySprite, memorySprite, itemSprite, skillsSprite;
    [SerializeField] TextMeshProUGUI[] enemyText, memoryText, itemText, journalText, skillsText;
    public Color enemyPage;
    public static int currentTab;
    private int buttonType;
    [SerializeField] GameObject displayInfo;
    public Image setImage;
    public TextMeshProUGUI setText;
    public TextMeshProUGUI enemyHeadingText, memoriesHeadingText, itemsHeadingText, skillsHeadingText, journalHeadingText;
    public GameObject journalScroll;
    public GameObject scannerCurrentObject;
    public int number;
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private ThirdPersonController thirdPersonController;
    public bool log;
    //Skills
    public bool skillsUnlocked = false, skillsUnlocked2 = false, skillsUnlocked3 = false, skillsUnlocked4 = false;
    public Button plasmaUpgradeButton, SlowEnemyButton, DamageOverTimeButton;
    public Button knockBackButton, OGBHGButton, bHGToolButton, BHGPullButton;
    public bool plasmaSkillUpgraded = false;
    public bool OGBHG = false, SlowEnemyUpgraded = false, DamageOverTimeSkillUpgraded = false;
    public bool knockBackUpgraded = false, BHGToolUpgraded = false;
    public bool BHGPullUpgraded = false;
    public Sprite upgradedSprite;
    AudioSource audioSource;
    public AudioClip SwitchTabSound;
    [SerializeField]SkillTree skillTree;
    [SerializeField]TutorialScript tutorialScript;
    //public GameObject player;
    public Scanning scnScr;
    [SerializeField] ScanCam scanCam;
    //Upgrade option pages
    public GameObject upgradePage1, upgradePage2, upgradePage3, upgradePage4;

    [SerializeField]PauseMenuScript pauseMenuScript;
    [SerializeField]MiniCore miniCore;
    [SerializeField]PlayerHealthMetric playerHealthMetric;

    void Awake()
    {
        miniCore = GetComponentInParent<MiniCore>();
        //player = GameObject.FindWithTag("Player");
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        starterAssetsInputs = miniCore.GetComponentInChildren<StarterAssetsInputs>();
        thirdPersonController = miniCore.GetComponentInChildren<ThirdPersonController>();
        tutorialScript = miniCore.GetComponentInChildren<TutorialScript>();
        audioSource = GetComponent<AudioSource>();
        skillTree = miniCore.GetComponentInChildren<SkillTree>();
        scnScr = miniCore.GetComponentInChildren<Scanning>();
        //scanCam = miniCore.GetComponentInChildren<ScanCam>();
        playerHealthMetric = miniCore.GetComponentInChildren<PlayerHealthMetric>();
    }
    void Start()
    {
        playerHealthMetric.playerData.UpdateLogArrays();
        log = false;
        LogPage.SetActive(false);
        displayInfo.SetActive(false);
        upgradePage1.SetActive(false);
        upgradePage2.SetActive(false);
        upgradePage3.SetActive(false);
        upgradePage4.SetActive(false);
        
        foreach (Button button in enemy)
        {
            int EnemybuttonIndex = Array.IndexOf(enemy, button);
            if(playerHealthMetric.playerData.enemyBools[EnemybuttonIndex] == true)
            {
                button.interactable = true;
                button.gameObject.SetActive(true);
                playerHealthMetric.playerData.UpdateLogArrays();
            }
            else
            {
            button.interactable = false;
            button.gameObject.SetActive(false);
            }
        }
        foreach (Button button in memory)
        {
            int MemorybuttonIndex = Array.IndexOf(memory, button);
            if(playerHealthMetric.playerData.memoryBools[MemorybuttonIndex] == true)
            {
                button.interactable = true;
                button.gameObject.SetActive(true);
            }
            else
            {
            button.interactable = false;
            button.gameObject.SetActive(false);
            }
        }
        foreach (Button button in item)
        {
            int ItembuttonIndex = Array.IndexOf(item, button);
            if(playerHealthMetric.playerData.itemBools[ItembuttonIndex] == true)
            {
                button.interactable = true;
                button.gameObject.SetActive(true);
            }
            else
            {
            button.interactable = false;
            button.gameObject.SetActive(false);
            }
        }
        foreach (Button button in journal)
        {
            int JournalbuttonIndex = Array.IndexOf(journal, button);
            if(playerHealthMetric.playerData.journalBools[JournalbuttonIndex] == true)
            {
                button.interactable = true;
                button.gameObject.SetActive(true);
            }
            else
            {
            button.interactable = false;
            button.gameObject.SetActive(false);
            }
        }
        foreach(Button button in skillExit)
        {
            button.interactable = false;
        }
        foreach(Button button in skills)
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }

        
        enemyImage = new Image[enemy.Length];
        memoryImage = new Image[memory.Length];
        itemImage = new Image[item.Length];
        skillsImage = new Image[skills.Length];


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
        for (int i = 0; i < skills.Length; i++)
        {
            int index = i; // Capture the current value of i for the lambda expression
            skills[i].onClick.AddListener(() => OnButtonClick(index));
        }
        if(tutorialScript.tutorialComplete == false)
        {
        skillsButton.SetActive(false);
        }
    }
    
    public void UpgradesUnlocked()
    {   
        //ReturnButton();
        LogPage.SetActive(false);
        if (skillsUnlocked == true)
        {            
            if (BHGToolUpgraded == true)
            {
                bHGToolButton.interactable = false;
                skillExit[0].interactable = true;
                skillsUnlocked = false;
            }
            else
            {
                bHGToolButton.interactable = true;
            }
        }
        else if (skillsUnlocked2 == true)
        {
            if (SlowEnemyUpgraded == true)
            {
                SlowEnemyButton.interactable = false;
                DamageOverTimeButton.interactable = false;
                skillExit[1].interactable = true;
                skillsUnlocked2 = false;
                
            }
            else
            {
                SlowEnemyButton.interactable = true;
            }
            if (DamageOverTimeSkillUpgraded == true)
            {

                DamageOverTimeButton.interactable = false;
                SlowEnemyButton.interactable = false;
                skillExit[1].interactable = true;
                skillsUnlocked2 = false;
            }
            else
            {
                DamageOverTimeButton.interactable = true;
            }
        }
        else if (skillsUnlocked3 == true)
        { 
            if (knockBackUpgraded == true)
            {
                knockBackButton.interactable = false;
                BHGPullButton.interactable = false;
                skillExit[2].interactable = true;
                skillsUnlocked3 = false;
            }
            else
            {
                knockBackButton.interactable = true;
            }
            if (BHGPullUpgraded == true)
            {
                BHGPullButton.interactable = false;
                knockBackButton.interactable = false;
                skillExit[2].interactable = true;
                skillsUnlocked3 = false;
            }
            else
            {
                BHGPullButton.interactable = true;
            }
        }
        else if (skillsUnlocked4 == true)
        {
            if (plasmaSkillUpgraded == true)
            {
                plasmaUpgradeButton.interactable = false;
                OGBHGButton.interactable = false;
                skillExit[3].interactable = true;
                skillsUnlocked4 = false;
            }
            else
            {
                plasmaUpgradeButton.interactable = true;
            }
            if (OGBHG == true)
            {
                OGBHGButton.interactable = false;
                plasmaUpgradeButton.interactable = false;
                skillExit[3].interactable = true;
                skillsUnlocked4 = false;
            }
            else
            {
                OGBHGButton.interactable = true;
            }
        }
        else 
        {
        plasmaUpgradeButton.interactable = false;
        SlowEnemyButton.interactable = false;
        DamageOverTimeButton.interactable = false;
        knockBackButton.interactable = false;
        OGBHGButton.interactable = false;
        bHGToolButton.interactable = false;
        BHGPullButton.interactable = false;
        }
    }

    public void SetLog()
    {
        UpgradesUnlocked();
        log = true;
        //Debug.LogWarning("log");
        LogPage.SetActive(true);
        starterAssetsInputs.delayShoot = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        SetTab();
    }
    public void CloseLog()
    {   
        log = false;
        //if(scnScr != null) scnScr.HudObject.SetActive(true);
        if(scnScr.Scan == true)
        {
            scnScr.ScanCamPriority();
        }
        else
        {
            scnScr.MainCamPriority();
        }
        //Debug.LogWarning("closelog");
        LogPage.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        Invoke("DelayShoot", 0.1f);

        pauseMenuScript.UnPause();
        //GetComponent<ToolTip>().HideToolTip();
    }

    public void EnemiesTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 0;
        SetTab();
    }

    public void MemoriesTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 1;
        SetTab();
    }

    public void ItemsTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 2;
        SetTab();
    }
    public void SkillsTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 3;
        SetTab();
    }
    public void JournalTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 4;
        SetTab();
    }
    void SetTab()
    {
        switch (currentTab)
        {
        case 0:
            enemiesPage.SetActive(true);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(false);
            skillsPage.SetActive(false);
            journalPage.SetActive(false);
            buttonType = 0;
            enemyHeadingText.color = new Color(1f, 0f, 0f, 1f);
            memoriesHeadingText.color = new Color(1f, 0f, 0f, 1f);
            itemsHeadingText.color = new Color(1f, 0f, 0f, 1f);
            skillsHeadingText.color = new Color(1f, 0f, 0f, 1f);
            journalHeadingText.color = new Color(1f, 0f, 0f, 1f);
        break;
        case 1:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(true);
            itemsPage.SetActive(false);
            skillsPage.SetActive(false);
            journalPage.SetActive(false);
            buttonType = 1;
            enemyHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
            memoriesHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
            itemsHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
            skillsHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
            journalHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
        break;
        case 2:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(true);
            skillsPage.SetActive(false);
            journalPage.SetActive(false);
            buttonType = 2;
            enemyHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);
            memoriesHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);
            itemsHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);
            skillsHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);
            journalHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);

        break;
        case 3:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(false);
            skillsPage.SetActive(true);
            journalPage.SetActive(false);
            buttonType = 3;
            enemyHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
            memoriesHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
            itemsHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
            skillsHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
            journalHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
        break;
        case 4:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(false);
            skillsPage.SetActive(false);
            journalPage.SetActive(true);
            journalScroll.SetActive(true);
            buttonType = 4;
            enemyHeadingText.color = new Color(155f / 255f, 89f / 255f, 182f / 255f, 1f);
            memoriesHeadingText.color = new Color(155f / 255f, 89f / 255f, 182f / 255f, 1f);
            itemsHeadingText.color = new Color(155f / 255f, 89f / 255f, 182f / 255f, 1f);
            skillsHeadingText.color = new Color(155f / 255f, 89f / 255f, 182f / 255f, 1f);
            journalHeadingText.color = new Color(155f / 255f, 89f / 255f, 182f / 255f, 1f);
        break;
        }
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
        playerHealthMetric.playerData.enemyBools[number] = true;
        playerHealthMetric.playerData.UpdateLogArrays();
        }
    }
    public void UpdateMemoryLog()
    {
        if (number >= 0 && number < memoryImage.Length)
        {
        memory[number].image.sprite = memorySprite[number];
        memory[number].interactable = true;
        memory[number].gameObject.SetActive(true);
        playerHealthMetric.playerData.memoryBools[number] = true;
        playerHealthMetric.playerData.UpdateLogArrays();
        }
    }
    public void UpdateItemLog()
    {
        if (number >= 0 && number < itemImage.Length)
        {
        item[number].image.sprite = itemSprite[number];
        item[number].interactable = true;
        item[number].gameObject.SetActive(true);
        playerHealthMetric.playerData.itemBools[number] = true;
        playerHealthMetric.playerData.UpdateLogArrays();
        }
    }
    public void UpdateJournalLog()
    {
        if (number >= 0 && number < journal.Length)
        {
        journal[number].interactable = true;
        journal[number].gameObject.SetActive(true);
        playerHealthMetric.playerData.journalBools[number] = true;
        playerHealthMetric.playerData.UpdateLogArrays();
        }
    }
    public void UpdateSkillsLog()
    {
        if (number >= 0 && number < skills.Length)
        {
        skills[number].interactable = true;
        skills[number].gameObject.SetActive(true);
        }
    }

    public void ReturnButton()
    {
        starterAssetsInputs.LogInput(true);
    }

    public void DisplayInfo()
    {
        displayInfo.SetActive(true);
        LogPage.SetActive(false);
    }

    public void returnToLog()
    {
        upgradePage1.SetActive(false);
        upgradePage2.SetActive(false);
        upgradePage3.SetActive(false);
        upgradePage4.SetActive(false);
        displayInfo.SetActive(false);
        LogPage.SetActive(true);
        log = true;
        SetTab();
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
                        Image sourceImage2 = item[buttonIndex].GetComponent<Image>();

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
                case 3:
                        Image sourceImage3 = skills[buttonIndex].GetComponent<Image>();

                        if (sourceImage3 != null)
                        {
                            // Get the sprite from the sourceImage
                            Sprite sourceSprite3 = sourceImage3.sprite;

                            // Set the sprite to the corresponding targetImage
                            setImage.sprite = sourceSprite3;
                        }
                        else
                        {
                            Debug.LogError("SourceButton does not have an Image component!");
                        }
                break;
            }


    }
        void UpdateText(int buttonIndex)
    {
            switch (buttonType)
            {
                case 0:
                        setText.text = enemyText[buttonIndex].text;
                break;
                case 1:
                        setText.text = memoryText[buttonIndex].text;
                break;
                case 2:
                        setText.text = itemText[buttonIndex].text;
                break;
                case 3:
                        setText.text = skillsText[buttonIndex].text;
                break;
                case 4:
                        setText.text = journalText[buttonIndex].text;
                break;
            }


    }

    // This method is called when any button in the array is clicked
    void OnButtonClick(int buttonIndex)
    {
        UpdateImage(buttonIndex);
        UpdateText(buttonIndex);
    }

    public void UpgradeToPlasma()
    {
        //Debug.LogWarning("SayHelloToMyLittleFriend");
        plasmaSkillUpgraded = true;
        plasmaUpgradeButton.image.sprite = upgradedSprite;
        skillTree.PlasmaUpgrade();
        number = 3;
        UpdateSkillsLog();
    }

    public void UpgradeToDamageOverTime()
    {
        //Debug.LogWarning("BURN! - Kelso");
        DamageOverTimeSkillUpgraded = true;
        DamageOverTimeButton.image.sprite = upgradedSprite;
        skillTree.DamageOverTimeUpgrade();
        number = 5;
        UpdateSkillsLog();
    }

    public void UpgradeSlowEnemyBullets()
    {
        //Debug.LogWarning("But you told me to Freeze - the mask");
        SlowEnemyUpgraded = true;
        SlowEnemyButton.image.sprite = upgradedSprite;
        skillTree.SlowEnemyUpgrade();
        number = 1;
        UpdateSkillsLog();
    }
    /*public void UpgradeMeleeDamage()
    {
        meleeSkillUpgraded = true;
        meleeButton.image.sprite = upgradedSprite;
        skillTree.MeleeDamageUpgrade();
    }*/

    public void UpgradeKnockBack()
    {
        knockBackUpgraded = true;
        knockBackButton.image.sprite = upgradedSprite;
        skillTree.KnockBackUpgrade();
        number = 2;
        UpdateSkillsLog();
    }
    public void OGBHGUpgrade()
    {
        OGBHG = true;
        OGBHGButton.image.sprite = upgradedSprite;
        skillTree.OGBHGUpgrade();
        number = 7;
        UpdateSkillsLog();
    }

    public void BHGToolUpgrade()
    {
        BHGToolUpgraded = true;
        tutorialScript.HasNexusTool();
        bHGToolButton.image.sprite = upgradedSprite;
        skillTree.BHGToolUpgrade();
        skillsButton.SetActive(true);
        currentTab = 3;
        SetTab();
        number = 4;
        UpdateSkillsLog();
    }
    public void BHGPullUpgrade()
    {
        BHGPullUpgraded = true;
        BHGPullButton.image.sprite = upgradedSprite;
        skillTree.BHGPullUpgrade();
        number = 6;
        UpdateSkillsLog();
    }
    
    public void DelayShoot()
    {
        starterAssetsInputs.delayShoot = false;
    }

    public void UpgradeAlreadySelected()
    {
        skillsButton.SetActive(true);
        currentTab = 3;
        SetTab();
    }

}
