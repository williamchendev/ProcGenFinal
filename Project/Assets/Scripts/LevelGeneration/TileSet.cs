using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tileset File", menuName = "LevelGeneration/Tileset", order = 1000)]
public class TileSet : ScriptableObject
{
    // Data //
    [Header("Settings")]
    public bool use_end_tiles = false;

    [Header("Assets")]
    public Sprite[] tiles = new Sprite[10];
    public Sprite[] end_tiles;

    // Methods //
    public virtual GameObject generate(Vector3 position, float width, Color color) {
        GameObject tileset_object = new GameObject(name + "_tileset_obj");
        tileset_object.transform.position = position;

        // Generate Left Side
        Vector3 temp_position = position;
        while (true) {
            if (temp_position.x <= position.x - (width / 2f) + 1) {
                if (use_end_tiles) {
                    int index = Random.Range(0, end_tiles.Length - 1);
                    GameObject new_tile = createTile(end_tiles[index], color);
                    float tile_width = new_tile.GetComponent<SpriteRenderer>().bounds.size.x;
                    temp_position.x -= tile_width;
                    new_tile.transform.position = temp_position + new Vector3(tile_width / 2f, 0f, 0f);
                    new_tile.transform.SetParent(tileset_object.transform);
                }
                break;
            }
            else {
                int index = Random.Range(0, tiles.Length - 1);
                GameObject new_tile = createTile(tiles[index], color);
                float tile_width = new_tile.GetComponent<SpriteRenderer>().bounds.size.x;
                temp_position.x -= tile_width;
                new_tile.transform.position = temp_position + new Vector3(tile_width / 2f, 0f, 0f);
                new_tile.transform.SetParent(tileset_object.transform);
            }
        }

        // Generate Right Side
        temp_position = position;
        while (true) {
            if (temp_position.x >= position.x + (width / 2f) - 1) {
                if (use_end_tiles) {
                    int index = Random.Range(0, end_tiles.Length - 1);
                    GameObject new_tile = createTile(end_tiles[index], color);
                    float tile_width = new_tile.GetComponent<SpriteRenderer>().bounds.size.x;
                    temp_position.x += tile_width;
                    new_tile.transform.position = temp_position - new Vector3(tile_width / 2f, 0f, 0f);
                    new_tile.GetComponent<SpriteRenderer>().flipX = true;
                    new_tile.transform.SetParent(tileset_object.transform);
                }
                break;
            }
            else {
                int index = Random.Range(0, tiles.Length - 1);
                GameObject new_tile = createTile(tiles[index], color);
                float tile_width = new_tile.GetComponent<SpriteRenderer>().bounds.size.x;
                temp_position.x += tile_width;
                new_tile.transform.position = temp_position - new Vector3(tile_width / 2f, 0f, 0f);
                new_tile.transform.SetParent(tileset_object.transform);
            }
        }

        return tileset_object;
    }

    public virtual GameObject createTile(Sprite tile_sprite, Color color) {
        GameObject new_object = new GameObject(name + "_tile");
        SpriteRenderer tmp_sr = new_object.AddComponent<SpriteRenderer>();
        tmp_sr.sprite = tile_sprite;
        tmp_sr.color = color;
        return new_object;
    }

}
