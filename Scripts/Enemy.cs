using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public TileType type;

	public float maxHealth = 10f;
	private float health;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(health < 0) Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		Projectile proj = other.gameObject.transform.parent.gameObject.GetComponent<Projectile>();
		if (proj == null) return;
		float tHealth = health;
		if (proj.Type == this.type) tHealth -= (proj.damage * 1.25f);
		else tHealth -= proj.damage;
		
		if(tHealth > -(proj.damage/2f)) Destroy(proj.gameObject);

		health = tHealth;
	}
}
