using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private GameController gc;

	private float ammo = 0f;
	private float maxAmmo = 100f;
	private Dictionary<TileType, float> ammoType;
	private float maxAmmoPerType = 25f;
	private float ammoPerTile = 5f;
	
	// Use this for initialization
	void Start ()
	{
		gc = GameObject.Find("GameController").GetComponent<GameController>();
		ammoType = new Dictionary<TileType, float>();
		foreach (TileType t in Enum.GetValues(typeof(TileType)))
		{
			ammoType.Add(t, 0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0) && ammo < maxAmmo)
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			GameObject go = gc.world.getTileFromPosition(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));
			Tile tile = go.GetComponent<Tile>();
			if (ammoType[tile.type] < maxAmmoPerType)
			{
				ammoType[tile.type] += ammoPerTile;
				ammo += ammoPerTile;
				Destroy(go);
			}
		}
	}
}
