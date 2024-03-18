using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime;
    private bool isTimerRunning = false;
    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartStopTimer(bool start)
    {
        if (start && !isTimerRunning)
        {
            isTimerRunning = true;
            gameObject.SetActive(true);
        }
        else if (!start && isTimerRunning)
        {
            // Stop the timer
            isTimerRunning = false;
            gameObject.SetActive(false);
        }
    }
}
