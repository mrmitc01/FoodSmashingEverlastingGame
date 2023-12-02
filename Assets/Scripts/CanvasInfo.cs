using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInfo : MonoBehaviour
{
    public bool isPausePanelOpen = false;
    public bool isExitPanelOpen = false;
    public bool isHowToPlayPanelOpen = false;
    public bool isCreditsPanelOpen = false;

    public GameObject gameOverStatusObject;
    public GameObject powerUpStatusObject;
    public GameObject darkPurpleBackgroundObject;
    public GameObject pausePanelObject;
    public GameObject exitPanelObject;
    public GameObject grayBackgroundObject;
    public GameObject howToPlayPanelObject;
    public GameObject creditsPanelObject;

    public Button exitGameButton;
    public Button pauseGameButton;

    public AudioSource buttonClickAudioSource;

    public GameController gameController;

    private int resumeButtonIndex = 0;
    private int closeExitPanelButtonIndex = 2;
    private int gameOverStatusIndex = 2;
    private int powerUpStatusIndex = 3;
    private int exitGameButtonIndex = 4;
    private int pauseGameButtonIndex = 5;
    private int darkPurpleBackgroundObjectIndex = 6;
    private int pausePanelObjectIndex = 7;
    private int grayBackgroundObjectIndex = 8;
    private int exitPanelObjectIndex = 9;
    private int howToPlayPanelObjectIndex = 10;
    private int creditsPanelObjectIndex = 11;
    private int buttonClickSoundEffectIndex = 12;

    private OpenPausePanel openPausePanel;
    private ClosePausePanel closePausePanel;
    private CloseExitPanel closeExitPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameOverStatusObject = gameObject.transform.GetChild(gameOverStatusIndex).gameObject;
        gameOverStatusObject.SetActive(false);
        powerUpStatusObject = gameObject.transform.GetChild(powerUpStatusIndex).gameObject;
        powerUpStatusObject.SetActive(false);

        darkPurpleBackgroundObject = gameObject.transform.GetChild(darkPurpleBackgroundObjectIndex).gameObject;
        pausePanelObject = gameObject.transform.GetChild(pausePanelObjectIndex).gameObject;
        grayBackgroundObject = gameObject.transform.GetChild(grayBackgroundObjectIndex).gameObject;
        exitPanelObject = gameObject.transform.GetChild(exitPanelObjectIndex).gameObject;
        howToPlayPanelObject = gameObject.transform.GetChild(howToPlayPanelObjectIndex).gameObject;
        creditsPanelObject = gameObject.transform.GetChild(creditsPanelObjectIndex).gameObject;

        exitGameButton = transform.GetChild(exitGameButtonIndex).GetComponent<Button>();
        pauseGameButton = transform.GetChild(pauseGameButtonIndex).GetComponent<Button>();

        openPausePanel = pauseGameButton.GetComponent<OpenPausePanel>();
        closePausePanel = pausePanelObject.transform.GetChild(resumeButtonIndex).GetComponent<Button>().GetComponent<ClosePausePanel>();
        closeExitPanel = exitPanelObject.transform.GetChild(closeExitPanelButtonIndex).GetComponent<Button>().GetComponent<CloseExitPanel>();

        darkPurpleBackgroundObject.SetActive(false);
        pausePanelObject.SetActive(false);
        grayBackgroundObject.SetActive(false);
        exitPanelObject.SetActive(false);
        howToPlayPanelObject.SetActive(false);
        creditsPanelObject.SetActive(false);

        buttonClickAudioSource = gameObject.transform.GetChild(buttonClickSoundEffectIndex).gameObject.GetComponent<AudioSource>();

        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            bool isUsingEscapeKey = true;
            if (!isPausePanelOpen && !isExitPanelOpen && !isHowToPlayPanelOpen && !isCreditsPanelOpen)
            {
                openPausePanel.OpenPauseMenu(isUsingEscapeKey);
            }
            else if (isPausePanelOpen && !isExitPanelOpen)
            {
                closePausePanel.ClosePauseMenu(isUsingEscapeKey);
            }
            if (isExitPanelOpen)
            {
                closeExitPanel.CloseExitMenu(isUsingEscapeKey);
            }
            else if (isHowToPlayPanelOpen || isCreditsPanelOpen)
            {
                if (isHowToPlayPanelOpen)
                {
                    isHowToPlayPanelOpen = false;
                    howToPlayPanelObject.SetActive(false);
                }
                else if (isCreditsPanelOpen)
                {
                    isCreditsPanelOpen = false;
                    creditsPanelObject.SetActive(false);
                }

                exitGameButton.interactable = true;
                pauseGameButton.interactable = true;
                darkPurpleBackgroundObject.SetActive(false);

                if (gameController.isTimerOn)
                {
                    if (gameController.isSlowDown)
                    {
                        Time.timeScale = gameController.slowTimeFactor;
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                    gameController.isPaused = false;
                }
            }
        }
    }
}
