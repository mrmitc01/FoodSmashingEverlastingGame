using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string associatedBarrel = "";

    private int hitParticleEffectIndex = 5;
    private int hitSoundEffectIndex = 6;
    private int durationSoundEffectIndex = 7;
    private int jumpSoundEffectIndex = 8;
    private float slowDownTimer = 10.0f;
    private float offScreenLocation = -20.0f;
    private float startDelay = 30.0f;
    private float betweenDelay = 15.0f;
    private float jumpForce = 2.4f;
    private float jumpFactor = 2.0f;
    private float minHeight = 5.3f;
    private float moveDelay = 0.25f;
    private bool isInitialJump = true;
    private bool shouldJump = false;
    private bool isClicked = false;
    private bool shouldPlayJumpSound = false;
    private bool isGrounded;
    private Rigidbody foodRigidBody;
    private PowerUpBarrel powerUpBarrel;
    private GameController gameController;
    private Vector3 jump;
    private ParticleSystem hitParticleSystem;
    private AudioSource hitAudioSource;
    private AudioSource durationAudioSource;
    private AudioSource jumpAudioSource;
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        var powerUpBarrelObject = GameObject.Find(associatedBarrel);
        powerUpBarrel = powerUpBarrelObject.GetComponent<PowerUpBarrel>();

        foodRigidBody = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, jumpFactor, 0.0f);

        hitParticleSystem = gameObject.transform.GetChild(hitParticleEffectIndex).gameObject.GetComponent<ParticleSystem>();
        hitAudioSource = gameObject.transform.GetChild(hitSoundEffectIndex).gameObject.GetComponent<AudioSource>();
        durationAudioSource = gameObject.transform.GetChild(durationSoundEffectIndex).gameObject.GetComponent<AudioSource>();
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
            if (transform.position.y >= minHeight && gameController.isTimerOn && !isClicked)
            {
                isClicked = true;

                // Show Power Up status
                canvasInfo.powerUpStatusObject.SetActive(true);

                hitParticleSystem.Play();
                hitAudioSource.PlayOneShot(hitAudioSource.clip);

                durationAudioSource.Play();

                // Make object disappear by moving it off screen
                StartCoroutine(MovePowerUp());
            }
        }
    }

    IEnumerator SetTimeScaleBackToNormal()
    {
        yield return new WaitForSeconds(slowDownTimer * gameController.slowTimeFactor);
        if (gameController.isTimerOn)
        {
            Time.timeScale = 1.0f;
            gameController.isSlowDown = false;

            powerUpBarrel.shouldSpawnAnother = true;
            powerUpBarrel.hasBeenDestroyedAtLeastOnce = true;

            // Hide Power Up status
            canvasInfo.powerUpStatusObject.SetActive(false);

            durationAudioSource.Stop();

            Destroy(gameObject);
        }
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

    IEnumerator MovePowerUp()
    {
        yield return new WaitForSeconds(moveDelay);

        Vector3 location = gameObject.transform.position;
        location.y = offScreenLocation;
        gameObject.transform.position = location;

        Time.timeScale = gameController.slowTimeFactor;
        gameController.isSlowDown = true;
        StartCoroutine(SetTimeScaleBackToNormal());
    }
}
