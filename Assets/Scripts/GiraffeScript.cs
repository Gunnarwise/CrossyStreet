using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class GiraffeScript : MonoBehaviour
{
    // variables
    public float forwardJumpForce = 2;
    public float jumpForce = 5;
    public float gravityModifier;
    private float startTextX;
    private float highScoreTextX;
    private float startTextSpeedUp = 0.0001f;
    public float highScore = SaveHighScore.highScore;

    public bool gameOver = false;
    public bool onGround = true;
    public bool firstMove = false;
    public bool hasPowerup;

    public int Score;

    public GameObject startText;
    public GameObject highScoreText;
    public List<TextMeshPro> highScoreTexts = new List<TextMeshPro>();
    public GameObject keyboard;
    public GameObject canvas;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScore;
    public AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip carDeathSound;
    public AudioClip waterDeathSound;
    public ParticleSystem waterSplash;
    private Rigidbody playerRb;
    private MeshCollider playerMesh;
    public Material giraffeMaterial;

    

    // Start is called before the first frame update
    void Start()
    {
        // get components
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        playerMesh = GetComponent<MeshCollider>();

        // Gravity
        Physics.gravity = new Vector3(0, -19.62f, 0);

        // reset score
        Score = 0;

        startTextX = startText.transform.position.x;
        highScoreTextX = highScoreText.transform.position.x;

        // changing all highscore texts to current highscore
        for (int i = 0; i < highScoreTexts.Count; i++)
        {
            highScoreTexts[i].text = $"High Score: {SaveHighScore.highScore}";
        }

        giraffeMaterial.SetColor("_EmissionColor", Color.black);

    }

    // powerup countdown
    IEnumerator powerupCountdownRoutine()
    {
        giraffeMaterial.SetColor("_EmissionColor", new Color(0f, 96f, 138f));


        yield return new WaitForSeconds(10);

        giraffeMaterial.SetColor("_EmissionColor", Color.black);

        hasPowerup = false;
        

    }
    // Update is called once per frame
    void Update()
    {

        // player movements
        if (onGround == true && gameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {

                Score = Convert.ToInt16(transform.position.z);
                onGround = false;
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                playerRb.AddForce(Vector3.forward * forwardJumpForce, ForceMode.Impulse);
                transform.rotation = Quaternion.LookRotation(Vector3.right);
                scoreText.text = Score.ToString();
                firstMove = true;

            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                onGround = false;
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                playerRb.AddForce(Vector3.left * forwardJumpForce, ForceMode.Impulse);
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
                firstMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                onGround = false;
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                playerRb.AddForce(Vector3.right * forwardJumpForce, ForceMode.Impulse);
                transform.rotation = Quaternion.LookRotation(Vector3.back);
                firstMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                onGround = false;
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                playerRb.AddForce(Vector3.back * forwardJumpForce, ForceMode.Impulse);
                transform.rotation = Quaternion.LookRotation(Vector3.left);
                firstMove = true;
            }

            
            
        }
       

        // saving high score to static class
        if (gameOver == true && (Score > SaveHighScore.highScore))
        {
            highScore = Score;
            SaveHighScore.highScore = Score;
        }
        
        if (transform.position.y < -3)
        {
            gameOver = true;
            // show final score
            scoreText.text = "";
            finalScore.text = $"Score:{Score}";
            // game over screen
            canvas.SetActive(true);
        }
        
        // get rid of starting text
        if (firstMove && startText != null)
        {
            startTextX = startTextX + startTextSpeedUp;
            startTextSpeedUp = startTextSpeedUp + 0.02f;
            startText.transform.position = new Vector3 (startTextX, startText.transform.position.y, startText.transform.position.z);

          
            keyboard.transform.position = new Vector3 (keyboard.transform.position.x, keyboard.transform.position.y - 0.05f, keyboard.transform.position.z);    
        }
        if (firstMove && highScoreText != null)
        {
            highScoreTextX = highScoreTextX + startTextSpeedUp;
            startTextSpeedUp = startTextSpeedUp + 0.02f;
            highScoreText.transform.position = new Vector3(highScoreTextX, highScoreText.transform.position.y, highScoreText.transform.position.z);
        }
    }

    // when player starts colliding
    private void OnCollisionEnter(Collision collision)
    {
        // making sure player on ground
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Log") || collision.gameObject.CompareTag("Tree"))
        {
            onGround = true;
            
        }
        

    }

    // whole time player is colliding
    private void OnCollisionStay(Collision collision)
    {
        // moving player with logs
        if (collision.gameObject.CompareTag("Log"))
        {
            if (transform.rotation.y == 1){
                transform.Translate(Vector3.left * 1 * Time.deltaTime);
                }
            else if (transform.rotation.y < 0){
                transform.Translate(Vector3.back * 1 * Time.deltaTime);
                }
            else if (transform.rotation.y == 0){
                transform.Translate(Vector3.right * 1 * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.forward * 1 * Time.deltaTime);
            }
           
        }
        if (collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            
        }
    }

    // when trigger starts
    private void OnTriggerEnter(Collider collision)
    {

        // game over for car death
        if (collision.gameObject.CompareTag("Obstacle") && !hasPowerup)
        {
            
            gameOver = true;

            // show final score
            scoreText.text = "";
            finalScore.text = $"Score:{Score}";

            // player splat
            transform.rotation = Quaternion.Euler(new Vector3(90, transform.rotation.y, -90));
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            transform.localScale = new Vector3(0.25f, 0.25f, 0.0005f);

            // game over screen
            canvas.SetActive(true);

            // death sound
            playerAudio.PlayOneShot(carDeathSound, 1);
        }

        // game over for water death
        if (collision.gameObject.CompareTag("WaterObstacle"))
        {
            gameOver = true;

            // show final score
            scoreText.text = "";
            finalScore.text = $"Score:{Score}";

            // player sinks
            playerRb.AddForce(-Vector3.forward * forwardJumpForce, ForceMode.Impulse);
            transform.gameObject.GetComponent<MeshCollider>().enabled = false;

            // game over screen
            canvas.SetActive(true);

            // death particles and sound
            waterSplash.Play();
            playerAudio.PlayOneShot(waterDeathSound, 1);
        }

        // collect powerup
        if (collision.CompareTag("Powerup"))
        {
            Destroy(collision.gameObject);
            hasPowerup = true;
      
            StartCoroutine(powerupCountdownRoutine());
        }
    }

    // player leaves collision
    private void OnCollisionExit(Collision collision)
    {
        // jump sound
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Log"))
        {
            playerAudio.PlayOneShot(jumpSound, 1);
            onGround = false;

        }
        
    }

}


// static class to keep high score through restarts
static class SaveHighScore
{
    public static float highScore = 0;



}
