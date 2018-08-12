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

    private float ammo = 0f;
    private float maxAmmo = 150f;
    private Dictionary<TileType, float> ammoType;
    private float maxAmmoPerType = 25f;
    private float ammoPerTile = 5f;

    private TileType chosenProjectile = TileType.Grey;
    public GameObject[] projectiles;
    public Transform projectileParent;

    public float maxShotCooldown = 0.2f;
    private float shotCooldown = 0f;

    public float speed = 0.1f;

    public TextMeshProUGUI ammoTypeFont;
    public TextMeshProUGUI[] curremtAmmoFont;

    public Slider ammoSlider;
    public TextMeshProUGUI ammoRemainingText;

    private int lives = 3;
    public GameObject livesPanel;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;
    private Image[] livesImages;
    private List<TileType> seenTypes;

    public Unlock unlockScript;
    public float unlockUIOpenTime = 0.5f;

//	Use this for initialization
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        ammoType = new Dictionary<TileType, float>();
        foreach (TileType t in Enum.GetValues(typeof(TileType)))
        {
            ammoType.Add(t, 0f);
        }
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
            if (shotCooldown <= 0 && ammoType[chosenProjectile] > 0 && ammo > 0)
                FireProjectile(mousePosition, chosenProjectile);
        }

        ammoSlider.value = ammo / maxAmmo;
        ammoRemainingText.text = ammo + " / " + maxAmmo;

        for (int i = 0; i < livesImages.Length; i++)
        {
            livesImages[i].sprite = (lives - i >= 0) ? fullHeartSprite : emptyHeartSprite;
        }

        if (Input.GetMouseButton(1) && ammo < maxAmmo)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject go =
                gc.world.getTileFromPosition(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));
            Tile tile = go.GetComponent<Tile>();
            if (tile.type != TileType.Grey && ammoType[tile.type] < maxAmmoPerType)
            {
                ammoType[tile.type] += ammoPerTile;
                ammo += ammoPerTile;
                go.SetActive(false);
                
                if (!seenTypes.Contains(tile.type))
                {
                    seenTypes.Add(tile.type);
                    unlockScript.OpenUnlockUI(unlockUIOpenTime, tile.typeName, tile.tileSprite);
                }
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            chosenProjectile += (int) Input.mouseScrollDelta.y;
            if ((int) chosenProjectile >= Enum.GetValues(typeof(TileType)).Length) chosenProjectile = 0;
            if (chosenProjectile < 0) chosenProjectile = (TileType) Enum.GetValues(typeof(TileType)).Length - 1;
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
        ammo--;
        shotCooldown = maxShotCooldown;
    }
}