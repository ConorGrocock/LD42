using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameController gc;

    public TextMeshProUGUI AmmoType;
    public TextMeshProUGUI AmmoCount;
    public Slider playerHealth;

    private void Start()
    {
//        gc.player.OnAmmoCountChanged += PlayerAmmoCountChanged;
//        gc.player.OnAmmoTypeChanged += PlayerAmmoTypeChanged;
//        gc.player.OnDamageTaken += PlayerDamaged;
        playerHealth.maxValue = gc.player.maxHealth;
        playerHealth.value = gc.player.maxHealth;
    }

    private void PlayerDamaged(float f)
    {
        playerHealth.value = f;
    }

    private readonly String[] ammoTypeStrings = Enum.GetNames(typeof(TileType));

    private void PlayerAmmoTypeChanged(TileType tileType)
    {
        AmmoType.text = ammoTypeStrings[(int) tileType];
    }

    private void PlayerAmmoCountChanged(int newAmmoCount)
    {
        AmmoCount.text = newAmmoCount.ToString();
    }
}