using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject m_spawnObject;
    public GameObject m_bounds;
    public RenderTexture m_destination;
    public int m_count = 100;
    public List<GameObject> m_spawns;

    private bool m_hasSpawned = false;
    private Renderer m_rBounds;

    // Use this for initialization
    void Start()
    {
        m_rBounds = m_bounds.GetComponent<Renderer>();
        //SpawnObjects();
    }

    void Update()
    {
        if (!m_hasSpawned && m_destination.width > 20)
        {
            m_hasSpawned = true;
            SpawnObjects();
        }
    }

    void SpawnObjects()
    {
        m_spawns = new List<GameObject>();
        for (int i = 0; i < m_count; i++)
        {
            float randx = Random.Range(m_rBounds.bounds.min.x, m_rBounds.bounds.max.x);
            float randz = Random.Range(m_rBounds.bounds.min.z, m_rBounds.bounds.max.z);
            Vector3 pos = new Vector3(randx, 0, randz);
            Instantiate(m_spawnObject, pos, Quaternion.identity);
        }

    }

    // 	void CheckPixels ()
    // {

    // 	RaycastHit hit;
    // 	Vector3 dir = transform.TransformDirection (-Vector3.up);

    // 	if (!Physics.Raycast (m_castPoint.position, dir, out hit))
    // 		return;

    // 	//Debug.Log ("checking");

    // 	Renderer rend = hit.transform.GetComponent<Renderer> ();
    // 	MeshCollider meshCollider = hit.collider as MeshCollider;
    // 	if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
    // 		return;

    // 	Texture2D tex = rend.material.mainTexture as Texture2D;
    // 	Vector2 pixelUV = hit.textureCoord;


    // 	pixelUV.x *= tex.width;
    // 	pixelUV.y *= tex.height;


    // 	int x = (int)pixelUV.x;
    // 	int y = (int)pixelUV.y;

    // 	int index = (y * tex.width) + x;
    // 	Color c = Color.red;

    // 	if (m_agent._agentManager != null)
    // 		c = m_agent._agentManager.m_drawTexture2D.GetPixelColor (index);


    // 	if (m_agent._flower) {
    // 		m_agent._flower.startColor = c;
    // 	}
    // 	tex.SetPixel (x, y, c);
    // 	tex.Apply ();
    // }
}
