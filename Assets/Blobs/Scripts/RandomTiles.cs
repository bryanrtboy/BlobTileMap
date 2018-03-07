// tested with unity version: 2017.2.0b4
// info: Fills tilemap with random tiles
// usage: Attach this script to empty gameobject, assign some tiles, then press play
// please make sure that you have at least version 2017.2 or the experimental 2d unity 5_5
// https://forum.unity3d.com/threads/update-july-2017.484397/

using UnityEngine;
#if UNITY_2017_2_OR_NEWER
using UnityEngine.Tilemaps;
namespace UnityLibary
{
    public class RandomTiles : MonoBehaviour
    {
        public int width = 32;
        public int height = 32;
        public RenderTexture m_source;

        public Tile[] tiles;

        private bool m_isTileMapCreated = false;
        private Texture2D tex;
        private Tilemap map;

        void Start()
        {
            //RandomTileMap();
        }

        void Update()
        {
            if (!m_isTileMapCreated && m_source.width > 100)
            {
                m_isTileMapCreated = true;
                Invoke("RandomTileMap", 2);
                //InvokeRepeating("SetRandomTiles", 3, 1);
            }
        }

        void RandomTileMap()
        {
            // validation
            if (tiles == null || tiles.Length < 1)
            {
                Debug.LogError("Tiles not assigned", gameObject);
                return;
            }

            var parent = transform.parent;
            if (parent == null)
            {
                var go = new GameObject("grid");
                go.AddComponent<Grid>();
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
            tex = new Texture2D(m_source.width, m_source.height, TextureFormat.ARGB32, false, true);

        }

        public void SetRandomTiles()
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


                    if (pix.r > 0)
                    {
                        if (!map.HasTile(tilePos))
                            map.SetTile(tilePos, tiles[Random.Range(0, tiles.Length)]);
                    }
                    else if (map.HasTile(tilePos))
                    {
                        map.SetTile(tilePos, null);
                    }


                }
            }
            RenderTexture.active = currentActiveRT;
        }
    }

}


#else
public class RandomTiles : MonoBehaviour
{
    public void Start()
    {
        Debug.LogWarning("This version of unity doesnt support UnityEngine.Tilemaps");
    }
}
#endif