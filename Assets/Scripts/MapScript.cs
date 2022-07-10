using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapScript : MonoBehaviour
{
    string url = "";
    string polyurl = "";
    public float lat = 21.014502f;
    public float lon = 105.5383387f;
    string polyline = "mdg_CovacSMQ]XCV?`Ep@AFApC_Ce@s@qBsClAcAx@nA`BzBdGpI|B|CLZLd@BV?RHPLJPBVEJIFOBQEQEI?[De@v@{Cj@}Bn@aDVgBFaAEsCMqAQaA_BqGuAiEiAaFo@}EO{ABeAVkF?wBYkC?GRIPMr@oATQd@WXK";
    LocationInfo li;
    public int zoom;
    public int mapWidth;
    public int mapHeight;
    public enum mapType {roadmap, satellite, hybrid, terrain};
    public mapType mapSelected;
    public int scale;
    private IEnumerator mapCoroutine;
    private string key = "AIzaSyBs8QZJkjG-dZVHH8qkVpwavBfc-RKDjy8";


    IEnumerator GetGoogleMap()
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon +
            "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight
            + "&maptype=" + mapSelected +
            "&key=" + key;
        polyurl = "https://maps.googleapis.com/maps/api/staticmap?size=" + mapWidth + "x" + mapHeight
            + "&path=enc%3A" + polyline + "&key=" + key;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(polyurl);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
        StopCoroutine(mapCoroutine);
    }

    void Start()
    {
        //StartCoroutine(GetGoogleMap());
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;
        for (int i = 1; i > 100000; i++)
        {
            count++;
            Debug.Log(count);
        }
    }
}
