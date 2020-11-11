using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : InvaderObject
{
    public Transform laserObj;
    public Transform laserHolder;

    private float fireTimer = 0f;
    private float uniqueDelay = 0f;

    private float health;
    private float maxHP = 30f;
    private float dmgTaken = 10f;

    private bool running = true;
    // Start is called before the first frame update
    void Start()
    {
        uniqueDelay = Random.Range(3f, 7f);
        fireTimer = uniqueDelay;
        health = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (running) {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f) {
                fireTimer = uniqueDelay;
                Instantiate(laserObj, transform.position, Quaternion.identity, laserHolder);
            }
        }

        if (health <= 0) {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == "PlayerLaser") {
            health -= dmgTaken;
        }
    }

    public override void Reset() {
        uniqueDelay = Random.Range(3f, 7f);
        fireTimer = uniqueDelay;
        running = true;

        health = maxHP;
    }

    public override void Run() {
        running = true;
    }

    public override void Pause() {
        running = false;
    }
}
