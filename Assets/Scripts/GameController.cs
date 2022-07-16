using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    TextAsset destinationData;
    [SerializeField]
    TextAsset distanceData;
    [SerializeField]
    GameObject destinationPrefab;
    [SerializeField]
    GameObject panel;
   
    [SerializeField]
    int desCount;
    float timer = 0;
    bool findingPath = false;
    Destination[] destinations;
    List<GameObject> currentLines = new List<GameObject>();
    List<GameObject> linesToBeActive = new List<GameObject>();
   
    public static int PoolSize;
    public static double ElitismRate;
    public static double MutationRate;
    public static double CrossoverRate;
    public static double Beta;
    public static double Alpha;
    public static int NumberOfAnts;
    public static int ProblemSize;
    public static bool IsGenetic = true;
    public static Data FullData;
    public static Data RealData;
    
    private void Awake()
    {
        FullData = new Data();
        DataReader reader = new DataReader(destinationData,distanceData);
        FullData.POI = reader.ReadDestination();
        FullData.D = reader.ReadDistance();

    }

    // Start is called before the first frame update
    public void StartBtn()
    {

    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (findingPath)
        {
            if (timer > 0.3f)
            {
                linesToBeActive[0].SetActive(true);
                linesToBeActive.RemoveAt(0);
                timer = 0f;
            }
            timer += Time.deltaTime;
            if (linesToBeActive.Count == 0)
                findingPath = false;
        }
    }
    public void GetLines(List<int> desSet)
    {
        for (int i = 0; i < desSet.Count - 1; i++)
        {
            Vector3 des = destinations[desSet[i]].Location;
            Vector3 desNext = destinations[desSet[i + 1]].Location;
            GameObject line = new GameObject("Line");
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            //For drawing line in the world space, provide the x,y,z values
            lineRenderer.SetPosition(0, des); //x,y and z position of the starting point of the line
            lineRenderer.SetPosition(1, desNext); //x,y and z position of the end point of the line
            currentLines.Add(line);
            linesToBeActive.Add(line);
            line.SetActive(false);
        }
        findingPath = true;
    }
    

}
