using UnityEngine;

public class EnemyAnt : MonoBehaviour
{
    public int direction = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move() {
        if (direction == 1) {
            transform.position += new Vector3(1, 0, 0);
            if (transform.position.x > 6) {
                transform.position = new Vector3(-6, transform.position.y, 0);
            }
        } else if (direction == 2) {
            transform.position += new Vector3(-1, 0, 0);
            if (transform.position.x < -6) {
                transform.position = new Vector3(6, transform.position.y, 0);
            }
        } else if (direction == 3) {
            transform.position += new Vector3(0, 1, 0);
            if (transform.position.y > 1) {
                transform.position = new Vector3(transform.position.x, -4, 0);
            }
        } else if (direction == 4) {
            transform.position += new Vector3(0, -1, 0);
            if (transform.position.y < -4) {
                transform.position = new Vector3(transform.position.x, 1, 0);
            }
        }
        int prevDirection = direction;
        while (true) {
            direction = Random.Range(1, 5);
            if (prevDirection == 1 && direction == 2) {
                continue;
            } else if (prevDirection == 2 && direction == 1) {
                continue;
            } else if (prevDirection == 3 && direction == 4) {
                continue;
            } else if (prevDirection == 4 && direction == 3) {
                continue;
            } else {
                break;
            }
        }
        if (direction == 1) {
            transform.right = new Vector3(1, 0, 0);
        } else if (direction == 2) {
            transform.right = new Vector3(-1, 0, 0);
        } else if (direction == 3) {
            transform.right = new Vector3(0, 1, 0);
        } else if (direction == 4) {
            transform.right = new Vector3(0, -1, 0);
        }
    }
}
