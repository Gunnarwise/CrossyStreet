using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public GiraffeScript GiraffeScript;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && GiraffeScript.gameOver == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            canvas.SetActive(false);
        }
    }
}
