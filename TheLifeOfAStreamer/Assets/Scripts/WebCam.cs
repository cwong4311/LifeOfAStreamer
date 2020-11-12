using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    private bool webcamConnected = false;
    private bool micConnected = false;

    private bool webcamPlaying = false;

    private WebCamTexture webcamTexture;
    private bool webcamMissing = false;
    private bool micMissing = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ConnectMedia());
    }

    // Update is called once per frame
    void Update()
    {
        if (webcamMissing)
        {
            this.gameObject.SetActive(false);
            return;
        }
        if (!webcamPlaying) Play();
    }

    void OnEnable()
    {
        StartCoroutine(ConnectMedia());
    }

    private void Play()
    {
        if (!webcamConnected) return;

        webcamTexture = new WebCamTexture();
        try {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = webcamTexture;
        }
        catch (Exception e) {
            RawImage renderer = GetComponent<RawImage>();
            renderer.texture = webcamTexture;
            renderer.material.mainTexture = webcamTexture;
        }
        webcamTexture.Play();
        webcamPlaying = true;
    }

    void OnDisable()
    {
        if (webcamTexture != null) webcamTexture.Stop();
    }

    void OnDestroy()
    {
        if (webcamTexture != null) webcamTexture.Stop();
    }

    IEnumerator ConnectMedia()
    {
        findWebCams();

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            webcamConnected = true;
        }
        else
        {
            webcamMissing = true;
        }

        findMicrophones();

        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Microphone found");
        }
        else
        {
            micMissing = true;
        }
    }

    void findWebCams()
    {
        foreach (var device in WebCamTexture.devices)
        {
            Debug.Log("Name: " + device.name);
        }
    }

    void findMicrophones()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }
}
