using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move obstacles
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > 25)
        {
            GameObject.Destroy(gameObject);
        }
    }

    
}
