using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Rural Generation File", menuName = "LevelGeneration/DefaultRuralGenerator", order = 100)]
public class DefaultRuralGenerator : Generator
{
    // Data //
    [Header("Assets")]
    public TileSet forground_tileset;
    public TileSet decorations_tileset;

    [Header("Settings")]
    public LayerMask solids;

    // Methods //
    public override void generate(LevelGen generator_file)
    {
        // Color Palette
        ColorPalette colors = generator_file.level_colors;
        if (generator_file.random_colors) {
            colors = generator_file.random_palette[(int) Random.Range(0, generator_file.random_palette.Length - 1)];
        }

        // Create Bounds of the Level
        int level_size = generator_file.level_size;
        int buildings = generator_file.buildings;
        if (generator_file.random_settings) {
            level_size = Random.Range(generator_file.min_level_size, generator_file.max_level_size);
            buildings = Random.Range(generator_file.min_buildings, generator_file.max_buildings);
        }

        GameObject bounds = new GameObject("Level_Bounds");
        GameObject ground = emptySquare();
        ground.layer = LayerMask.NameToLayer("Solids");
        ground.GetComponent<SpriteRenderer>().color = colors.forground;
        ground.transform.localScale = new Vector3(level_size, 5f, 1f);
        ground.transform.position = new Vector3(level_size / 2f, -2.5f, -3f);
        ground.transform.SetParent(bounds.transform);
        GameObject leftwall = emptySquare();
        leftwall.layer = LayerMask.NameToLayer("Solids");
        leftwall.GetComponent<SpriteRenderer>().color = colors.forground;
        leftwall.transform.localScale = new Vector3(5f, 100f, 1f);
        leftwall.transform.position = new Vector3(-2.5f, 45f, -3f);
        leftwall.transform.SetParent(bounds.transform);
        GameObject rightwall = emptySquare();
        rightwall.layer = LayerMask.NameToLayer("Solids");
        rightwall.GetComponent<SpriteRenderer>().color = colors.forground;
        rightwall.transform.localScale = new Vector3(5f, 100f, 1f);
        rightwall.transform.position = new Vector3(level_size + 2.5f, 45f, -3f);
        rightwall.transform.SetParent(bounds.transform);

        // Set Camera Bounds
        if (Camera.main.GetComponent<CameraBehaviour>() != null) {
            Camera.main.GetComponent<CameraBehaviour>().clamp_camera = true;
            Camera.main.GetComponent<CameraBehaviour>().clamp_bounds = new Vector2(0, level_size);
        }

        // Create Forground
        if (forground_tileset != null) {
            forground_tileset.generate(new Vector3(level_size / 2f, 0.5f, -3.01f), level_size + 2f, colors.forground);
        }
        if (decorations_tileset != null) {
            decorations_tileset.generate(new Vector3(level_size / 2f, 0.5f, 6.0f), level_size + 2f, colors.decorations);
        }
        
    }

    // This Method Generates an Empty White Square (sized 1x1 in unity units) with a box collider
    public GameObject emptySquare() {
        GameObject tmp_square = new GameObject("empty_square");
        SpriteRenderer tmp_sr = tmp_square.AddComponent<SpriteRenderer>();

        Texture2D tex = new Texture2D(2, 2);
        Color[] colors_array = { Color.white, Color.white, Color.white, Color.white };
        tex.SetPixels(colors_array);
        tex.Apply();
        Sprite stex = Sprite.Create(tex, new Rect(new Vector2(0f, 0f), new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.5f), 2f);
        tmp_sr.sprite = stex;
        tmp_sr.size = new Vector2(1f, 1f);

        tmp_square.AddComponent<BoxCollider2D>();

        return tmp_square;
    }

}
