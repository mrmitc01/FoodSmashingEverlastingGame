using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public int score;
    public float slowTimeFactor = 0.4f;
    public bool isTimerOn = false;
    public bool isSlowDown = false;
    public bool isPaused = false;

    private float timeLeft = 302.0f;
    private float lowGravityFactor = 4.0f;

    private TextMeshProUGUI timerText;
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        isTimerOn = true;
        Physics.gravity = new Vector3(0, Physics.gravity.y / lowGravityFactor, 0);
    }

    // Much of the timer code below came from: https://www.youtube.com/watch?v=hxpUk0qiRGs
    void Update()
    {
        if (isTimerOn && !isPaused)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.unscaledDeltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                isTimerOn = false;

                canvasInfo.gameOverStatusObject.SetActive(true);

                // Freeze game
                Time.timeScale = 0;
            }
        }
    }

    private void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = "Timer: " + formattedTime;
    }
}
