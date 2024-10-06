using Unity.VisualScripting;
using UnityEngine;

public class MovingAnt : MonoBehaviour
{
    public bool isEnemy = false;
    public bool isWorker = false;
    public GameController gameController;
    float moveSpeed = 0.5f;
    public SpriteRenderer spriteRenderer;
    public Color redColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isEnemy) {
            spriteRenderer.color = redColor;
            spriteRenderer.flipX = true;
        } else {
            if (isWorker) {
                spriteRenderer.color = Color.black;
            } else {
                spriteRenderer.color = new Color(1, 0.5f, 0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemy) {
            transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * moveSpeed;
            if (transform.position.x <= -6.5f) {
                gameController.EnemyReachedAnthill();
                Destroy(gameObject);
            }
        } else {
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime * moveSpeed;
            if (transform.position.x >= 8) {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("HillSpot")) {
            if (isWorker) {
                gameController.BuildHill(other.transform.position);
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }

        if (other.gameObject.CompareTag("Ant")) {
            Debug.Log("Collision");
            var otherAnt = other.gameObject.GetComponent<MovingAnt>();
            if (isEnemy && otherAnt.isWorker) {
                gameController.audioSource.PlayOneShot(gameController.beepSound);
                // enemy kills worker
                Destroy(other.gameObject);
            } else if (isEnemy && !otherAnt.isWorker) {
                gameController.audioSource.PlayOneShot(gameController.beepSound);
                // enemy and soldier kill each other
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
