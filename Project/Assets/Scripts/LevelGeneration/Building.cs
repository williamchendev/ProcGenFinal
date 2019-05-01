using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building File", menuName = "LevelGeneration/Building", order = 500)]
public class Building : ScriptableObject
{

    public virtual void generate(float min, float max) {
        
    }

}
