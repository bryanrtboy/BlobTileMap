// Bryan Leister
// Based on RandomTiles.cs
//
//
// tested with unity version: 2017.2.0b4
// info: Fills tilemap with random tiles
// usage: Attach this script to empty gameobject, assign some tiles, then press play
// please make sure that you have at least version 2017.2 or the experimental 2d unity 5_5
// https://forum.unity3d.com/threads/update-july-2017.484397/

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityLibary;
using System.Collections;
using System.Collections.Generic;


public class Generate2DTileMap : MonoBehaviour
{
    public RenderTexture m_source;
    public Camera m_cam;
    public GameObject m_renderQuad;
    public GameObject m_obstaclePrefab;
    public int m_obstacleCount;

    //public RuleTile ruleTile;
    public TileBase ruleTile;

    private bool m_isTileMapCreated = false;
    private Texture2D tex;
    private Tilemap map;
    private Dictionary<Vector3Int, GameObject> obstacleDictionary;
    private int poolCount = 0;

    void Start()
    {
    }

    void SetupCam()
    {
        if (m_cam != null && m_renderQuad != null)
        {
            m_renderQuad.transform.localScale = new Vector3(m_source.width, m_source.height, m_renderQuad.transform.localScale.z);
            m_cam.transform.position = new Vector3(m_source.width * .5f, m_cam.transform.position.y, m_source.height * .5f);
            m_cam.orthographicSize = m_source.height * .5f;

        }
    }

    void Update()
    {
        if (!m_isTileMapCreated && m_source.width > 30)
        {
            m_isTileMapCreated = true;
            //SetupCam();
            Invoke("RandomRuleTileMap", 2);
        }
    }

    //This runs after m_source has a size (web cam is being read), it only runs one time...
    void RandomRuleTileMap()
    {
        // validation
        if (ruleTile == null)
        {
            Debug.LogError("Tiles not assigned", gameObject);
            return;
        }

        var parent = transform.parent;

        if (parent == null)
        {
            var go = new GameObject("grid");
            go.AddComponent<Grid>();
            //go.AddComponent<GridInformation>();
            transform.SetParent(go.transform);
        }
        else
        {
            if (parent.GetComponent<Grid>() == null)
            {
                parent.gameObject.AddComponent<Grid>();
            }
        }


        TilemapRenderer tr = GetComponent<TilemapRenderer>();
        if (tr == null)
        {
            tr = gameObject.AddComponent<TilemapRenderer>();
        }

        map = GetComponent<Tilemap>();
        if (map == null)
        {
            map = gameObject.AddComponent<Tilemap>();
        }
        //Make a texture2D from the render texture
        tex = new Texture2D(m_source.width, m_source.height, TextureFormat.ARGB32, false, true);

        //Make all of the obstacle objects to cover the entire grid
        Invoke("SetupObstacles", 3);

    }
    //Set from another script or Invoke after making the Map
    public void SetRandomRuleTiles()
    {
        if (!m_isTileMapCreated || tex == null)
            return;

        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = m_source;
        tex.ReadPixels(new Rect(0, 0, m_source.width, m_source.height), 0, 0, false);
        //Debug.Log("Render texture is " + m_source.width);
        // random map generation
        Vector3Int tilePos = Vector3Int.zero;
        for (int x = 0; x < m_source.width; x++)
        {
            for (int y = 0; y < m_source.height; y++)
            {
                Color pix = tex.GetPixel(x, y);
                //Debug.Log("Pixel Color is " + pix.ToString());
                tilePos.x = x;
                tilePos.y = y;
                SetATile(tilePos, pix);
            }
        }
        RenderTexture.active = currentActiveRT;

        UpdateObstacles();
    }

    void SetATile(Vector3Int tilePos, Color color)
    {
        if (color.r > 0)
        {
            if (!map.HasTile(tilePos))
                map.SetTile(tilePos, ruleTile);
        }
        else if (map.HasTile(tilePos))
        {
            map.SetTile(tilePos, null);
        }
    }

    //Make an obstacle for each position in the tilemap
    void SetupObstacles()
    {
        obstacleDictionary = new Dictionary<Vector3Int, GameObject>();
        foreach (var position in map.cellBounds.allPositionsWithin)
        {
            Vector3 pos = new Vector3(position.x, 0, position.y);
            GameObject g = Instantiate(m_obstaclePrefab, pos, Quaternion.identity);
            obstacleDictionary.Add(position, g);
        }
    }

    public void UpdateObstacles()
    {
        if (obstacleDictionary == null)
            return;

        foreach (var position in map.cellBounds.allPositionsWithin)
        {
            obstacleDictionary[position].SetActive(!map.HasTile(position));
        }

    }


}