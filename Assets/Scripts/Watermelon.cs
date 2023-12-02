using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Watermelon : MonoBehaviour
{
    public string associatedBarrel = "";

    private int clicksCount = 0;
    private int scoreIncrementAmount = 100;
    private int hitParticleEffectIndex = 4;
    private int hitSoundEffectIndex = 5;
    private int explosionParticleEffectIndex = 6;
    private int explosionSoundEffectIndex = 7;
    private int jumpSoundEffectIndex = 8;
    private float jumpForce = 2.2f;
    private float jumpFactor = 2.0f;
    private float startDelay = 2.0f;
    private float betweenDelay = 7.0f;
    private float minHeight = 5.3f;
    private float offScreenLocation = -20.0f;
    private float moveDelay = 0.25f;
    private float destroyDelay = 1.0f;
    private bool isInitialJump = true;
    private bool shouldJump = false;
    private bool shouldPlayJumpSound = false;
    private bool isGrounded;
    private Rigidbody foodRigidBody;
    private TextMeshProUGUI scoreText;
    private Vector3 jump;
    private Barrel barrel;
    private GameController gameController;
    private ParticleSystem explosionParticleSystem;
    private ParticleSystem hitParticleSystem;
    private AudioSource hitAudioSource;
    private AudioSource explosionAudioSource;
    private AudioSource jumpAudioSource;
    private CanvasInfo canvasInfo;
    private List<GameObject> portions = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();

        var barrelObject = GameObject.Find(associatedBarrel);
        barrel = barrelObject.GetComponent<Barrel>();

        foodRigidBody = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, jumpFactor, 0.0f);

        for (int i = 0; i < transform.childCount; i++)
        {
            portions.Add(transform.GetChild(i).gameObject);
        }

        hitParticleSystem = gameObject.transform.GetChild(hitParticleEffectIndex).gameObject.GetComponent<ParticleSystem>();
        explosionParticleSystem = gameObject.transform.GetChild(explosionParticleEffectIndex).gameObject.GetComponent<ParticleSystem>();

        hitAudioSource = gameObject.transform.GetChild(hitSoundEffectIndex).gameObject.GetComponent<AudioSource>();
        explosionAudioSource = gameObject.transform.GetChild(explosionSoundEffectIndex).gameObject.GetComponent<AudioSource>();
        jumpAudioSource = gameObject.transform.GetChild(jumpSoundEffectIndex).gameObject.GetComponent<AudioSource>();

        canvasInfo = FindObjectOfType<Canvas>().gameObject.GetComponent<CanvasInfo>();

        StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldJump && isGrounded)
        {
            foodRigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            shouldJump = false;
            shouldPlayJumpSound = true;
        }
        if (transform.position.y >= minHeight && shouldPlayJumpSound)
        {
            jumpAudioSource.PlayOneShot(jumpAudioSource.clip);
            shouldPlayJumpSound = false;
        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }
    void OnCollisionExit()
    {
        isGrounded = false;
    }

    private void OnMouseUpAsButton()
    {
        if (!canvasInfo.isPausePanelOpen && !canvasInfo.isExitPanelOpen && !canvasInfo.isHowToPlayPanelOpen && !canvasInfo.isCreditsPanelOpen)
        {
            // If it is at a certain height (i.e. it is outside the barrel),
            // then allow the player click to have an effect
            if (transform.position.y >= minHeight && gameController.isTimerOn)
            {
                clicksCount++;

                if (clicksCount == 1)
                {
                    hitParticleSystem.Play();
                    hitAudioSource.PlayOneShot(hitAudioSource.clip);

                    portions[0].SetActive(false);
                    portions[1].SetActive(true);
                }
                else if (clicksCount == 2)
                {
                    hitParticleSystem.Play();
                    hitAudioSource.PlayOneShot(hitAudioSource.clip);

                    portions[1].SetActive(false);
                    portions[2].SetActive(true);
                }
                else if (clicksCount == 3)
                {
                    hitParticleSystem.Play();
                    hitAudioSource.PlayOneShot(hitAudioSource.clip);

                    portions[2].SetActive(false);
                    portions[3].SetActive(true);
                }
                else if (clicksCount == 4)
                {
                    barrel.shouldSpawnAnother = true;
                    barrel.hasBeenDestroyedAtLeastOnce = true;

                    gameController.score += scoreIncrementAmount;
                    SetScoreText();

                    explosionParticleSystem.Play();
                    explosionAudioSource.PlayOneShot(explosionAudioSource.clip);

                    StartCoroutine(MoveAndDestroy());
                }
            }
        }
    }

    private void SetScoreText()
    {
        scoreText.text = "Score: " + gameController.score.ToString();
    }

    IEnumerator Jump()
    {
        if (isInitialJump)
        {
            isInitialJump = false;
            yield return new WaitForSeconds(startDelay);
            StartCoroutine(Jump());
        }
        else
        {
            shouldJump = true;
            yield return new WaitForSeconds(betweenDelay);
            StartCoroutine(Jump());
        }
    }

    IEnumerator MoveAndDestroy()
    {
        yield return new WaitForSeconds(moveDelay);

        Vector3 location = gameObject.transform.position;
        location.y = offScreenLocation;
        gameObject.transform.position = location;

        Destroy(gameObject, destroyDelay);
    }
}
