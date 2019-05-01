using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Foilage Tileset File", menuName = "LevelGeneration/FoilageTileset", order = 1001)]
public class FoilageTileSet : TileSet
{

    // Data //
    [Header("Foilage Settings")]
    public bool use_this_tileset = false;
    public int min_divisions = 1;
    public int max_divisions = 1;
    public int div_offset = 3;

    [Header("Foilage Tilesets")]
    public TileSet[] tilesets;

    [Header("Foilage Assets")]
    public bool sparse_forest = false;
    public bool forest = false;
    public Sprite[] trees;


    // Methods //
    public override GameObject generate(Vector3 position, float width, Color color) {
        GameObject tileset_object = new GameObject(name + "_foilage_obj");

        if (use_this_tileset) {
            GameObject default_tiles = base.generate(position, width, color);
            default_tiles.transform.SetParent(tileset_object.transform);
        }

        for (int i = 0; i < tilesets.Length; i++) {
            FoilageDivLeaves new_set = new FoilageDivLeaves(position.x, width);
            int divisions = Random.Range(min_divisions, max_divisions);
            for (int q = 0; q < divisions; q++) {
                new_set.split(div_offset);
            }

            List<Vector2> foilage_data = new_set.generateData(new List<Vector2>());
            for (int k = 0; k < foilage_data.Count; k++) {
                if (k % 2 == 0) {
                    GameObject new_tileset = tilesets[i].generate(new Vector3(foilage_data[k].x, position.y, position.z), foilage_data[k].y, color);
                    new_tileset.transform.SetParent(tileset_object.transform);
                    if (Random.Range(0, 10) < 5) {
                        GameObject new_tree = createTile(trees[Random.Range(0, trees.Length - 1)], color);
                        new_tree.transform.position = new Vector3(foilage_data[k].x + Random.Range(-(foilage_data[k].y / 2f), (foilage_data[k].y / 2f)), position.y - 0.5f, position.z);
                        new_tree.transform.position = new Vector3(Mathf.RoundToInt(new_tree.transform.position.x * 48f) / 48f, Mathf.RoundToInt(new_tree.transform.position.y * 48f) / 48f, position.z);
                        new_tree.transform.SetParent(tileset_object.transform);
                    }
                }
            }
        }

        if (forest) {
            int last_tree = -1;
            for (float i = -0.1f; i <= width + 0.1f; i += 1f) {
                float tree_position_x = position.x + (i - (width / 2f));

                int current_tree = Random.Range(0, trees.Length - 1);
                while (current_tree == last_tree) {
                    current_tree = Random.Range(0, trees.Length - 1);
                }
                GameObject new_tree = createTile(trees[current_tree], color);
                new_tree.transform.position = new Vector3(tree_position_x + Random.Range(-1f, 1f), position.y - 0.5f, position.z);
                new_tree.transform.position = new Vector3(Mathf.RoundToInt(new_tree.transform.position.x * 48f) / 48f, Mathf.RoundToInt(new_tree.transform.position.y * 48f) / 48f, position.z);
                new_tree.transform.SetParent(tileset_object.transform);
                if (Random.Range(0, 10) < 5) {
                    new_tree.GetComponent<SpriteRenderer>().flipX = true;
                }
                last_tree = current_tree;
            }
        }

        return tileset_object;
    }

}

public class FoilageDivLeaves {

    public FoilageDivLeaves left_leaf;
    public FoilageDivLeaves right_leaf;

    public float x_pos;
    public float width;
    public bool is_split;

    public FoilageDivLeaves(float new_x_pos, float new_width) {
        is_split = false;
        x_pos = new_x_pos;
        width = new_width;

        left_leaf = null;
        right_leaf = null;
    }

    public bool split() {
        if (!is_split) {
            is_split = true;
            float left_width = (width / 2f) + (Random.Range(-(width / 6f), (width / 6f)));
            float right_width = width - left_width;
            left_leaf = new FoilageDivLeaves(x_pos - (right_width / 2f), left_width);
            right_leaf = new FoilageDivLeaves(x_pos + (left_width / 2f), right_width);
            return true;
        }
        else {
            int random_index = Random.Range(0, 10);
            if (random_index < 5) {
                if (left_leaf != null) {
                    return left_leaf.split();
                }
            }
            else {
                if (right_leaf != null) {
                    return right_leaf.split();
                }
            }
        }
        return false;
    }

    public bool split(int offset) {
        if (!is_split) {
            is_split = true;
            float left_width = (width / 2f) + (Random.Range(-(width / offset), (width / offset)));
            float right_width = width - left_width;
            left_leaf = new FoilageDivLeaves(x_pos - (right_width / 2f), left_width);
            right_leaf = new FoilageDivLeaves(x_pos + (left_width / 2f), right_width);
            return true;
        }
        else {
            int random_index = Random.Range(0, 10);
            if (random_index < 5) {
                if (left_leaf != null) {
                    return left_leaf.split(offset);
                }
            }
            else {
                if (right_leaf != null) {
                    return right_leaf.split(offset);
                }
            }
        }
        return false;
    }

    public List<Vector2> generateData(List<Vector2> new_array) {
        if (is_split) {
            if (left_leaf != null) {
                new_array = left_leaf.generateData(new_array);
            }
            if (right_leaf != null) {
                new_array = right_leaf.generateData(new_array);
            }
        }
        else {
            new_array.Add(new Vector3(x_pos, width));
            return new_array;
        }
        return new_array;
    }

}