using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : InvaderObject
{
    private float myLifeTime = 0f;
    private float maxLifeTime = 10f;
    private float movementSpeed = 0.2f;

    private bool running = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (running) {
            transform.position = transform.position + new Vector3(0f, movementSpeed, 0f); 
            
            myLifeTime += Time.deltaTime;
            if (myLifeTime >= maxLifeTime) Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy" || 
            collision.gameObject.name == "Wall") {
            Destroy(gameObject);
        }
    }

    public override void Run() {
        running = true;
    }

    public override void Pause() {
        running = false;
    }

    public override void Reset() {
        Destroy(gameObject);
    }
}
