using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();
    }

    public void OpenNewPanel()
    {
        canvasInfo.buttonClickAudioSource.PlayOneShot(canvasInfo.buttonClickAudioSource.clip);

        if (name == "HowToPlay")
        {
            canvasInfo.isHowToPlayPanelOpen = true;
            canvasInfo.howToPlayPanelObject.SetActive(true);
        }
        else if (name == "Credits")
        {
            canvasInfo.isCreditsPanelOpen = true;
            canvasInfo.creditsPanelObject.SetActive(true);
        }

        canvasInfo.isPausePanelOpen = false;
        canvasInfo.pausePanelObject.SetActive(false);
    }
}
