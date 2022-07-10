using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataReader : MonoBehaviour
{
    public TextAsset destinationData;
    public TextAsset distanceData;

    void ReadData()
    {
        int numberOfLocation = 100;
        double[,] distance = new double[numberOfLocation, numberOfLocation];
        Destination[] destinations = new Destination[numberOfLocation];
        string[] destinationArray = destinationData.text.Split("\n");
        for (int i = 0; i < numberOfLocation; i++)
        {
            var values = destinationArray[i+1].Split(",");
            destinations[i] = new Destination
            {
                Id = i,
                Location = new Vector2(float.Parse(values[0]), float.Parse(values[1])),
                Start = float.Parse(values[2]),
                End = float.Parse(values[3]),
                Cost = float.Parse(values[4]),
                Duration = float.Parse(values[5]),
                Rating = float.Parse(values[6])
            };
        }
        
        string[] distanceArray = distanceData.text.Split("\n");
        
        for (int i = 0; i < numberOfLocation; i++)
        {
            var values = distanceArray[i + 1].Split(",");
            for (int j = 0; j < numberOfLocation; j++)
            {
                distance[i, j] = float.Parse(values[j + 1]);
            }
        }
        

    }
    private void Awake()
    {
        ReadData();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
