﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private GameController gc;

	private float ammo = 0f;
	private float maxAmmo = 150f;
	private Dictionary<TileType, float> ammoType;
	private float maxAmmoPerType = 25f;
	private float ammoPerTile = 5f;
	
	private TileType chosenProjectile = TileType.Grey;
	public GameObject projectile;
	public Transform projectileParent;
	
	
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
		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			FireProjectile(mousePosition, chosenProjectile);
		}

		if (Input.GetMouseButton(1) && ammo < maxAmmo)
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

		if (Input.mouseScrollDelta.y != 0)
		{
			chosenProjectile += (int)Input.mouseScrollDelta.y;
			if ((int)chosenProjectile >= Enum.GetValues(typeof(TileType)).Length) chosenProjectile = 0;
			if (chosenProjectile < 0) chosenProjectile = (TileType)Enum.GetValues(typeof(TileType)).Length - 1;
		}
	}

	private void FireProjectile(Vector3 mousePosition, TileType tileType)
	{
		GameObject newProjectile = Instantiate(projectile);
		newProjectile.transform.position = transform.position + transform.up *1f;
		newProjectile.SetActive(true);
		newProjectile.transform.parent = projectileParent;
		Projectile proj = newProjectile.GetComponent<Projectile>();
		proj.Type = tileType;
		Vector3 direction = (mousePosition - transform.position).normalized;
		direction.z = 0;
		proj.Direction = direction;

		ammoType[tileType]--;
		ammo--;
	}
}
