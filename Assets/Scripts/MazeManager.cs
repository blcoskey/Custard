using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public const float MAZE_1_Y = -40;
    public const float MAZE_2_Y = -20;
    public const float MAZE_3_Y = -0;
    public int currentMaze = 1;

    public Transform mazeTransform;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchMaze(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchMaze(2);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchMaze(3);
        }
    }

    public void SwitchMaze(int mazeNumber){
        var newPosition = mazeTransform.position;

        switch(mazeNumber){
            case 1: newPosition.y = MAZE_1_Y; break;
            case 2: newPosition.y = MAZE_2_Y; break;
            default: newPosition.y = MAZE_3_Y; break;
        }

        mazeTransform.position = newPosition;
    }
}
