using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenExitPanel : MonoBehaviour
{
    private CanvasInfo canvasInfo;

    void Start()
    {
        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();
    }

    public void ExitMenu()
    {
        canvasInfo.buttonClickAudioSource.PlayOneShot(canvasInfo.buttonClickAudioSource.clip);

        canvasInfo.exitGameButton.interactable = false;
        canvasInfo.pauseGameButton.interactable = false;

        canvasInfo.isExitPanelOpen = true;
        canvasInfo.exitPanelObject.SetActive(true);
        canvasInfo.grayBackgroundObject.SetActive(true);
    }
}
