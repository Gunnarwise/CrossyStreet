using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GiraffeScript : MonoBehaviour
{
    // variables
    public float forwardJumpForce = 2;
    public float jumpForce = 5;
    public float gravityModifier;
    private float startTextX;
    private float startTextSpeedUp = 0.0001f;

    public bool gameOver = false;
    public bool onGround = true;
    public bool firstMove = false;

    private int Score;

    public GameObject startText;
    public GameObject keyboard;
    public GameObject canvas;
    public Text scoreText;
    public Text finalScore;
    public AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip carDeathSound;
    public AudioClip waterDeathSound;
    public ParticleSystem waterSplash;
    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        // get components
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();

        // Gravity
        Physics.gravity = new Vector3(0, -19.62f, 0);

        // reset score
        Score = 0;

        startTextX = startText.transform.position.x;
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
        if (collision.gameObject.CompareTag("Obstacle"))
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
