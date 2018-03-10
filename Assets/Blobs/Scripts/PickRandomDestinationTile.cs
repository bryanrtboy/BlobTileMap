using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityLibary;

public class PickRandomDestinationTile : MonoBehaviour
{
    private Tilemap m_map;
    private NavMeshAgent m_agent;

    private Vector3Int m_currentDestination;
    private Vector3 m_lastPosition;

    // Use this for initialization
    void Start()
    {
        m_agent = this.GetComponent<NavMeshAgent>();
        float rand = Random.Range(1f, 3f);
        InvokeRepeating("PickRandomDestination", 3, rand);
    }
    void PickRandomDestination()
    {
        if (m_map == null)
        {
            m_map = FindObjectOfType<Tilemap>();
        }
        if (m_map == null)
            return;

        //Because our tilemap is horizontal...
        Vector3Int temp = new Vector3Int(m_currentDestination.x, m_currentDestination.z, 0);

        if (!m_map.HasTile(temp) || Vector3.Distance(m_agent.transform.position, m_lastPosition) < .01f)
        {
            List<Vector3Int> blocks = new List<Vector3Int>();
            foreach (var position in m_map.cellBounds.allPositionsWithin)
            {
                if (m_map.HasTile(position))
                    blocks.Add(position);
            }
            if (blocks.Count < 10)
                return;

            int rand = Random.Range(0, blocks.Count);
            temp = blocks[rand];
            m_currentDestination = new Vector3Int(temp.x, 0, temp.y);
            //m_agent.SetDestination(m_currentDestination);
            if (m_agent.isActiveAndEnabled && m_agent.isOnNavMesh)
                m_agent.destination = m_currentDestination;
            //            Debug.Log("Destination is " + m_currentDestination.ToString());
        }

        m_lastPosition = m_agent.transform.position;
    }
}
