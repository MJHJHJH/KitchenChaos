using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplate : MonoBehaviour
{
    public Image icon;
    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
