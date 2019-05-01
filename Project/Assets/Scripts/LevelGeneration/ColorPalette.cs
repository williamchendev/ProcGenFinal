using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette File", menuName = "LevelGeneration/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    // Enums //
    public enum LightingMode {
        day,
        night
    }

    // Data //
    [Header("Colors")]
    public Color forground = Color.white;
    public Color characters = Color.white;
    public Color interactables = Color.white;
    public Color decorations = Color.white;
    public Color buildings = Color.white;

    [Header("Background")]
    public bool generate_palette = true;
    public Color[] manual_palette = { Color.white, Color.white, Color.white, Color.white, Color.white };

    [Header("Additional Settings")]
    public bool manual_lighting = false;
    public LightingMode light_mode = LightingMode.night;
    public Color lit_color = Color.white;
    public Color unlit_color = new Color(0.5f, 0.5f, 0.5f, 1f);

    //public Color[] additional_colors = { Color.white, Color.white, Color.white, Color.white, Color.white };

}
