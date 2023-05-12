using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PowerupSpawn : MonoBehaviour
{
    public GameObject powerup;
    private Vector3 position;
    public int spawnChance;
    private int spawnNum;
    [SerializeField] private List<GameObject> generatedPowerups = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        for (int i = -24; i < 24; i++)
        {
            spawnNum = Random.Range(0, spawnChance);
            if (spawnNum == 0)
            {
                position.x = i + 0.5f;
                position.y = -0.637f;
                
                powerup = Instantiate(powerup, new Vector3(position.x, position.y, position.z + 3), Quaternion.identity);
                generatedPowerups.Add(powerup);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject go in generatedPowerups)
        {
            Destroy(go);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
