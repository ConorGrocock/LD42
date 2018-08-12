using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private GameController gc;
	public TileType type;

	public float maxHealth = 10f;
	private float health;
	public float speed = 5f;
	public float playerDistance = 4f;

	public Action<Enemy> deathCallback;
	
	// Use this for initialization
	void Start ()
	{
		gc = GameObject.Find("GameController").GetComponent<GameController>();
		deathCallback += death;
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (health <= 0) deathCallback(this);
		Vector3 position;
		position = Vector3.MoveTowards(transform.position, gc.player.transform.position, speed*Time.deltaTime);
		if(Vector3.Distance(transform.position, gc.player.transform.position) > playerDistance) 
			transform.position = position;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.transform.parent == null) return;
		if (other.gameObject.transform.parent.gameObject.GetComponent<Projectile>() == null) return;
		
		Projectile proj = other.gameObject.transform.parent.gameObject.GetComponent<Projectile>();
		float tHealth = health;
		if (proj.Type == this.type) tHealth -= (proj.damage * 1.25f);
		else tHealth -= proj.damage;
		
		if(tHealth > -(proj.damage/2f)) Destroy(proj.gameObject);

		health = tHealth;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision");
		Projectile proj = other.gameObject.transform.parent.gameObject.GetComponent<Projectile>();
		if (proj == null) return;
		float tHealth = health;
		if (proj.Type == this.type) tHealth -= (proj.damage * 1.25f);
		else tHealth -= proj.damage;
		
		if(tHealth > -(proj.damage/2f)) Destroy(proj.gameObject);

		health = tHealth;
	}

	void death(Enemy enemy)
	{
		Debug.Log("Enemy dead");
		Destroy(this.gameObject);
	}
}
