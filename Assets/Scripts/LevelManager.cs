using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    [SerializeField]
    private bool usedFirstKey = false;
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
    [SerializeField]
    private Enemy enemyScript;
    [SerializeField]
    private bool gamePaused;

    [Header ("Prefabs")]
    [SerializeField]
    private GameObject enemy;
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

    [Header ("Relations")]
    [SerializeField]
    private BasicCharacter player;
    [SerializeField]
    private AudioSource ambienceSource;
    [SerializeField]
    private AudioSource chaseSource;
    [SerializeField]
    private GameObject pauseObject;

    void Start () {
        player = GameObject.FindWithTag ("Player").GetComponent<BasicCharacter> ();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void PauseGame () {
        gamePaused = true;
        Time.timeScale = 0;
        pauseObject.SetActive (true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void ResumeGame () {
        gamePaused = false;
        Time.timeScale = 1;
        pauseObject.SetActive (false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (gamePaused) {
                ResumeGame ();
            } else {
                PauseGame ();
            }
        }
        if (redKey && greenKey && blueKey) {
            Win ();
        }
        if (player.PlayerIsDead ()) {
            GameOver ();
        }

        if (enemyScript == null) {
            var newEnemy = GameObject.FindWithTag ("Enemy");
            if (newEnemy != null) {
                enemyScript = newEnemy.GetComponent<Enemy> ();
            }
        } else {
            if (enemyScript.currentState == EnemyState.Chase && !chaseSource.isPlaying) {
                PlayChaseMusic ();
            }
            if (enemyScript.currentState == EnemyState.Patrol && !ambienceSource.isPlaying) {
                PlayAmbienceMusic ();
            }
        }
    }

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

        SpawnEnemy (nextLevel);
    }

    private void SpawnKey (int level) {
        Vector3 spawnPoint = new Vector3 ();

        if (level == 2) {
            spawnPoint = keyspawnLevel2.transform.position;
        }
        if (level == 3) {
            spawnPoint = keyspawnLevel3.transform.position;
        }

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

    public void Quit () {
        SceneManager.LoadScene ("MainMenu");
    }

    private void GameOver () {
        SceneManager.LoadScene ("GameOver");
    }

    private void Win () {
        SceneManager.LoadScene ("WinScreen");
    }

    private void RestartGame () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SpawnEnemy (int level) {
        GameObject oldEnemy = GameObject.FindWithTag ("Enemy");
        if (oldEnemy != null) {
            Destroy (oldEnemy);
        }

        Vector3 spawnPoint = new Vector3 ();

        if (level == 2) {
            spawnPoint = keyspawnLevel2.transform.position;
        }
        if (level == 3) {
            spawnPoint = keyspawnLevel3.transform.position;
        }

        Instantiate (enemy, spawnPoint, Quaternion.identity);
    }

    public void PlayChaseMusic () {
        chaseSource.Play ();
        ambienceSource.Stop ();
    }
    public void PlayAmbienceMusic () {
        ambienceSource.Play ();
        chaseSource.Stop ();
    }

}