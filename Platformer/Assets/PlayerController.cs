using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D collider;
    public float SpeedMultiplier = 5;
    public float JumpForce = 10;

    public bool Dead = false;
    private Vector2 _spawnPos;

    public int Lives = 3;

    public TMP_Text LifeCounter;
    public GameObject GameOverScreen;



    // Start is called before the first frame update
    void Start()
    {
        _spawnPos = transform.position;

        LifeCounter.text = "Lives: " + Lives.ToString();
        GameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Lives < 0) {
            //gameover
            if (Input.anyKeyDown) {
                //reset the game by reloading the scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1;
            }
        } else {
            Move();
            Jump();
        }
    }

    void Move() {
        float horizontal = Input.GetAxis("Horizontal");

        if(horizontal != 0) {
            rb.velocity = new Vector2(horizontal * SpeedMultiplier, rb.velocity.y);
        }
    }

    void Jump() {
        bool isGrounded = rb.velocity.y == 0;

        Vector2 raycastOrigin = transform.position;
        //raycastOrigin.y -= collider.size.y / 2;

        LayerMask layerMask = LayerMask.GetMask("Default");

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, collider.size.y / 2 + 0.1f, layerMask);

        if (hit.collider != null) {
            Debug.Log("Hit ground: " + hit.collider.name);
        }

        if (isGrounded && Input.GetButton("Jump")) {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }


    private void Die() {
        Dead = true;

        Lives--;

        if(Lives < 0) {
            //game over
            GameOverScreen.SetActive(true);

            Time.timeScale = 0;

            return;
        }

        LifeCounter.text = "Lives: " + Lives.ToString();

        GameObject[] ais = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in ais) {
            enemy.GetComponent<KyleAI>().ResetOnDeath();
        }

        transform.position = _spawnPos;

        Dead = false;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody.CompareTag("Enemy") && !Dead) {
            if (transform.position.y - collider.size.y / 2 > collision.collider.transform.position.y + collision.collider.bounds.size.y / 2) {
                collision.collider.attachedRigidbody.GetComponent<KyleAI>().KillEnemy();
            } else {
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("DeathPlane")) {
            Die();
        }
    }
}
