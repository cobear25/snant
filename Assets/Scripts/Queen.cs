using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Queen : MonoBehaviour
{
    public GameController gameController;
    public List<GameObject> babies;
    public List<Vector2> previousPositions;
    public GameObject babyPrefab;
    public GameObject pickupPrefab;
    List<GameObject> pickups = new List<GameObject>();
    public Color[] pickupColors;
    
    float lastMovedTime = 0;
    int moveCount = 0;
    bool moving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPositions = new List<Vector2>
        {
            transform.position
        };
        AddPickup(0);
        AddPickup(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            transform.position += new Vector3(-1, 0, 0);
            // StartCoroutine(MoveLeftCoroutine());
            if (transform.position.x < -6) {
                transform.position = new Vector3(6, transform.position.y, 0);
            }
            transform.right = new Vector3(-1, 0, 0);
            Moved();
        } else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Time.time - lastMovedTime > 0.4f) {
            transform.position += new Vector3(-1, 0, 0);
            // StartCoroutine(MoveLeftCoroutine());
            if (transform.position.x < -6) {
                transform.position = new Vector3(6, transform.position.y, 0);
            }
            transform.right = new Vector3(-1, 0, 0);
            Moved();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            transform.position += new Vector3(1, 0, 0); 
            // StartCoroutine(MoveRightCoroutine());
            if (transform.position.x > 6) {
                transform.position = new Vector3(-6, transform.position.y, 0);
            }
            transform.right = new Vector3(1, 0, 0);
            Moved();
        } else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Time.time - lastMovedTime > 0.4f) {
            transform.position += new Vector3(1, 0, 0); 
            // StartCoroutine(MoveRightCoroutine());
            if (transform.position.x > 6) {
                transform.position = new Vector3(-6, transform.position.y, 0);
            }
            transform.right = new Vector3(1, 0, 0);
            Moved();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            transform.position += new Vector3(0, 1, 0); 
            // StartCoroutine(MoveUpCoroutine());
            if (transform.position.y > 1) {
                transform.position = new Vector3(transform.position.x, -4, 0);
            }
            transform.right = new Vector3(0, 1, 0);
            Moved();
        } else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Time.time - lastMovedTime > 0.4f) {
            transform.position += new Vector3(0, 1, 0); 
            // StartCoroutine(MoveUpCoroutine());
            if (transform.position.y > 1) {
                transform.position = new Vector3(transform.position.x, -4, 0);
            }
            transform.right = new Vector3(0, 1, 0);
            Moved();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            transform.position += new Vector3(0, -1, 0);
            // StartCoroutine(MoveDownCoroutine());
            if (transform.position.y < -4) {
                transform.position = new Vector3(transform.position.x, 1, 0);
            }
            transform.right = new Vector3(0, -1, 0);
            Moved();
        } else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Time.time - lastMovedTime > 0.4f) {
            transform.position += new Vector3(0, -1, 0);
            // StartCoroutine(MoveDownCoroutine());
            if (transform.position.y < -4) {
                transform.position = new Vector3(transform.position.x, 1, 0);
            }
            transform.right = new Vector3(0, -1, 0);
            Moved();
        }
    }

    IEnumerator MoveRightCoroutine() {
        moving = true;
        for (int i = 0; i < 10; i++) {
            transform.position += new Vector3(0.1f, 0, 0);
            yield return new WaitForSeconds(0.015f);
        }
        moving = false;
    }

    IEnumerator MoveLeftCoroutine() {
        moving = true;
        for (int i = 0; i < 10; i++) {
            transform.position += new Vector3(-0.1f, 0, 0);
            yield return new WaitForSeconds(0.015f);
        }
        moving = false;
    }

    IEnumerator MoveUpCoroutine() {
        moving = true;
        for (int i = 0; i < 10; i++) {
            transform.position += new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(0.015f);
        }
        moving = false;
    }   

    IEnumerator MoveDownCoroutine() {
        moving = true;
        for (int i = 0; i < 10; i++) {
            transform.position += new Vector3(0, -0.1f, 0);
            yield return new WaitForSeconds(0.015f);
        }
        moving = false;
    }

    void Moved() {
        moveCount++;
        lastMovedTime = Time.time;
        previousPositions.Add(transform.position);
        UpdateBabyPositions();
        moveCount++;
        if (Vector2.Distance(transform.position, new Vector2(0, -1)) < 0.5f) {
            List<int> types = new List<int>();
            foreach (var baby in babies) {
                baby.GetComponent<Ant>().MoveToAnthill();
                types.Add(baby.GetComponent<Ant>().type);
            }
            babies.Clear();
            StartCoroutine(DeliverBabies(types));
        }
        gameController.QueenMoved();
    }

    IEnumerator DeliverBabies(List<int> types) {
        for (int i = 0; i < types.Count; i++) {
            yield return new WaitForSeconds(0.5f);
            gameController.AntDelivered(types[i]);
        }
    }

    void UpdateBabyPositions() {
        for (int i = 0; i < babies.Count; i++) {
            int babyIndex = previousPositions.Count - 2 - i;
            babies[i].transform.position = previousPositions[babyIndex];
            if (i == 0) {
                babies[i].transform.right = (Vector2)transform.position - previousPositions[babyIndex];
            }
            else {
                babies[i].transform.right = previousPositions[babyIndex + 1] - previousPositions[babyIndex];
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Pickup") {
            gameController.audioSource.PlayOneShot(gameController.pickupSound);
            pickups.Remove(other.gameObject);
            Destroy(other.gameObject);
            var baby = Instantiate(babyPrefab);
            baby.GetComponent<Ant>().gameController = gameController;
            baby.GetComponent<Ant>().type = other.GetComponent<Pickup>().type;
            babies.Add(baby);
            UpdateBabyPositions();
            AddPickup(other.GetComponent<Pickup>().type);
        }
        if (other.gameObject.tag == "Ant") {
            gameController.audioSource.PlayOneShot(gameController.crashSound);
            gameController.GameOver();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void AddPickup(int type) {
        var pickup = Instantiate(pickupPrefab);
        pickup.GetComponent<Pickup>().type = type;
        pickup.GetComponentInChildren<SpriteRenderer>().color = pickupColors[type];
        while (true) {
            bool canPlace = true;
            Vector2 position = new Vector2(Random.Range(-6, 6), Random.Range(-4, 1));
            foreach (var baby in babies) {
                if (Vector2.Distance(baby.transform.position, position) < 1) {
                    canPlace = false;
                    break;
                }
            }
            foreach (var p in pickups)
            {
                if (Vector2.Distance(p.transform.position, position) < 1)
                {
                    canPlace = false;
                    break;
                }
            }
            foreach (var e in gameController.enemyAnts)
            {
                if (Vector2.Distance(e.transform.position, position) < 1)
                {
                    canPlace = false;
                    break;
                }
            }
            if (Vector2.Distance(transform.position, position) < 1) {
                canPlace = false;
            }
            if (Vector2.Distance(new Vector2(0, -1), position) < 1) {
                canPlace = false;
            }
            if (canPlace) {
                pickup.transform.position = position;
                pickups.Add(pickup);
                break;
            }
        }
    }
}
