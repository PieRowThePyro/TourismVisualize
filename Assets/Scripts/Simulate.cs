using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulate : MonoBehaviour
{   [SerializeField]
    TextAsset destinationData;
    [SerializeField]
     TextAsset distanceData;
    Data data;
    GeneticAlgorithm ga;
    AntColonyAgorithm aco;
    private void Awake()
    {
        data = new Data();
        DataReader reader = new DataReader(destinationData,distanceData);
        data.POI = reader.ReadDestination();
        data.D = reader.ReadDistance();
        aco = new AntColonyAgorithm(data, 0.5, 1, 200);
        ga = new GeneticAlgorithm(data, 100, 0.1f, 0.9f, 0.1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aco.Evolve();
    }
}
