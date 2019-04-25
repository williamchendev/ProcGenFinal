using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneBehaviour : MonoBehaviour
{

    // Cutscene Blocks
    public List<CutsceneBlockBehaviour> cutscene_blocks;

    // Variables
    private int cutscene_index;

    // Start Event
    void Start()
    {
        cutscene_index = 0;
        if (cutscene_blocks.Count > 0) {
            cutscene_blocks[0].gameObject.SetActive(true);
        }
        else {
            endCutscene();
        }
    }

    // Late Update Event
    void LateUpdate()
    {
        if (cutscene_index < cutscene_blocks.Count) {
            if (cutscene_blocks[cutscene_index].finished) {
                cutscene_index++;
                if (cutscene_index < cutscene_blocks.Count) {
                    cutscene_blocks[cutscene_index].gameObject.SetActive(true);
                }
            }
        }
        else {
            // End Cutscene
            endCutscene();
        }
    }

    // Misc Methods
    private void endCutscene() {
        foreach (CutsceneBlockBehaviour block in cutscene_blocks) {
            Destroy(block.gameObject);
        }
        Destroy(gameObject);
        Application.Quit();
    }
}
