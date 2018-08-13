using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private GameController gc;

    public float maxHealth = 10f;
    private float health;
    private float maxAmmo = 150f;
    private Dictionary<TileType, float> ammoType;
    private float maxAmmoPerType = 100f;
    private float ammoPerTile = 5f;

    private TileType chosenProjectile = TileType.Grey;
    public GameObject[] projectiles;
    public Transform projectileParent;

    public float maxShotCooldown = 0.2f;
    private float shotCooldown = 0f;

    public float speed = 0.1f;

    public Slider ammoSlider;
    public TextMeshProUGUI ammoRemainingText;

    private float lives = 3;
    public float maxLives = 3;

    public GameObject livesPanel;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;
    public Slider healthSlider;

    private Image[] livesImages;

    private List<TileType> seenTypes;

    public Unlock unlockScript;
    public float unlockUIOpenTime = 0.5f;

    //    public Action<int> OnAmmoCountChanged;
    //    public Action<TileType> OnAmmoTypeChanged;
    //    public Action<float> OnDamageTaken;
    public Action OnDeath;

//	Use this for initialization
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        ammoType = new Dictionary<TileType, float>();
        health = maxHealth;

        foreach (TileType t in Enum.GetValues(typeof(TileType)))
        {
            ammoType.Add(t, 0f);
        }
        
        OnDeath += death;

        livesImages = livesPanel.GetComponentsInChildren<Image>();
        seenTypes = new List<TileType>();
    }

    GameObject getTile(Vector3 position, float offsetX, float offsetY)
    {
        return gc.world.getTileFromPosition(Mathf.RoundToInt(position.x + offsetX),
            Mathf.RoundToInt(position.y + offsetY));
    }

    Rect getRectangle(GameObject go)
    {
        Vector3 pos = go.transform.position;
        Bounds bounds = go.GetComponentInChildren<SpriteRenderer>().bounds;
        return new Rect(pos.x, pos.y, bounds.size.x, bounds.size.y);
    }

    //Returns the new position
    Vector3 checkCollision(Vector3 currentPosition, float padding, Vector3 oldPosition)
    {
        GameObject tile;
        tile = getTile(oldPosition, 0, 0);
        if (tile == null || !tile.active)
            return
                oldPosition +
                Vector3.right; //Util.getNearestPointInPerimeter(getRectangle(tile), oldPosition.x, oldPosition.y);
        tile = getTile(currentPosition, 0, 0);
        if (tile == null || !tile.active)
            return oldPosition;
        tile = getTile(currentPosition, padding, 0);
        if (tile == null || !tile.active)
        {
            oldPosition.y = currentPosition.y;
            return oldPosition;
        }

        tile = getTile(currentPosition, -padding, 0);
        if (tile == null || !tile.active)
        {
            oldPosition.y = currentPosition.y;
            return oldPosition;
        }

        tile = getTile(currentPosition, 0, padding);
        if (tile == null || !tile.active)
        {
            oldPosition.x = currentPosition.x;
            return oldPosition;
        }

        tile = getTile(currentPosition, 0, -padding);
        if (tile == null || !tile.active)
        {
            oldPosition.x = currentPosition.x;
            return oldPosition;
        }

        return currentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        shotCooldown -= Time.deltaTime;

        var oldPos = transform.position;
        var position = transform.position;
        float axisHorizontal = Input.GetAxisRaw("Horizontal");
        float axisVertical = Input.GetAxisRaw("Vertical");
        position.x += axisHorizontal * speed * Time.deltaTime;
        position.y += axisVertical * speed * Time.deltaTime;

        transform.position = checkCollision(position, 0.3f, oldPos);

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (shotCooldown <= 0 && ammoType[chosenProjectile] > 0)
                FireProjectile(mousePosition, chosenProjectile);
        }

        if (Input.GetMouseButton(1))
        {
            MineTile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        for (int i = 0; i < livesImages.Length; i++)
        {
            livesImages[i].sprite = (lives - i >= 0) ? fullHeartSprite : emptyHeartSprite;
        }

        healthSlider.value = health / maxHealth;

        if (Input.mouseScrollDelta.y != 0)
        {
            chosenProjectile += (int) Input.mouseScrollDelta.y;
            if ((int) chosenProjectile >= Enum.GetValues(typeof(TileType)).Length) chosenProjectile = 0;
            if (chosenProjectile < 0) chosenProjectile = (TileType) Enum.GetValues(typeof(TileType)).Length - 1;
            
            //OnAmmoTypeChanged(chosenProjectile);
        }
    }

    private void MineTile(Vector3 mousePosition)
    {
        GameObject go =
            gc.world.getTileFromPosition(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));
        Tile tile = go.GetComponent<Tile>();
        if (ammoType[tile.type] + ammoPerTile <= maxAmmoPerType && tile.type == chosenProjectile)
        {
            ammoType[tile.type] += ammoPerTile;
            go.SetActive(false);

            //if(tile.type == chosenProjectile) OnAmmoCountChanged((int) ammoType[chosenProjectile]);

            if (!seenTypes.Contains(tile.type))
            {
                seenTypes.Add(tile.type);
                unlockScript.OpenUnlockUI(unlockUIOpenTime, tile.typeName, tile.tileSprite);
            }
        }
    }

    private void FireProjectile(Vector3 mousePosition, TileType projectileType)
    {
        GameObject newProjectile = Instantiate(projectiles[(int) projectileType]);
        newProjectile.transform.position = transform.position;
        newProjectile.SetActive(true);
        newProjectile.transform.parent = projectileParent;
        Projectile proj = newProjectile.GetComponent<Projectile>();
        Vector3 direction = (mousePosition - transform.position).normalized;
        direction.z = 0;
        proj.Direction = direction;
        proj.firedBy = Team.Player;

        ammoType[projectileType]--;
        shotCooldown = maxShotCooldown;
        //OnAmmoCountChanged((int) ammoType[projectileType]);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.parent == null) return;
        if (other.gameObject.transform.parent.gameObject.GetComponent<Projectile>() == null) return;

        Projectile proj = other.gameObject.transform.parent.gameObject.GetComponent<Projectile>();
        if (proj.firedBy == Team.Player) return;

        health = health - proj.damage;

        Destroy(proj.gameObject);
        //OnDamageTaken(health);
        if (health <= 0)
        {
            lives--;

            if (lives == 0)
                OnDeath();
            else
                health = maxHealth;
        }
    }

    void death()
    {
        Time.timeScale = 0;
    }
}