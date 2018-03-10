using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{

    public GameObject m_spawn;
    public MeshRenderer m_spawnArea;
    public int m_spawnCount = 10;
    public float m_delayBeforeSpawning = 5f;

    void Start()
    {
        Invoke("Spawn", m_delayBeforeSpawning);
    }

    void Spawn()
    {
        Tilemap m_map = FindObjectOfType<Tilemap>();

        List<Vector3Int> blocks = new List<Vector3Int>();
        foreach (var position in m_map.cellBounds.allPositionsWithin)
        {
            if (m_map.HasTile(position))
                blocks.Add(position);
        }

        for (int i = 0; i < m_spawnCount; i++)
        {
            int rand = Random.Range(0, blocks.Count);
            Vector3 pos = new Vector3(blocks[rand].x, 0, blocks[rand].y);
            GameObject g = Instantiate(m_spawn, pos, Quaternion.identity);
        }
        m_spawnArea.enabled = false;
    }
}
