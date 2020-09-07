using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleAI : MonoBehaviour
{
    private Transform _playerTransform;
    public Rigidbody2D EnemyRB;
    public float Speed = 5;

    private Vector2 _spawnPos;


    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    void Move() {
        Vector2 dirToPlayer = _playerTransform.position - transform.position;
        dirToPlayer.y = 0;
        dirToPlayer.Normalize();

        EnemyRB.velocity = new Vector2(dirToPlayer.x * Speed, EnemyRB.velocity.y);
    }

    public void ResetOnDeath() {
        transform.position = _spawnPos;
    }

    public void KillEnemy() {
        Destroy(this.gameObject);
    }
}
