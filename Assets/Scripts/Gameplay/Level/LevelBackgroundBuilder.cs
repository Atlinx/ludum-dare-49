using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBackgroundBuilder : MonoBehaviour
{

    public GameObject background;
    
    public void CreateBackground(int sizeX, int sizeY)
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                var backgroundObj = Instantiate(background);
                backgroundObj.transform.position = new Vector3(x, y);
            }
        }
    }
}
