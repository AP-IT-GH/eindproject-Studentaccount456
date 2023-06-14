using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class ChaseAgentEnvController : MonoBehaviour
{
    [System.Serializable]
    public class AgentInfo
    {
        public ChaseAgentCollab Agent;
        public Transform StartingPos;
        public Rigidbody Rb;
    }

    public GameObject target;
    public int MaxEnvironmentSteps = 25000;
    public GameObject ground;
    public List<AgentInfo> agents;
    public GameObject wallPrefab;
    public int numWalls;
    private List<GameObject> walls;
    private int m_ResetTimer;
    private bool targetTouched;

    private SimpleMultiAgentGroup m_AgentGroup;

    private Bounds areaBounds;

    private HashSet<ChaseAgentCollab> fallenAgents = new HashSet<ChaseAgentCollab>();



    private void Start()
    {
        areaBounds = ground.GetComponent<Collider>().bounds;
        m_AgentGroup = new SimpleMultiAgentGroup();

        foreach (var agent in agents)
        {
            agent.StartingPos.position = GetRandomPositionOnGround();
            agent.Rb = agent.Agent.GetComponent<Rigidbody>();
            m_AgentGroup.RegisterAgent(agent.Agent);
            agent.Agent.AssignEnvController(this);
        }

        SpawnWalls();
        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_AgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }

        // Hurry Up Penalty
        m_AgentGroup.AddGroupReward(-0.5f / MaxEnvironmentSteps);

        // Check if target has been touched and provide group reward
        if (targetTouched)
        {
            m_AgentGroup.AddGroupReward(1.0f);
            targetTouched = false; // Reset the flag
            m_AgentGroup.EndGroupEpisode();
            ResetScene();
        }
    }

    private Vector3 GetRandomPositionOnGround()
    {
        float xSpawn = Random.Range(-areaBounds.extents.x, areaBounds.extents.x);
        float zSpawn = Random.Range(-areaBounds.extents.z, areaBounds.extents.z);
        float ySpawn = areaBounds.extents.y + 2.5f;

        Vector3 spawnPos = new Vector3(xSpawn, ySpawn, zSpawn);

        return spawnPos;
    }

    private void ResetAgents()
    {
        fallenAgents.Clear();
        foreach (var agent in agents)
        {
            agent.StartingPos.position = GetRandomPositionOnGround();
            agent.Rb.velocity = Vector3.zero;
            agent.Rb.angularVelocity = Vector3.zero;
            agent.Agent.EndEpisode();
        }
    }

    public void ResetScene()
    {
        m_ResetTimer = 0;
        ResetAgents();
        target.transform.position = GetRandomPositionOnGround();
        targetTouched = false;
        DestroyWalls();
        SpawnWalls();
    }

    private void SpawnWalls()
    {
        walls = new List<GameObject>();

        for (int i = 0; i < numWalls; i++)
        {
            Vector3 spawnPos = GetRandomPositionOnGround();
            spawnPos += ground.transform.position;
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject wall = Instantiate(wallPrefab, spawnPos, randomRotation);
            walls.Add(wall);
        }
    }

    private void DestroyWalls()
    {
        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }

        walls.Clear();
    }

    public void OnAgentTargetCollision(ChaseAgentCollab agent)
    {
        targetTouched = true;
    }

    public bool HasAgentFallen(ChaseAgentCollab agent)
    {
        return fallenAgents.Contains(agent);
    }

    public void OnAgentFell(ChaseAgentCollab agent)
    {
        fallenAgents.Add(agent);

        // Check if all agents have fallen off the map
        if (fallenAgents.Count == agents.Count)
        {
            // End the episode and reset the scene
            m_AgentGroup.EndGroupEpisode();
            ResetScene();
        }
    }


}
