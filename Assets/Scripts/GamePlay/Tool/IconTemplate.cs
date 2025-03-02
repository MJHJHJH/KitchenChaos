using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GFrame;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplate : MonoBehaviour
{
    public DynamicImage icon;
    public void SetIcon(int spriteID)
    {
        // icon.sprite = sprite;
        DRTextures dRTextures = DataHelper.GetDataRowByID<DRTextures>(spriteID);
        DRAsset dRAsset = DataHelper.GetDataRowByID<DRAsset>(dRTextures.AssetID);
        icon.SetImage(dRAsset.AssetPath);
    }
}
