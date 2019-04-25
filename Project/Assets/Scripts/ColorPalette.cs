using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette", menuName = "ColorPalette")]
public class ColorPalette : ScriptableObject
{

    [Header("Colors")]
    public Color[] colors = { Color.white, Color.white, Color.white, Color.white, Color.white };

}
