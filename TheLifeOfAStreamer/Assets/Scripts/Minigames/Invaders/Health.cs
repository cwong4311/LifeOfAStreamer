using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float health;
    private float maxHP = 30f;

    // Update is called once per frame
    void Update()
    {
        float displayHealth = (health / maxHP) * 5f;
        transform.localScale = new Vector3(displayHealth, 0.2f, 0.01f);
    }

    public void SetHealth(float curHP, float maximumHP) {
        health = curHP;
        maxHP = maximumHP;
    }
}
