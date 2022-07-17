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
    Material DashedLine;
    GeneticAlgorithm ga;
    AntColonyAgorithm aco;

    [SerializeField]
    int desCount;
    float timer = 0;
    Destination[] destinations;
    List<GameObject> currentLines = new List<GameObject>();
    List<GameObject> linesToBeActive = new List<GameObject>();
    public static StrategyManager manager;
    List<Color> colors = new List<Color>() { Color.red, Color.green, Color.yellow, Color.white, Color.black };

    public static int PoolSize;
    public static float ElitismRate;
    public static float MutationRate;
    public static float CrossoverRate;
    public static float Beta;
    public static float Alpha;
    public static int NumberOfAnts;
    public static int ProblemSize;
    public static bool IsGenetic = true;
    public static Data FullData;
    public static Data RealData;

    public bool drawingPath = false;
    public bool isStarted = false;
    public bool isGenerated = false;
    private void Awake()
    {
        FullData = new Data();
        DataReader reader = new DataReader(destinationData, distanceData);
        FullData.POI = reader.ReadDestination();
        FullData.D = reader.ReadDistance();
        manager = new StrategyManager();
        //aco = new AntColonyAgorithm(data, 0.7, 1, 100);
        //ga = new GeneticAlgorithm(data, 100, 0.1f, 0.9f, 0.3f);
        //for (int i = 0; i < data.P; i++)
        //{
        //    Instantiate(destinationPrefab, data.POI[i].Location, Quaternion.identity);
        //}
        Time.fixedDeltaTime = 1f;
    }


    // Update is called once per frame
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
    void FixedUpdate()
    {
        if (isGenerated)
            if (isStarted)
            {
                manager.DoAlgorithm();
                if (!drawingPath)
                {
                    if (StrategyManager.BestSolutions.Count == 1)
                    {
                        GetLines(StrategyManager.BestSolutions[0], RealData);
                    }
                    else
                    {
                        if (StrategyManager.BestSolutions[StrategyManager.BestSolutions.Count - 1].Equals(StrategyManager.BestSolutions[StrategyManager.BestSolutions.Count - 2]) == false)
                        {
                            foreach (var line in currentLines)
                                GameObject.Destroy(line);
                            currentLines.Clear();
                            GetLines(StrategyManager.BestSolutions[StrategyManager.BestSolutions.Count - 1], RealData);
                        }
                    }
                }
            }
            
    }

    public void ChangeAlgorithm()
    {
        
        if (IsGenetic)
        {
            PoolSize = int.Parse(panel.gameObject.transform.Find("PoolSizeNumber").gameObject.GetComponent<TextMeshProUGUI>().text);
            ElitismRate = float.Parse(panel.gameObject.transform.Find("ElitismNumber").gameObject.GetComponent<TextMeshProUGUI>().text);
            MutationRate = float.Parse(panel.gameObject.transform.Find("MutationRateNumber").gameObject.GetComponent<TextMeshProUGUI>().text);
            CrossoverRate = float.Parse(panel.gameObject.transform.Find("CrossoverRateNumber").gameObject.GetComponent<TextMeshProUGUI>().text);

            manager.SetStrategy(new GeneticAlgorithm(RealData, PoolSize, ElitismRate, CrossoverRate, MutationRate));
        } else
        {
            Beta = float.Parse(panel.gameObject.transform.Find("BetaNumber").gameObject.GetComponent<TextMeshProUGUI>().text);
            Alpha = float.Parse(panel.gameObject.transform.Find("AlphaNumber").gameObject.GetComponent<TextMeshProUGUI>().text);
            NumberOfAnts = int.Parse(panel.gameObject.transform.Find("NumberOfAntsNumber").gameObject.GetComponent<TextMeshProUGUI>().text);

            manager.SetStrategy(new AntColonyAgorithm(RealData, Alpha, Beta, NumberOfAnts));
        }
        IsGenetic = !IsGenetic;

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
            if (color < data.K - 1)
            {
                Vector3 des = data.POI[desSet[desSet.Count - 1]].Location;
                Vector3 desNext = data.POI[s.gene[color + 1][0]].Location;
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
