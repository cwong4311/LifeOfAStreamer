using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3f;
    void Start()
    {
        if (Globals.attitude <= -30) {
            transform.RotateAround(Vector3.zero, Vector3.right, -90 + Globals.attitude);

            if (Globals.attitude > -100) {
                RenderSettings.ambientIntensity = 0.6f + (Globals.attitude / 200);
                RenderSettings.reflectionIntensity = 0.6f + (Globals.attitude / 200);
            } else {
                RenderSettings.ambientIntensity = 0.1f;
                RenderSettings.reflectionIntensity = 0.1f;
            }
        } else {
            RenderSettings.ambientIntensity = 1f;
            RenderSettings.reflectionIntensity = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, speed*Time.deltaTime);
        transform.LookAt(Vector3.zero);
        float intensity = gameObject.transform.rotation.x * 10;
        gameObject.GetComponent<Light>().intensity = intensity;
    }
}
