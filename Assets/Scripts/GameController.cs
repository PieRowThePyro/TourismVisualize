using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject destinationPrefab;

    List<int> desSet1 = new List<int>() { 1, 5, 6, 3, 8};
    List<int> desSet2 = new List<int>() { 9, 12, 49, 2, 78, 6, 4};

    [SerializeField]
    int desCount;
    float timer = 0;
    bool findingPath = false;
    List<GameObject> destinations = new List<GameObject>();
    List<GameObject> currentLines = new List<GameObject>();
    List<GameObject> linesToBeActive = new List<GameObject>();
    string filename;
    string filenameDis;
    // Start is called before the first frame update
    void Start()
    {
        filename = Application.dataPath + "/DestinationCSV.csv";
        filename = Application.dataPath + "/DistanceCSV.csv";
        for (int i = 0; i < desCount; i++)
        {
            float randomX = Random.Range(-8f, 8f);
            float randomY = Random.Range(-4f, 4f);
            GameObject desTemp = Instantiate(destinationPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
            destinations.Add(desTemp);
        }
        WriteToCSV();
        CalculateDistance();
        GetLines(desSet1);

    }

    // Update is called once per frame
    void Update()
    {
        if (findingPath)
        {
            if (timer > 0.5f)
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
            Vector3 des = destinations[desSet[i]].transform.position;
            Vector3 desNext = destinations[desSet[i + 1]].transform.position;
            GameObject line = new GameObject("Line");
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.blue;
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
    public void WriteToCSV()
    {
        if (destinations.Count > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("X, Y");
            tw.Close();

            tw = new StreamWriter(filename, true);
            for (int i = 0; i < destinations.Count; i++)
            {
                tw.WriteLine(destinations[i].transform.position.x + "," + destinations[i].transform.position.y);

            }
            tw.Close();
        }
    }
    public void CalculateDistance()
    {
        if (destinations.Count > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Point 1, Point 2, Distance");
            tw.Close();

            tw = new StreamWriter(filename, true);
            for (int i = 0; i < destinations.Count; i++)
            {
                for (int j = 0; j < destinations.Count; j++)
                {
                    if (i != j)
                    {
                        float dis = Vector3.Distance(destinations[i].transform.position, destinations[j].transform.position);
                        string temp = "(" + destinations[i].transform.position.x + ";" + destinations[i].transform.position.y + ")";
                        temp += "," + "(" + destinations[j].transform.position.x + ";" + destinations[j].transform.position.y + ")";
                        temp += "," + dis;
                        tw.WriteLine(temp);
                    }
                }

            }
            tw.Close();
        }
    }

}
