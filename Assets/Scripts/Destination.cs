using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public int Id { get; set; }
    public Vector2 Location { get; set; }
    public float Start { get; set; }
    public float End { get; set; }
    public float Duration { get; set; }
    public float Cost { get; set; }
    public float Rating { get; set; }
    public string Title { get; set; }
}
