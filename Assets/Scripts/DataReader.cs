using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataReader : MonoBehaviour
{
    private void Awake()
    {
        ReadData();
    }
    void ReadData()
    {
        int numberOfLocation = 0;
        double[,] distance = new double[numberOfLocation, numberOfLocation];
        Destination[] destinations = new Destination[100];

        using (StreamReader reader = new StreamReader("Assets/DestinationCSV.csv"))
        {
            int count = -1;
            bool endOfFile = false;
            while (!endOfFile)
            {
                // leave the header
                if (count == -1) continue;
                string dataString = reader.ReadLine();
                if (count==100)
                {
                    endOfFile = true;
                    numberOfLocation = count - 1;
                    break;
                }
                var values = dataString.Split(",");
                destinations[count] = new Destination
                {
                    Id = count,
                    Location = new Vector2(float.Parse(values[0]), float.Parse(values[1])),
                    Start = float.Parse(values[2]),
                    End = float.Parse(values[3]),
                    Cost = float.Parse(values[4]),
                    Duration = float.Parse(values[5]),
                    Rating = float.Parse(values[6])
                };
                count++;
            }
        }

        using (StreamReader reader = new StreamReader("Assets/DistanceCSV.csv"))
        {
            int count = -1;
            bool endOfFile = false;
            while (!endOfFile)
            {
                // leave the header
                if (count == -1) continue;
                string dataString = reader.ReadLine();
                if (count==100)
                {
                    endOfFile = true;
                    break;
                }
                var values = dataString.Split(",");
                for (int i = 0; i < numberOfLocation; i++)
                {
                    distance[count, i] = float.Parse(values[i+1]);
                }
                count++;
            }
        }
        int counter = 0;

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
