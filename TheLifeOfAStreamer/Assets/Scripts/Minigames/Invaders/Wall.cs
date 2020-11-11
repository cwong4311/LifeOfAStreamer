using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : InvaderObject
{
    private float health;
    private float maxHP = 30f;
    private float dmgTaken = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f) {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyLaser" || 
            collision.gameObject.tag == "PlayerLaser") {
            health -= dmgTaken;
        }
    }

    public override void Reset() {
        health = maxHP;
    }

    public override void Run() {
    }

    public override void Pause() {
    }
}
