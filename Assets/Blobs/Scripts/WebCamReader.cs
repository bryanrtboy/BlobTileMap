using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamReader : MonoBehaviour
{
    //public Material material;
    private WebCamTexture webcamTexture;

    void Start()
    {

        webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }


}