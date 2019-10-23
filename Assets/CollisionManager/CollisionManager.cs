using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private GameObject[] collidables;

    void Start()
    {
        collidables = GameObject.FindGameObjectsWithTag("Rigid");
    }

    void Update()
    {
        for (int i = 0; i < collidables.Length - 1; i++)
        {
            for(int j = i+1; j < collidables.Length; j++)
            {
                ResolveCollision(collidables[i], collidables[j]);
            }
        }
    }

    private void ResolveCollision(GameObject gameObject1, GameObject gameObject2)
    {
        LineRenderer object1Renderer = gameObject1.GetComponent<LineRenderer>(); 
        LineRenderer object2Renderer = gameObject2.GetComponent<LineRenderer>();


        //TODO: need to skip rock rock and ghost ghost
        //TODO:
        //use convexHall of the ball to resolve collision between rock and ball
        if (object1Renderer) // ball X stone collision
        {

        }
        else if (object2Renderer) // ball X stone collision
        {

        }
        else //ball X ball collision
        {

        }

    }
}
