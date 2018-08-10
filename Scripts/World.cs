using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	private GameObject[,] worldObjects;
	public int worldWidth;
	public int worldHeight;

	// Use this for initialization
	void Start ()
	{
		worldObjects = generate(worldWidth, worldHeight, Resources.LoadAll<GameObject>("Prefabs/Tiles"));
	}

	public GameObject[,] generate(int width, int height, GameObject[] tiles)
	{
		GameObject[,] objects = new GameObject[width,height]; 
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				GameObject obj = Instantiate(tiles[Random.RandomRange(0,tiles.Length)]);
				obj.transform.position = new Vector3((width/2)-i,(height/2)-j,1);
				objects[i, j] = obj;
			}
		}

		return null;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
