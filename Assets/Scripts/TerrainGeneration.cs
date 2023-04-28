using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    // variables
    private int maxTerrainCount = 40;

    private Vector3 position = new Vector3(0, 0, 7);

    private GiraffeScript GiraffeScript;
    private GameObject terrain;

    [SerializeField] private List<GameObject> terrains = new List<GameObject>();
    [SerializeField] private List<GameObject> generatedTerrains = new List<GameObject>();
    public GameObject grass1;
    public GameObject grass2;
    public GameObject water;
    public GameObject road;
    private int roadsInARow = 0;
    private int watersInARow = 0;


    // Start is called before the first frame update
    void Start()
    {
        roadsInARow = 0;
        watersInARow = 0;
        terrains = new List<GameObject>{ grass1, grass2, water, road};
        // access giraffe script
        GiraffeScript = GameObject.Find("giraffe").GetComponent<GiraffeScript>();

        // generate starting terrain
        for (int i = 0; i < 15; i++)
        {
            GenerateTerrain();
        }

    }

    // Update is called once per frame
    void Update()
    {
        // generate new terrain every move forward
        if (Input.GetKeyDown(KeyCode.W) && GiraffeScript.onGround == true)
        {
            GenerateTerrain();
        }
    }

    // instantiate new terrain
    private void GenerateTerrain()
    {
        
        terrain = Instantiate(terrains[Random.Range(0, terrains.Count)], position, Quaternion.identity);

        // grass generation chances
        if (terrain.ToString().Contains("Grass1"))
        {
            terrains = new List<GameObject> { grass2, grass2, water, road};
            roadsInARow = 0;
            watersInARow = 0;
        }
        else if (terrain.ToString().Contains("Grass2"))
        {
            terrains = new List<GameObject> { grass1, grass1, water, road};
            roadsInARow = 0;
            watersInARow = 0;
        }

        // road generation chances
        if (terrain.ToString().Contains("Road") && roadsInARow <= 3)
        {
            terrains = new List<GameObject> { grass1, grass2, water, road, road, road, road };
            roadsInARow++;
            watersInARow = 0;
        }
        else if (terrain.ToString().Contains("Road"))
        {
            terrains = new List<GameObject> { grass1, grass2, grass1, water };
            roadsInARow = 0;
            watersInARow = 0;
        }

        // water generation chances
        if (terrain.ToString().Contains("Water") && watersInARow <= 3)
        {
            terrains = new List<GameObject> { grass1, grass2, water, water, water, water, road };
            watersInARow++;
            roadsInARow = 0;
        }
        else if (terrain.ToString().Contains("Water"))
        {
            terrains = new List<GameObject> { grass1, grass2, grass1, road };
            watersInARow = 0;
            roadsInARow = 0;
        }
        position.z++;

        generatedTerrains.Add(terrain);
        if (generatedTerrains.Count > maxTerrainCount)
        {
            GameObject.Destroy(generatedTerrains[0]);
            generatedTerrains.RemoveAt(0);
        }
    }
}
