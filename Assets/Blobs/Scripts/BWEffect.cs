//Bryan Leister, March 2018
//
//This script uses the BWShader to create a high contrast video from an webcamPlayer
//The concept is this:
//  1. Setup a Camera that is looking at a Quad, the Quad has a WebCamReader.cs to display
//  a live feed from the webcam (use Layers to isolate the Quad and make the Camera only
//  render that Layer)
//
//  2. Set the Rendertexture to the desired resolution (downsampling on the cheap)
//
//  3. Name this Rendertexture the Source
//
//  4. Create an identical Rendertexture and name it Destination
//
//  5. Create a new MainCamera, with a new Quad. The Quad needs a Material that is using
//  the Destination RenderTexture as it's texture
//
//  6. Apply this script to any gameobject, set up the input source and destination
//

using UnityEngine;
using System.Collections;
using UnityLibary;

[ExecuteInEditMode]
public class BWEffect : MonoBehaviour
{
    [Range(0f, 1f)]
    public float m_threshold = .5f;
    [Range(0.1f, 2f)]
    public float m_interval = .5f;
    [Tooltip("The Rendertexture used by the Camera set up to read the webcamtexture feed")]
    public RenderTexture m_source;
    [Tooltip("The Rendertexture used by the Material visible to the MainCamera")]
    public RenderTexture m_destination;
    public RandomTiles m_tileMaker;

    private Material m_material;


    // Creates a private material used to the effect
    void Awake()
    {
        m_material = new Material(Shader.Find("Hidden/BWShader"));
    }

    void Start()
    {
        InvokeRepeating("Process", 1, m_interval);
    }

    // // Uncomment the Invoke and use this to Ppostprocess the image all the time (most responsive)
    // void OnRenderImage(RenderTexture source, RenderTexture destination)
    // {
    //     material.SetFloat("_threshold", threshold);
    //     Graphics.Blit(source, destination, material);
    // }

    // Postprocess the image as needed
    void Process()
    {
        m_material.SetFloat("_threshold", m_threshold);
        Graphics.Blit(m_source, m_destination, m_material);
        if (m_tileMaker != null)
            m_tileMaker.SetRandomTiles();
    }

}