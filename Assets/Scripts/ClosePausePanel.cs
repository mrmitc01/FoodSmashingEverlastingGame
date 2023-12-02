using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePausePanel : MonoBehaviour
{
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();
    }

    public void ClosePauseMenu(bool isUsingEscapeKey = false)
    {
        if (!isUsingEscapeKey)
        {
            canvasInfo.buttonClickAudioSource.PlayOneShot(canvasInfo.buttonClickAudioSource.clip);
        }

        canvasInfo.exitGameButton.interactable = true;
        canvasInfo.pauseGameButton.interactable = true;

        canvasInfo.isPausePanelOpen = false;
        canvasInfo.darkPurpleBackgroundObject.SetActive(false);
        canvasInfo.pausePanelObject.SetActive(false);

        if (canvasInfo.gameController.isTimerOn)
        {
            if (canvasInfo.gameController.isSlowDown)
            {
                Time.timeScale = canvasInfo.gameController.slowTimeFactor;
            }
            else
            {
                Time.timeScale = 1;
            }
            canvasInfo.gameController.isPaused = false;
        }
    }
}
