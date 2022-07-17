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
    [SerializeField]
    Material DashedLine;
    GeneticAlgorithm ga;
    AntColonyAgorithm aco;
    List<GameObject> currentLines = new List<GameObject>();
    List<GameObject> linesToBeActive = new List<GameObject>();
    List<Color> colors = new List<Color>() {Color.red,Color.green,Color.yellow,Color.white,Color.black };
    float timer = 0;
    bool drawingPath = false;
    private void Awake()
    {
        data = new Data();
        DataReader reader = new DataReader(destinationData,distanceData);
        data.POI = reader.ReadDestination();
        data.D = reader.ReadDistance();

        aco = new AntColonyAgorithm(data, 0.7, 1, 100);
        ga = new GeneticAlgorithm(data, 100, 0.1f, 0.9f, 0.3f);
        for (int i = 0; i < data.P; i++)
        {
            Instantiate(destinationPrefab, data.POI[i].Location, Quaternion.identity);
        }
        Time.fixedDeltaTime = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if (drawingPath)
        {
            if (timer > 0.05f)
            {
                linesToBeActive[0].SetActive(true);
                linesToBeActive.RemoveAt(0);
                timer = 0f;
            }
            timer += Time.deltaTime;
            if (linesToBeActive.Count == 0)
                drawingPath = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        aco.Evolve();
        if (AntColonyAgorithm.bestSolutions.Count == 1)
        {
            GetLines(AntColonyAgorithm.bestSolutions[0], data);
        }
        else {
            if (AntColonyAgorithm.bestSolutions[AntColonyAgorithm.bestSolutions.Count-1].Equals(AntColonyAgorithm.bestSolutions[AntColonyAgorithm.bestSolutions.Count - 2]) == false) {
                foreach (var line in currentLines)
                    GameObject.Destroy(line);
                currentLines.Clear();
                GetLines(AntColonyAgorithm.bestSolutions[AntColonyAgorithm.bestSolutions.Count - 1], data);
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
                line.SetActive(false);
            }
            if (color < data.K - 1) {
                Vector3 des = data.POI[desSet[desSet.Count-1]].Location;
                Vector3 desNext = data.POI[s.gene[color+1][0]].Location;
                GameObject line = new GameObject("Line");
                LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                lineRenderer.material = DashedLine;
                lineRenderer.startWidth = 0.1f;
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
            color++;
           
        }
        drawingPath = true;
    }
    
}
