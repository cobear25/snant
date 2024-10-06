using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public Color[] backgroundColors;
    public Transform backgroundGrid;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public List<GameObject> enemyAnts;
    public Queen queen;
    public Transform startingAnthill;

    public GameObject backgroundSquarePrefab;
    public GameObject movingAntPrefab;
    public GameObject anthillPrefab;

    public TextMeshProUGUI gameOverReasonText;
    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip crashSound;
    public AudioClip winSound;
    public AudioClip beepSound;

    int hillsBuilt = 0;
    int enemiesAtHill = 0;
    bool isGameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
        AddEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            audioSource.mute = !audioSource.mute;
        }
    }

    void GenerateGrid() {
        for (int i = 0; i < 13; i++) {
            for (int j = 0; j < 6; j++) {
                GameObject backgroundSquare = Instantiate(backgroundSquarePrefab, new Vector3(i - 6, -j + 1, 0), Quaternion.identity);
                backgroundSquare.transform.SetParent(backgroundGrid);
                if ((i + j) % 2 == 0) {
                    backgroundSquare.GetComponent<SpriteRenderer>().color = backgroundColors[0];
                }
                else {
                    backgroundSquare.GetComponent<SpriteRenderer>().color = backgroundColors[1];
                }
            }
        }
    }

    public void GameOver() {
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void Win() {
        if (isGameOver) {
            return;
        }
        winPanel.SetActive(true);
    }

    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PickupPickedUp() {

    }

    public void EnemyReachedAnthill() {
        enemiesAtHill++;
        startingAnthill.transform.localScale = new Vector3(0.5f - (0.1f * enemiesAtHill), 0.5f - (0.1f * enemiesAtHill), 1);
        if (enemiesAtHill >= 5) {
            gameOverReasonText.text = "The enemy has destroyed your anthill!";
            GameOver();
        }
    }

    public void AddEnemy() {
        GameObject newEnemy = Instantiate(movingAntPrefab, new Vector3(7, 3, 0), Quaternion.identity);
        newEnemy.GetComponent<MovingAnt>().gameController = this;
        newEnemy.GetComponent<MovingAnt>().isEnemy = true;

        Invoke("AddEnemy", 4);
    }

    public void BuildHill(Vector3 position) {
        audioSource.PlayOneShot(winSound);
        Instantiate(anthillPrefab, position, Quaternion.identity);
        hillsBuilt++;
        if (hillsBuilt >= 12) {
            Win();
        }
    }

    public void AntDelivered(int type) {
        var newAnt = Instantiate(movingAntPrefab, new Vector3(-6.5f, 3, 0), Quaternion.identity).GetComponent<MovingAnt>();
        newAnt.gameController = this;
        newAnt.isWorker = type == 0;
    }

    public void QueenMoved() {
        foreach (GameObject enemyAnt in enemyAnts) {
            enemyAnt.GetComponent<EnemyAnt>().Move();
        }
    }
}
