using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevel : MonoBehaviour
{

    public LevelGen generator = null;

    // Start is called before the first frame update
    void Awake()
    {
        generator.generateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
