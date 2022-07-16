using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    TextAsset destinationData;
    [SerializeField]
    TextAsset distanceData;
    [SerializeField]
    GameObject destinationPrefab;

    List<int> desSet1 = new List<int>() { 1, 5, 6, 3, 8, 9, 12, 41, 56, 98};
    List<int> desSet2 = new List<int>() { 9, 12, 49, 2, 78, 6, 4};

    [SerializeField]
    int desCount;
    float timer = 0;
    bool findingPath = false;
    Destination[] destinations;
    List<GameObject> currentLines = new List<GameObject>();
    List<GameObject> linesToBeActive = new List<GameObject>();
    string filename;
    string filenameDis;
    [SerializeField]
    TextAsset desFile;
    [SerializeField]
    TextAsset disFile;
    int PoolSize;
    double ElitismRate;
    double MutationRate;
    double CrossoverRate;
    double Beta;
    double Alpha;
    int NumberOfAnts;
    int ProblemSize;


    private void Awake()
    {
        Data data = new Data();
        DataReader reader = new DataReader(destinationData,distanceData);
        data.POI = reader.ReadDestination();
        data.D = reader.ReadDistance();

    }

    // Start is called before the first frame update
    public void StartBtn()
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
    public void ButtonPressed()
    {
        foreach (var line in currentLines)
            GameObject.Destroy(line);
        currentLines.Clear();

        GetLines(desSet2);
    }
    /*public void WriteToCSV()
    {
        if (destinations.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("X, Y");
            tw.Close();

            tw = new StreamWriter(filename, true);
            for (int i = 0; i < destinations.Length; i++)
            {
                tw.WriteLine(destinations[i].Location.x + "," + destinations[i].Location.y);

            }
            tw.Close();
        }
    }
    public void CalculateDistance()
    {
        if (destinations.Length > 0)
        {
            
            TextWriter tw = new StreamWriter(filenameDis, false);
            string header = ",";
            for (int i = 1; i <= destinations.Length; i++)
            {
                header += i + ",";
            }
            tw.WriteLine(header);
            tw.Close();

            tw = new StreamWriter(filenameDis, true);
            for (int i = 0; i < destinations.Length; i++)
            {
                int currentI = i + 1;
                string distanceRow = currentI + ",";
                for (int j = 0; j < destinations.Length; j++)
                {
                    float dis = Vector3.Distance(destinations[i].Location, destinations[j].Location);
                    distanceRow += dis + ",";
                }
                tw.WriteLine(distanceRow);
            }
            tw.Close();
        }
    }*/

}
