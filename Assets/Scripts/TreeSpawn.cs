using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawn : MonoBehaviour
{
    public GameObject tree;
    private Vector3 position;
    public int spawnChance;
    private int spawnNum;
    [SerializeField] private List<GameObject> generatedTrees = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        for (int i = -24; i < 24; i++)
        {
            spawnNum = Random.Range(0, spawnChance);
            if (spawnNum == 0)
            {
                position.x = i;
                position.y = 0.48f;
                tree = Instantiate(tree, position, Quaternion.identity);
                generatedTrees.Add(tree);
            }
        }
    }
    private void OnDestroy()
    {
        foreach (GameObject go in generatedTrees)
        {
            Destroy(go);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
