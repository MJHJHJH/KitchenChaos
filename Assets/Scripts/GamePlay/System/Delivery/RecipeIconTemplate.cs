using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeIconTemplate : MonoBehaviour
{
    [SerializeField] private Image icon;
    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
