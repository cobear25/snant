using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    public GameObject workerPrefab;
    public GameObject enemyPrefab;
    AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddAnt();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            audioSource.mute = !audioSource.mute;
        }
    }

    void AddAnt() {
        var worker = Instantiate(workerPrefab, new Vector3(Random.Range(-9, 9), Random.Range(-6, 6), 0), Quaternion.identity);
        worker.transform.right = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        var enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-9, 9), Random.Range(-6, 6), 0), Quaternion.identity);
        Invoke("AddAnt", Random.Range(1, 3));
        enemy.transform.right = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }
}
