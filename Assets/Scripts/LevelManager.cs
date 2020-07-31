using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    [SerializeField]
    private List<int> mazeLevelsUsed = new List<int> () { 1 };
    [SerializeField]
    private bool redKey = false;
    [SerializeField]
    private bool blueKey = false;
    [SerializeField]
    private bool greenKey = false;

    [SerializeField]
    private MazeManager mazeManager;
    [SerializeField]
    private Text pickupDialogue;

    [Header ("Prefabs")]
    [SerializeField]
    private GameObject redKeyPrefab;
    [SerializeField]
    private GameObject blueKeyPrefab;
    [SerializeField]
    private GameObject greenKeyPrefab;

    [Header ("Spawn Points")]
    [SerializeField]
    private GameObject keyspawnLevel2Parent;
    [SerializeField]
    private Transform keyspawnLevel2;
    [SerializeField]
    private Transform keyspawnLevel3Parent;
    [SerializeField]
    private Transform keyspawnLevel3;

    public void UseKey (KeyType key) {
        switch (key) {
            case KeyType.Red:
                redKey = true;
                break;
            case KeyType.Green:
                greenKey = true;
                break;
            case KeyType.Blue:
                blueKey = true;
                break;
        }

        int nextLevel = 3;
        if (mazeLevelsUsed.Count == 1) {
            nextLevel = Random.Range (2, 4);
        } else {
            if (!mazeLevelsUsed.Contains (2)) {
                nextLevel = 2;
            }
            if (!mazeLevelsUsed.Contains (3)) {
                nextLevel = 3;
            }
        }

        SpawnKey (nextLevel);
        mazeManager.SwitchMaze (nextLevel);
        mazeLevelsUsed.Add (nextLevel);
    }

    private void SpawnKey (int level) {
        Vector3 spawnPoint = new Vector3 ();

        if (level == 2) {
            spawnPoint = keyspawnLevel2.transform.position;
        }
        if (level == 3) {
            spawnPoint = keyspawnLevel3.transform.position;
        }

        Debug.Log (spawnPoint);
        Debug.Log (level);

        GameObject spawnedKey = new GameObject ();
        if (!redKey) {
            spawnedKey = Instantiate (redKeyPrefab, spawnPoint, Quaternion.identity);
        } else if (!blueKey) {
            spawnedKey = Instantiate (blueKeyPrefab, spawnPoint, Quaternion.identity);
        } else if (!greenKey) {
            spawnedKey = Instantiate (greenKeyPrefab, spawnPoint, Quaternion.identity);
        }

        if (level == 2) {
            spawnedKey.transform.parent = keyspawnLevel2Parent.transform;
        }
        if (level == 3) {
            spawnedKey.transform.parent = keyspawnLevel3Parent.transform;
        }
    }
}