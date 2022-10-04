using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CoolMotionBlur : MonoBehaviour 
{
	[SerializeField] private Material screenMat;
	[SerializeField] private Vector2 movingCenter = new Vector2(0.5f, 0.5f);

	public Material ScreenMat { get{ return screenMat; } }

	void Start ()
	{
		screenMat.SetVector("_Center", new Vector4(movingCenter.x, movingCenter.y, 0, 0));
	}

	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst, screenMat);
	}	
}
