using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    public Transform[] Effects;
    private ArrayList effectList;
    // Start is called before the first frame update
    void Start()
    {
        effectList = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffects(int index) {
        Transform newEffect = Instantiate(Effects[index], transform);
        effectList.Add(newEffect);
    }

    public void StopEffects(int index) {
        ((Transform)effectList[index]).gameObject.SetActive(false);
    }
}
