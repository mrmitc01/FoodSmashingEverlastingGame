using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();
    }

    // Update is called once per frame
    public void CloseCurrentPanel()
    {
        canvasInfo.buttonClickAudioSource.PlayOneShot(canvasInfo.buttonClickAudioSource.clip);
        canvasInfo.exitGameButton.interactable = true;
        canvasInfo.pauseGameButton.interactable = true;
        canvasInfo.darkPurpleBackgroundObject.SetActive(false);

        if (transform.parent.name == "HowToPlayPanel")
        {
            canvasInfo.isHowToPlayPanelOpen = false;
            canvasInfo.howToPlayPanelObject.SetActive(false);
        }
        else if (transform.parent.name == "CreditsPanel")
        {
            canvasInfo.isCreditsPanelOpen = false;
            canvasInfo.creditsPanelObject.SetActive(false);
        }

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
