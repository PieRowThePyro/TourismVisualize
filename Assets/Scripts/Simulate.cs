using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulate : MonoBehaviour
{   [SerializeField]
    TextAsset destinationData;
    [SerializeField]
     TextAsset distanceData;
    Data data;
    [SerializeField]
    GameObject destinationPrefab;
    GeneticAlgorithm ga;
    AntColonyAgorithm aco;
    List<GameObject> currentLines = new List<GameObject>();
    List<GameObject> linesToBeActive = new List<GameObject>();
    List<Color> colors = new List<Color>() {Color.red,Color.green,Color.yellow,Color.white,Color.black };
    float timer = 0;
    bool findingPath = false;
    private void Awake()
    {
        data = new Data();
        DataReader reader = new DataReader(destinationData,distanceData);
        data.POI = reader.ReadDestination();
        data.D = reader.ReadDistance();

        aco = new AntColonyAgorithm(data, 0.7, 1, 100);
        ga = new GeneticAlgorithm(data, 100, 0.1f, 0.9f, 0.3f);

    }
    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        ga.Evolve();
        if (GeneticAlgorithm.bestSolutions.Count == 1)
        {
            GetLines(GeneticAlgorithm.bestSolutions[0], data);
        }
        else {
            if (GeneticAlgorithm.bestSolutions[GeneticAlgorithm.bestSolutions.Count-1].Equals(GeneticAlgorithm.bestSolutions[GeneticAlgorithm.bestSolutions.Count - 2]) == false) {
                foreach (var line in currentLines)
                    GameObject.Destroy(line);
                currentLines.Clear();
                GetLines(GeneticAlgorithm.bestSolutions[GeneticAlgorithm.bestSolutions.Count - 1], data);
            }
        }
        
    }
    public void GetLines(Solution s, Data data)
    {
        int color = 0;
        foreach (List<int> desSet in s.gene)
        {
            for (int i = 0; i < desSet.Count - 1; i++)
            {
                Vector3 des = data.POI[desSet[i]].Location;
                Vector3 desNext = data.POI[desSet[i + 1]].Location;
                GameObject line = new GameObject("Line");
                LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                lineRenderer.startColor = colors[color];
                lineRenderer.endColor = colors[color];
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.05f;
                lineRenderer.positionCount = 2;
                lineRenderer.useWorldSpace = true;
                //For drawing line in the world space, provide the x,y,z values
                lineRenderer.SetPosition(0, des); //x,y and z position of the starting point of the line
                lineRenderer.SetPosition(1, desNext); //x,y and z position of the end point of the line
                currentLines.Add(line);
                linesToBeActive.Add(line);
                line.SetActive(true);
            }
            color++;
           
        }
        findingPath = true;
    }
    
}
