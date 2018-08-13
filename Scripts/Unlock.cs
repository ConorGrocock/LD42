using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    public List<GameObject> uiParts;

    // TODO: Use this
    public void OpenUnlockUI(float openTime, string projectileName, Sprite displaySprite)
    {
        foreach (GameObject uiPart in uiParts)
        {
            uiPart.GetComponent<UIFill>().Open(openTime, projectileName, displaySprite);
        }
    }
}