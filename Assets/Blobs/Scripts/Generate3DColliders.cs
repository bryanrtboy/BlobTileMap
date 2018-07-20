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
    [Tooltip("Do not evaluate pixels above this height, to save time and ignore objects that are high in the frame.")]
    public int m_heightLimit = 8;
    [Tooltip("Simple Background subtraction, only light up pixels that are not from the last background capture. A ten second delay between captures works well.")]
    public bool m_useBackgroundSubtraction = false;
    public float m_backgroundRefreshRate = 10f;

    private Texture2D m_tex;
    private Dictionary<Vector2Int, GameObject> m_colliderDictionary;
    private bool m_isGridCreated = false;
    private float m_nextBackgroundTime;
    private Texture2D m_background;

    void Update()
    {
        if (!m_isGridCreated && m_inputRenderTex.width > 5)
        {
            m_isGridCreated = true;
            //Once we have a source, run the SetupColliders one time...
            Invoke("SetupColliders", 2);
        }
    }

    void SetupColliders()
    {
        //Since we now have a source, grab it!
        m_tex = new Texture2D(m_inputRenderTex.width, m_inputRenderTex.height, TextureFormat.ARGB32, false, true);
        m_background = new Texture2D(m_inputRenderTex.width, m_inputRenderTex.height, TextureFormat.ARGB32, false, true);

        m_colliderDictionary = new Dictionary<Vector2Int, GameObject>();

        float w = m_destinationQuad.bounds.size.x / m_inputRenderTex.width;
        float h = m_destinationQuad.bounds.size.y / m_inputRenderTex.height;
        float d = m_destinationQuad.bounds.size.z;
        // Debug.Log("Render Quad bounds is " + m_destinationQuad.bounds.size.ToString());

        Vector3 offset = m_destinationQuad.bounds.extents;

        for (int x = 0; x < m_inputRenderTex.width; x++)
        {
            for (int y = 0; y < m_inputRenderTex.height; y++)
            {
                GameObject g = Instantiate(m_prefab, new Vector3((x * w) - offset.x, (y * h) - offset.y, m_destinationQuad.transform.position.z), Quaternion.identity);
                g.transform.localScale = new Vector3(w, h, g.transform.localScale.z);
                m_colliderDictionary.Add(new Vector2Int(x, y), g);
            }
        }
        InvokeRepeating("UpdateColliders", 0, m_updateFrequency);
    }

    public void UpdateColliders()
    {
        if (!m_isGridCreated || m_tex == null)
            return;

        //Save the current render texture to
        RenderTexture currentActiveRT = RenderTexture.active;
        //Change the current render texture to the source rendertexture temporarily
        RenderTexture.active = m_inputRenderTex;
        //Fill the texture with the render texture
        m_tex.ReadPixels(new Rect(0, 0, m_inputRenderTex.width, m_inputRenderTex.height), 0, 0, false);



        //Start with the tile position at 0,0
        Vector2Int tilePos = Vector2Int.zero;
        for (int x = 0; x < m_inputRenderTex.width; x++)
        {
            for (int y = 0; y < m_inputRenderTex.height; y++)
            {
                Color pix = m_tex.GetPixel(x, y);

                int colorValue = Mathf.RoundToInt(pix.r);


                if (m_useBackgroundSubtraction)
                {
                    Color backpix = m_background.GetPixel(x, y);
                    colorValue += Mathf.RoundToInt(backpix.r);
                    //Grab a background to test against for background subtraction
                    if (Time.time > m_nextBackgroundTime)
                    {
                        m_nextBackgroundTime = Time.time + m_backgroundRefreshRate;
                        m_background.ReadPixels(new Rect(0, 0, m_inputRenderTex.width, m_inputRenderTex.height), 0, 0, false);
                        m_background.Apply();
                        //Debug.Log("Got a new background at " + Time.time);
                    }

                }

                tilePos.x = x;
                tilePos.y = y;

                if (colorValue == 1 && y < m_heightLimit)
                    m_colliderDictionary[tilePos].SetActive(true);
                else
                    m_colliderDictionary[tilePos].SetActive(false);

            }
        }


        //Restore the render texture
        RenderTexture.active = currentActiveRT;

    }


}

