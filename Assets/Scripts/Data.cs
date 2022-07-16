using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public static float MAX_HAPPINESS = 884.0000004f;
    public static float MIN_HAPPINESS = 171.3f;

    public static float MAX_NUMBER_OF_DESTINATION = 28;
    public static float MIN_NUMBER_OF_DESTINATION = 0;

    public static float MAX_WATING_TIME = 43191.16f;
    public static float MIN_WATING_TIME = 0;

    public static float MAX_DISTANCE = 43191.16f;
    public static float MIN_DISTANCE = 0;

    public static float MAX_BUDGET = 100000;
    public static float MIN_BUDGET = 0;

    public int P; // Number destinations;
    public int C;
    public Destination[] POI = new Destination[550]; // Destination
    public float[,] D = new float[550,550]; // distance between each destination
    public float[,] tourist = new float[550,550];//Tourist Preference
    public float S; // Cost per km
    public int K; // Number of Trip
    public float[] T_max = new float[550];// Max time for each trip
    public float[] C_max = new float[550];// Max budget for each trip
    public float[] t_s = new float[550]; // start of service each trip
    public float[] t_e = new float[550]; // end of service each trip
    public float w1;
    public float w2;
    public float w3;
    public float w4;
    public float w5;

    public Data(Data data, List<int> destinations) {
        //Viet kieu deo gi day?
        for (int i = 0; i < destinations.Count; i++) {
            this.POI[i] = data.POI[destinations[i]];
            for (int j = 0; j < destinations.Count; j++) {
                this.D[i, j] = data.D[destinations[i], destinations[j]];
            }
        }
        K = 2;
        P = destinations.Count;
        this.C_max[0] = 5000000f;
        this.C_max[1] = 5000000f;
        this.C_max[2] = 5000000f;
        this.C_max[3] = 5000000f;
        this.C_max[4] = 5000000f;
        this.T_max[0] = 73800f;
        this.T_max[1] = 73800f;
        this.T_max[2] = 73800f;
        this.T_max[3] = 73800f;
        this.T_max[4] = 73800f;
        this.t_s[0] = 27000;
        this.t_s[1] = 27000;
        this.t_s[2] = 27000;
        this.t_s[3] = 27000;
        this.t_s[4] = 27000;
        this.t_e[0] = 75600;
        this.t_e[1] = 75600;
        this.t_e[2] = 75600;
        this.t_e[3] = 75600;
        this.t_e[4] = 75600;
        this.w1 = 1;
        this.w2 = 1;
        this.w3 = 1;
        this.w4 = 1;
        this.w5 = 1;
    }
    public Data()
    {
        this.P = 100;
        this.K = 2;
        this.S = 15000;
        this.C = 10;
        this.C_max[0] = 5000000f;
        this.C_max[1] = 5000000f;
        this.C_max[2] = 5000000f;
        this.C_max[3] = 5000000f;
        this.C_max[4] = 5000000f;
        this.T_max[0] = 73800f;
        this.T_max[1] = 73800f;
        this.T_max[2] = 73800f;
        this.T_max[3] = 73800f;
        this.T_max[4] = 73800f;
        this.t_s[0] = 27000;
        this.t_s[1] = 27000;
        this.t_s[2] = 27000;
        this.t_s[3] = 27000;
        this.t_s[4] = 27000;
        this.t_e[0] = 75600;
        this.t_e[1] = 75600;
        this.t_e[2] = 75600;
        this.t_e[3] = 75600;
        this.t_e[4] = 75600;
        this.w1 = 1;
        this.w2 = 1;
        this.w3 = 1;
        this.w4 = 1;
        this.w5 = 1;
    }

    
}
