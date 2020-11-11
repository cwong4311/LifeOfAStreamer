using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : InvaderObject
{
    public GameObject rootObj;
    public GameObject gameHandler;
    public Transform laserObj;
    public Transform laserHolder;

    public float[] x_boundary;
    private float moveSpeed = 15;
    
    private float health;
    private float maxHP = 100f;
    private float dmgTaken = 4f;


    private float fireDelay = 0.3f;
    private float delayCounter = 0f;

    private bool running;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (running) {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime);
            if (transform.localPosition.x < x_boundary[0]) 
                transform.localPosition = new Vector3(x_boundary[0], 
                    transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x > x_boundary[1]) 
                transform.localPosition = new Vector3(x_boundary[1], 
                    transform.localPosition.y, transform.localPosition.z);

            delayCounter -= Time.deltaTime;
            if (Input.GetKey("space") && delayCounter <= 0f)
            {
                delayCounter = fireDelay;
                Instantiate(laserObj, transform.position, Quaternion.identity, laserHolder);
            }
        }

        if (health <= 0 && rootObj.tag != "GameOver") {
            Debug.Log("GameOver");
            rootObj.tag = "GameOver";
            gameHandler.GetComponent<InvaderHandler>().GameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == "EnemyLaser") {
            health -= dmgTaken;
        }
    }

    public override void Reset() {
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
