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
    public string typeName;
    public Sprite tileSprite;
    public Color ammoColour;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}