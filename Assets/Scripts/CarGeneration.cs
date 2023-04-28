using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CarGeneration : MonoBehaviour
{
    private GameObject car;
    [SerializeField] public List<GameObject> cars = new List<GameObject>();
    private Vector3 position;
    private float secondsBetweenSpawn;
    public float elapsedTime = 0.0f;
    public int spawnChance;
    private int spawnNum;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        for (int i = -24; i < 19; i++)
        {
            spawnNum = Random.Range(0, spawnChance * 3);
            if (spawnNum == 0)
            {
                position.x = i;
                position.y = 0.5f;
                car = Instantiate(cars[Random.Range(0, cars.Count)], position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        secondsBetweenSpawn = Random.Range(3,50);
        elapsedTime += Time.deltaTime;
        position = transform.position;
        position.x = -25;
        position.y = 0.5f;

        if (elapsedTime > secondsBetweenSpawn)
        {
            elapsedTime = 0;

            car = Instantiate(cars[Random.Range(0, cars.Count)], position, Quaternion.identity);
        }
    }
}
