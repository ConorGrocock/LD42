using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public enum Team {
	Player,
	Enemy
}

public class Enemy : MonoBehaviour
{
	private GameController gc;
	public TileType type;

	public float maxHealth = 10f;
	private float health;
	public float speed = 5f;
	public float playerDistance = 4f;
	
	public float maxShotCooldown = 0.4f;
	private float shotCooldown = 0f; 

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
		shotCooldown -= Time.deltaTime;
		if (health <= 0) deathCallback(this);
		Vector3 position;
		position = Vector3.MoveTowards(transform.position, gc.player.transform.position, speed*Time.deltaTime);
		if(Vector3.Distance(transform.position, gc.player.transform.position) > playerDistance) 
			transform.position = position;

		if (shotCooldown <= 0 && Vector3.Distance(transform.position, gc.player.transform.position) <= playerDistance)
		{
			Vector3 playerPos = gc.player.gameObject.transform.position;
			playerPos.y++;
			FireProjectile(playerPos);
		}

	}
	
	private void FireProjectile(Vector3 playerPosition)
	{
		GameObject newProjectile = Instantiate(gc.player.projectiles[(int) this.type]);
		newProjectile.transform.position = transform.position;
		newProjectile.SetActive(true);
		newProjectile.transform.parent = gc.player.projectileParent;
		Projectile proj = newProjectile.GetComponent<Projectile>();
		Vector3 direction = (playerPosition - transform.position).normalized;
		direction.z = 0;
		proj.Direction = direction;
		proj.firedBy = Team.Enemy;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.transform.parent == null) return;
		if (other.gameObject.transform.parent.gameObject.GetComponent<Projectile>() == null) return;
		
		Projectile proj = other.gameObject.transform.parent.gameObject.GetComponent<Projectile>();
		if (proj.firedBy == Team.Enemy) return;
		
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
