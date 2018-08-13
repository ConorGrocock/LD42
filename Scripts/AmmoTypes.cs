using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoTypes : MonoBehaviour
{
    public GameController gc;
    public TileType type;

    // Use this for initialization
    void Start()
    {
        gc.player.OnAmmoCountChanged += countChanged;
        gc.player.OnAmmoTypeChanged += typeChanged;
        TextMeshProUGUI textobj = GetComponentInChildren<TextMeshProUGUI>();
        textobj.text = "0";
        textobj.ForceMeshUpdate(true);
    }

    private void countChanged(TileType type, int newCount)
    {
        if (type == this.type) {
            TextMeshProUGUI textobj = GetComponentInChildren<TextMeshProUGUI>();
            textobj.text = newCount.ToString();
            textobj.ForceMeshUpdate(true);
        }
    }

    private void typeChanged(TileType newType)
    {
        if (newType == type) GetComponent<Outline>().effectColor = new Color(180, 64, 0);
        else GetComponent<Outline>().effectColor = new Color(31, 66, 138);
    }
}