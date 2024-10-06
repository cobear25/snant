using System.Collections;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public GameController gameController;
    public int type = 0;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == 0) {
            anim.Play("Worker");
        } else {
            anim.Play("Soldier");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToAnthill() {
        // Move to anthill
        Destroy(GetComponent<Collider2D>());
        StartCoroutine(MoveToAnthillCoroutine());
        Destroy(gameObject, 0.65f);
    }

    IEnumerator MoveToAnthillCoroutine() {
        Vector2 anthillPosition = new Vector2(0, -1);
        Vector2 direction = anthillPosition - (Vector2)transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        float speed = 8;
        float time = distance / speed;
        float startTime = Time.time;
        while (Time.time - startTime < time) {
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ant")) {
            gameController.queen.babies.Remove(gameObject);
            gameController.audioSource.PlayOneShot(gameController.crashSound);
            Destroy(gameObject);
        }
    }
}
