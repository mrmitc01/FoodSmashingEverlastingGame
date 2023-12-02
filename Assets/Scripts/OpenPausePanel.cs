using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPausePanel : MonoBehaviour
{
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();
    }

    public void OpenPauseMenu(bool isUsingEscapeKey = false)
    {
        if (!isUsingEscapeKey)
        {
            canvasInfo.buttonClickAudioSource.PlayOneShot(canvasInfo.buttonClickAudioSource.clip);
        }

        canvasInfo.exitGameButton.interactable = false;
        canvasInfo.pauseGameButton.interactable = false;

        canvasInfo.isPausePanelOpen = true;
        canvasInfo.pausePanelObject.SetActive(true);
        canvasInfo.darkPurpleBackgroundObject.SetActive(true);

        Time.timeScale = 0;
        canvasInfo.gameController.isPaused = true;
    }
}
