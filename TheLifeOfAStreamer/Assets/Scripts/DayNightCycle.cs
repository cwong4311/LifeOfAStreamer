using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.2f;
    void Start()
    {
        if (Globals.attitude <= -20) {
            transform.rotation = Quaternion.Euler(-30f, 69f, 141f);

            if (Globals.attitude > -100) {
                RenderSettings.ambientIntensity = 0.6f + (Globals.attitude / 200);
                RenderSettings.reflectionIntensity = 0.6f    + (Globals.attitude / 200);
            } else {
                RenderSettings.ambientIntensity = 0.25f;
                RenderSettings.reflectionIntensity = 0.25f;
            }
        } else {
            RenderSettings.ambientIntensity = 1f;
            RenderSettings.reflectionIntensity = 1f;

            if (Globals.attitude >= 5) {
                transform.rotation = Quaternion.Euler(23f, 74f, 259f);
            } else {
                transform.rotation = Quaternion.Euler(5f, 20f, 245f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-speed * Time.deltaTime, 0, 0);
        float intensity = gameObject.transform.rotation.x * 10;
        gameObject.GetComponent<Light>().intensity = intensity;
    }
}
