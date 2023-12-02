using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodItem : MonoBehaviour
{
    public string associatedBarrel = "";

    private int scoreIncrementAmount = 25;
    private int hitParticleEffectIndex = 4;
    private int hitSoundEffectIndex = 5;
    private int jumpSoundEffectIndex = 6;
    private float jumpForce = 2.2f;
    private float jumpFactor = 2.0f;
    private float startDelay = 2.0f;
    private float betweenDelay = 7.0f;
    private float minHeight = 5.3f;
    private float moveDelay = 0.25f;
    private float destroyDelay = 1.0f;
    private float offScreenLocation = -20.0f;
    private bool isInitialJump = true;
    private bool shouldJump = false;
    private bool isClicked = false;
    private bool shouldPlayJumpSound = false;
    private bool isGrounded;
    private Rigidbody foodRigidBody;
    private TextMeshProUGUI scoreText;
    private Vector3 jump;
    private Barrel barrel;
    private GameController gameController;
    private ParticleSystem hitParticleSystem;
    private AudioSource hitAudioSource;
    private AudioSource jumpAudioSource;
    private CanvasInfo canvasInfo;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();

        var barrelObject = GameObject.Find(associatedBarrel);
        barrel = barrelObject.GetComponent<Barrel>();

        foodRigidBody = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, jumpFactor, 0.0f);

        hitParticleSystem = gameObject.transform.GetChild(hitParticleEffectIndex).gameObject.GetComponent<ParticleSystem>();
        hitAudioSource = gameObject.transform.GetChild(hitSoundEffectIndex).gameObject.GetComponent<AudioSource>();
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
            if (transform.position.y >= minHeight && gameController.isTimerOn && !isClicked)
            {
                isClicked = true;
                barrel.shouldSpawnAnother = true;
                barrel.hasBeenDestroyedAtLeastOnce = true;

                gameController.score += scoreIncrementAmount;
                SetScoreText();

                hitParticleSystem.Play();
                hitAudioSource.PlayOneShot(hitAudioSource.clip);
                StartCoroutine(MoveAndDestroy());
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