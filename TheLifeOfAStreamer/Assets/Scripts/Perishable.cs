using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perishable : MonoBehaviour
{
    private float lifetime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.gameSetting == -1) {return;}
        
        transform.position += new Vector3(-0.1f, 0, 0);
        lifetime += Time.deltaTime;
        if (lifetime > 30f) {
            Destroy(gameObject);
        }
    }
}
