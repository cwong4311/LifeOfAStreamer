using UnityEngine;
using System.Collections;

public class Drunk : MonoBehaviour 
{
	public Material material;
	void OnRenderImage (RenderTexture source, RenderTexture destination) 
	{
		Graphics.Blit (source, destination, material);
	}

	void Update() {
		if (GetComponent<CameraPan>().isPanning) this.enabled = false;
	}
}