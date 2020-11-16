using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	private Transform camTransform;
	
	// How long the object should shake for.
	private float shakeDuration = 600f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	private float shakeAmount = 0.1f;
	private float decreaseFactor = 0.1f;
	
	Vector3 originalPos;
	
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GameObject.Find("FPSController/FirstPersonCharacter").transform;
            originalPos = camTransform.localPosition;
		}
	}

	void Update()
	{
		if (shakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
	}
}