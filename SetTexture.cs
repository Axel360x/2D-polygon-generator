using UnityEngine;
using System.Collections;

public class SetTexture : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        tex.filterMode = FilterMode.Point;
        tex.SetPixel(0, 0, Color.black);
        tex.SetPixel(1, 1, Color.green);
        tex.SetPixel(0, 1, Color.gray);
        tex.SetPixel(1, 0, Color.white);
        tex.Apply(false);
        renderer.material.mainTexture = tex;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
