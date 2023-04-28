using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMove : MonoBehaviour
{
    public float speed;
    private Rigidbody logRb;
    public CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        logRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        
        if (transform.position.x > 25)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
