using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : MonoBehaviour
{
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();
    }

    public void BackOut()
    {
        canvasInfo.buttonClickAudioSource.PlayOneShot(canvasInfo.buttonClickAudioSource.clip);

        if (transform.parent.name == "HowToPlayPanel")
        {
            canvasInfo.isHowToPlayPanelOpen = false;
            canvasInfo.howToPlayPanelObject.SetActive(false);
            canvasInfo.isPausePanelOpen = true;
            canvasInfo.pausePanelObject.SetActive(true);
        }
        else if (transform.parent.name == "CreditsPanel")
        {
            canvasInfo.isCreditsPanelOpen = false;
            canvasInfo.creditsPanelObject.SetActive(false);
            canvasInfo.isPausePanelOpen = true;
            canvasInfo.pausePanelObject.SetActive(true);
        }
    }
}
