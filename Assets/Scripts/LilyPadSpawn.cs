using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LilyPadSpawn : MonoBehaviour
{
    public GameObject lilypad;
    public GameObject log;
    private Vector3 position;
    public int spawnChance;
    private int spawnNum;
    private int liyOrLog;
    private float secondsBetweenSpawn;
    public float elapsedTime = 0.0f;
    [SerializeField] private List<GameObject> generatedLilypads = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        liyOrLog = Random.Range(0, 2);
        if (liyOrLog == 0)
        {
            position = transform.position;
            for (int i = -24; i < 24; i++)
            {
                spawnNum = Random.Range(0, spawnChance);
                if (spawnNum == 0)
                {
                    position.x = i;
                    position.y = 0.48f;
                    lilypad = Instantiate(lilypad, position, Quaternion.identity);
                    lilypad.transform.rotation = new Quaternion(0, Random.Range(0, 3) * 90, 0, 0);
                    generatedLilypads.Add(lilypad);
                }
            }
        }
        else
        {
            position = transform.position;
            for (int i = -24; i < 17; i++)
            {
                spawnNum = Random.Range(0, spawnChance * 2);
                if (spawnNum == 0)
                {
                    position.x = i;
                    position.y = 0.48f;
                    log = Instantiate(log, position, new Quaternion(90, -90, 0, 0));
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject go in generatedLilypads)
        {
            Destroy(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (liyOrLog == 1)
        {
            secondsBetweenSpawn = Random.Range(5, 25);
            elapsedTime += Time.deltaTime;
            position = transform.position;
            position.x = -25;
            position.y = 0.3f;

            if (elapsedTime > secondsBetweenSpawn)
            {
                elapsedTime = 0;

                log = Instantiate(log, position, new Quaternion(90, -90, 0, 0));
            }
        }
    }
}
