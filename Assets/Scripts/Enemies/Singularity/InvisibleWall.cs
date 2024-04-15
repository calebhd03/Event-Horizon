using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    //add as much as we want
    public GameObject wall;
    public GameObject boss;
    public bool triggerWall = false;
    public bool isTriggered = false;
    public bool isPaused = false;
    public float Delay = 5f;
    [SerializeField] private InvisibleWall thisScript;
    [SerializeField] private PauseMenuScript pauseMenuScript;

    private void Awake()
    {
        thisScript = GetComponent<InvisibleWall>();
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        wall.SetActive(false);
        if(boss != null)
        {
            boss.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerWall)
        {
            wall.SetActive(true);
            if (!isPaused)
            {
                PauseGame();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            triggerWall = true;
        }

    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        StartCoroutine(UnpauseAfterDelay());
    }
    IEnumerator UnpauseAfterDelay()
    {
        yield return new WaitForSecondsRealtime(Delay);
        Time.timeScale = 1f;
        isPaused = false;
        if (boss != null)
        {
            boss.SetActive(true);
        }
        // Reset triggerWall to prevent repeating the pause/unpause cycle
        triggerWall = false;
        yield return new WaitForSeconds(2f);
        thisScript.enabled = false;
    }
}
