using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using JetBrains.Annotations;

public class LogSystem : MonoBehaviour
{
    [SerializeField] GameObject enemiesButton, memoriesButton, itemButton, skillsButton, returnButton;
    [SerializeField] public GameObject enemiesPage, memoriesPage, itemsPage, pauseMenu, LogPage, skillsPage;
    [SerializeField] public Button[] enemy, memory, item;
    [HideInInspector] public Image[] enemyImage, memoryImage, itemImage;
    [SerializeField] Sprite[] enemySprite, memorySprite, itemSprite;
    [SerializeField] TextMeshProUGUI[] enemyText, memoryText, itemText;
    public Color enemyPage;
    public static int currentTab;
    private int buttonType;
    [SerializeField] GameObject displayInfo;
    public Image setImage;
    public TextMeshProUGUI setText;
    public TextMeshProUGUI enemyHeadingText, memoriesHeadingText, itemsHeadingText, skillsHeadingText;
    public GameObject scannerCurrentObject;
    public int number;
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    public bool log;
    //Skills
    public bool skillsUnlocked = false, skillsUnlocked2 = false, skillsUnlocked3 = false, skillsUnlocked4 = false;
    public Button plasmaUpgradeButton, SlowEnemyButton, DamageOverTimeButton;
    public Button meleeButton, knockBackButton, laserButton, bHGToolButton;
    public bool plasmaSkillUpgraded = false;
    public bool laserUpgraded = false, SlowEnemyUpgraded = false, DamageOverTimeSkillUpgraded = false;
    public bool meleeSkillUpgraded = false, knockBackUpgraded = false, BHGToolUpgraded = false;
    public Sprite upgradedSprite;
    AudioSource audioSource;
    public AudioClip SwitchTabSound;
    SkillTree skillTree;
    public Scanning scnScr;
    //Upgrade option pages
    public GameObject upgradePage1, upgradePage2, upgradePage3, upgradePage4;
    void Start()
    {
        log = false;
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
        LogPage.SetActive(false);
        displayInfo.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        skillTree = FindObjectOfType<SkillTree>();
        scnScr = FindObjectOfType<Scanning>();
        upgradePage1.SetActive(false);
        upgradePage2.SetActive(false);
        upgradePage3.SetActive(false);
        upgradePage4.SetActive(false);
        
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
        if (log == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        switch (currentTab)
        {
        case 0:
            enemiesPage.SetActive(true);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(false);
            skillsPage.SetActive(false);
            buttonType = 0;
            enemyHeadingText.color = new Color(1f, 0f, 0f, 1f);
            memoriesHeadingText.color = new Color(1f, 0f, 0f, 1f);
            itemsHeadingText.color = new Color(1f, 0f, 0f, 1f);
            skillsHeadingText.color = new Color(1f, 0f, 0f, 1f);
        break;
        case 1:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(true);
            itemsPage.SetActive(false);
            skillsPage.SetActive(false);
            buttonType = 1;
            enemyHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
            memoriesHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
            itemsHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
            skillsHeadingText.color = new Color(0f, 133f / 255f, 255f / 255f, 1f);
        break;
        case 2:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(true);
            skillsPage.SetActive(false);
            buttonType = 2;
            enemyHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);
            memoriesHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);
            itemsHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);
            skillsHeadingText.color = new Color(0f, 1f, 31f / 255f, 1f);

        break;
        case 3:
            enemiesPage.SetActive(false);
            memoriesPage.SetActive(false);
            itemsPage.SetActive(false);
            skillsPage.SetActive(true);
            buttonType = 3;
            enemyHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
            memoriesHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
            itemsHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
            skillsHeadingText.color = new Color(231f / 255f, 120f / 255f, 31f / 255f, 1f);
        break;
        }
        
        if (skillsUnlocked == true)
        {
            /*if (healthSkillUpgraded == true)
            {
                healthUpgradeButton.interactable = false;
                skillsUnlocked = false;
                upgradePage1.SetActive(false);
            }
            else
            {
                healthUpgradeButton.interactable = true;
            }*/
            
            if (BHGToolUpgraded == true)
            {
                bHGToolButton.interactable = false;
                skillsUnlocked = false;
                upgradePage1.SetActive(false);
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
                skillsUnlocked2 = false;
                upgradePage2.SetActive(false);
            }
            else
            {
                SlowEnemyButton.interactable = true;
            }
            if (DamageOverTimeSkillUpgraded == true)
            {

                DamageOverTimeButton.interactable = false;
                SlowEnemyButton.interactable = false;
                skillsUnlocked2 = false;
                upgradePage2.SetActive(false);
            }
            else
            {
                DamageOverTimeButton.interactable = true;
            }
            /*if (speedSkillUpgraded == true)
            {
                meleeButton.interactable = false;
                speedUpgradeButton.interactable = false;
                ammoCapactiyButton.interactable = false;
                skillsUnlocked2 = false;
                upgradePage2.SetActive(false);
            }
            else
            {
                speedUpgradeButton.interactable = true;
            }*/
            
            /*if (ammoSkillUpgraded == true)
            {
                meleeButton.interactable = false;
                ammoCapactiyButton.interactable = false;
                speedUpgradeButton.interactable = false;
                skillsUnlocked2 = false;
                upgradePage2.SetActive(false);
            }
            else
            {
                ammoCapactiyButton.interactable = true;
            }*/
            /*if (meleeSkillUpgraded == true)
            {
                meleeButton.interactable = false;
                ammoCapactiyButton.interactable = false;
                speedUpgradeButton.interactable = false;
                skillsUnlocked2 = false;
                upgradePage2.SetActive(false);
            }
            else
            {
                meleeButton.interactable = true;
            }*/
        }
        else if (skillsUnlocked3 == true)
        { 
            if (knockBackUpgraded == true)
            {
                knockBackButton.interactable = false;
                skillsUnlocked3 = false;
                upgradePage3.SetActive(false);
            }
            else
            {
                knockBackButton.interactable = true;
            }
        }
        else if (skillsUnlocked4 == true)
        {
            if (plasmaSkillUpgraded == true)
            {
                plasmaUpgradeButton.interactable = false;
                laserButton.interactable = false;
                skillsUnlocked4 = false;
                upgradePage4.SetActive(false);
            }
            else
            {
                plasmaUpgradeButton.interactable = true;
            }
            if (laserUpgraded == true)
            {
                laserButton.interactable = false;
                plasmaUpgradeButton.interactable = false;
                skillsUnlocked4 = false;
                upgradePage4.SetActive(false);
            }
            else
            {
                laserButton.interactable = true;
            }
        }
        else 
        {
        //healthUpgradeButton.interactable = false;
        plasmaUpgradeButton.interactable = false;
        //speedUpgradeButton.interactable = false;
        //ammoCapactiyButton.interactable = false;
        SlowEnemyButton.interactable = false;
        DamageOverTimeButton.interactable = false;
        meleeButton.interactable = false;
        knockBackButton.interactable = false;
        laserButton.interactable = false;
        bHGToolButton.interactable = false;
        }
    }

    public void SetLog()
    {
        log = true;
        //Debug.LogWarning("log");
        LogPage.SetActive(true);
        starterAssetsInputs.delayShoot = true;
    }
    public void CloseLog()
    {   
        log = false;
        scnScr.HudObject.SetActive(true);
        //Debug.LogWarning("closelog");
        LogPage.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        Invoke("DelayShoot", 0.1f);
    }

    public void EnemiesTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 0;
    }

    public void MemoriesTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 1;
        
    }

    public void ItemsTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 2;
    }
    public void SkillsTab()
    {
        audioSource.PlayOneShot(SwitchTabSound);
        currentTab = 3;
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
        //Debug.LogError("Invalid enemy index: " + number);
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
            }


    }

    // This method is called when any button in the array is clicked
    void OnButtonClick(int buttonIndex)
    {
        UpdateImage(buttonIndex);
        UpdateText(buttonIndex);
    }

    /*public void UpgradeSpeed()
    {
        //Debug.LogWarning("IAmSpeed");
        speedSkillUpgraded = true;
        speedUpgradeButton.image.sprite = upgradedSprite;
        skillTree.SpeedUpgraded();
    }*/

    /*public void UpgradeHealth()
    {
        //Debug.LogWarning("1Up");
        healthSkillUpgraded = true;
        healthUpgradeButton.image.sprite = upgradedSprite;
        skillTree.HealthUpgraded();
    }*/

    public void UpgradeToPlasma()
    {
        //Debug.LogWarning("SayHelloToMyLittleFriend");
        plasmaSkillUpgraded = true;
        plasmaUpgradeButton.image.sprite = upgradedSprite;
        //skillTree.PlasmaUpgraded();
    }

    /*public void UpgradeAmmoCapacity()
    {
        //Debug.LogWarning("Ammo capacity increase");
        ammoSkillUpgraded = true;
        ammoCapactiyButton.image.sprite = upgradedSprite;
        //Put code here for ammo capacity function or call to thirdpersonshootercontroller
    }*/

    public void UpgradeToDamageOverTime()
    {
        //Debug.LogWarning("BURN! - Kelso");
        DamageOverTimeSkillUpgraded = true;
        DamageOverTimeButton.image.sprite = upgradedSprite;
        skillTree.DamageOverTimeUpgrade();
    }

    public void UpgradeSlowEnemyBullets()
    {
        //Debug.LogWarning("But you told me to Freeze - the mask");
        SlowEnemyUpgraded = true;
        SlowEnemyButton.image.sprite = upgradedSprite;
        skillTree.SlowEnemyUpgrade();
    }
    public void UpgradeMeleeDamage()
    {
        meleeSkillUpgraded = true;
        meleeButton.image.sprite = upgradedSprite;
        skillTree.MeleeDamageUpgrade();
    }

    public void UpgradeKnockBack()
    {
        knockBackUpgraded = true;
        knockBackButton.image.sprite = upgradedSprite;
        skillTree.KnockBackUpgrade();
    }
    public void LaserUpgrade()
    {
        laserUpgraded = true;
        laserButton.image.sprite = upgradedSprite;
        skillTree.LaserUpgrade();
    }

    public void BHGToolUpgrade()
    {
        BHGToolUpgraded = true;
        bHGToolButton.image.sprite = upgradedSprite;
        skillTree.BHGToolUpgrade();
    }
    
    public void DelayShoot()
    {
        starterAssetsInputs.delayShoot = false;
    }
}
