using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate3DColliders : MonoBehaviour
{
    [Tooltip("This is the rendertexture used to create the objects. Set the rendertexture resolution to the number of objects you want. A good frame rate can be at 64 X 36 pixels.")]
    public RenderTexture m_inputRenderTex;
    [Tooltip("Object to instantiate")]
    public GameObject m_prefab;
    [Tooltip("Create a Quad with material that uses the inputRenderTex as it's texture. The quad size places and scales the prefabs, the texture provides a preview image to check placement.")]
    public MeshRenderer m_destinationQuad;
    public float m_updateFrequency = 1f;

    private Texture2D tex;
    private Dictionary<Vector2Int, GameObject> colliderDictionary;
    private bool isGridCreated = false;


    void Update()
    {
        if (!isGridCreated && m_inputRenderTex.width > 30)
        {
            isGridCreated = true;
            //Once we have a source, run the SetupColliders one time...
            Invoke("SetupColliders", 2);
        }
    }

    void SetupColliders()
    {
        //Since we now have a source, grab it!
        tex = new Texture2D(m_inputRenderTex.width, m_inputRenderTex.height, TextureFormat.ARGB32, false, true);

        colliderDictionary = new Dictionary<Vector2Int, GameObject>();

        float w = m_destinationQuad.bounds.max.x / m_inputRenderTex.width;
        float h = m_destinationQuad.bounds.max.z / m_inputRenderTex.height;

        for (int x = 0; x < m_inputRenderTex.width; x++)
        {
            for (int y = 0; y < m_inputRenderTex.height; y++)
            {
                GameObject g = Instantiate(m_prefab, new Vector3(x * w, 0, y * h), Quaternion.identity);
                g.transform.localScale = new Vector3(w, h, h);
                colliderDictionary.Add(new Vector2Int(x, y), g);
            }
        }
        InvokeRepeating("UpdateColliders", 0, m_updateFrequency);
    }

    public void UpdateColliders()
    {
        if (!isGridCreated || tex == null)
            return;

        //Save the current render texture to
        RenderTexture currentActiveRT = RenderTexture.active;
        //Change the current render texture to the source rendertexture temporarily
        RenderTexture.active = m_inputRenderTex;
        //Fill the texture with the render texture
        tex.ReadPixels(new Rect(0, 0, m_inputRenderTex.width, m_inputRenderTex.height), 0, 0, false);

        //Start with the tile position at 0,0
        Vector2Int tilePos = Vector2Int.zero;
        for (int x = 0; x < m_inputRenderTex.width; x++)
        {
            for (int y = 0; y < m_inputRenderTex.height; y++)
            {
                Color pix = tex.GetPixel(x, y);
                //Debug.Log("Pixel Color is " + pix.ToString());
                tilePos.x = x;
                tilePos.y = y;

                if (pix.r > 0)
                    colliderDictionary[tilePos].SetActive(true);
                else
                    colliderDictionary[tilePos].SetActive(false);

            }
        }
        //Restore the render texture
        RenderTexture.active = currentActiveRT;

    }


}

