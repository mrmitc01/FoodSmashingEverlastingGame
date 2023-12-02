using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseExitPanel : MonoBehaviour
{
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();
    }

    public void CloseExitMenu(bool isUsingEscapeKey = false)
    {
        if (!isUsingEscapeKey)
        {
            canvasInfo.buttonClickAudioSource.PlayOneShot(canvasInfo.buttonClickAudioSource.clip);
        }

        // If pause panel is not active, set exit and pause buttons to be interactable
        if (!canvasInfo.pausePanelObject.activeInHierarchy)
        {
            canvasInfo.exitGameButton.interactable = true;
            canvasInfo.pauseGameButton.interactable = true;
        }

        canvasInfo.isExitPanelOpen = false;
        canvasInfo.exitPanelObject.SetActive(false);
        canvasInfo.grayBackgroundObject.SetActive(false);
    }
}
