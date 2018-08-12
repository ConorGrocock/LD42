using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum TileType
{
    Grey,
    Blue,
    Green,
    Orange,
    Pink
}

public class Tile : MonoBehaviour
{
    public TileType type;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}