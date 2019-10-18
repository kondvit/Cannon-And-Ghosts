using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeedTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int x = 5;
        int y = 6;
        int seed = (int)System.DateTime.Now.Ticks;
        Random.InitState(seed);


        Random.InitState(seed + x);
        float a = Random.value;
        Random.InitState(seed + y);
        float b = Random.value;

        Debug.Log(a + b);

        Debug.Log(Random.value);

        Random.InitState(seed + x);
        a = Random.value;
        Random.InitState(seed + y);
        b = Random.value;

        Debug.Log(a + b);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
