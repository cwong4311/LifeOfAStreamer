using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : InvaderObject
{
    public float[] x_boundary;    // int[0] is min, int[1] is max
    public float[] y_boundary;    // int[0] is min, int[1] is max

    public Transform laserHolder;

    private bool running = false;
    private int direction = 0; // 0 is going left, 1 is going right

    private float movementSpeed = 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
        running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (running) {
            if (direction == 0) {
                float newX = transform.localPosition.x - movementSpeed;
                if (newX < x_boundary[0]) {
                    transform.localPosition += new Vector3(0f, -1f, 0f);
                    direction = 1;
                } else {
                    transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
                }
            } else if (direction == 1) {
                float newX = transform.localPosition.x + movementSpeed;
                if (newX > x_boundary[1]) {
                    transform.localPosition += new Vector3(0f, -1f, 0f);
                    direction = 0;
                } else {
                    transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
                }
            }

            if (transform.localPosition.y < y_boundary[0]) {
                transform.localPosition = new Vector3(transform.localPosition.x, y_boundary[0], transform.localPosition.z);
            }
        }
    }

    public override void Run() {
        running = true;

        foreach(Transform child in transform)
        {
            child.gameObject.GetComponent<InvaderObject>().Run();
        }

        foreach(Transform child in laserHolder)
        {
            child.gameObject.GetComponent<InvaderObject>().Run();
        }
    }

    public override void Pause() {
        running = false;

        foreach(Transform child in transform)
        {
            child.gameObject.GetComponent<InvaderObject>().Pause();
        }

        foreach(Transform child in laserHolder)
        {
            child.gameObject.GetComponent<InvaderObject>().Pause();
        }
    }

    public override void Reset() {
        running = false;
        direction = 0;

        transform.localPosition = new Vector3 (x_boundary[1], y_boundary[1], transform.localPosition.z);
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<InvaderObject>().Reset();
        }

        foreach(Transform child in laserHolder)
        {
            Destroy(child.gameObject);
        }
    }
}
