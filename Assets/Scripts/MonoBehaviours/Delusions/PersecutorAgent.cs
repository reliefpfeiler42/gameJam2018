﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PersecuterAgent : MonoBehaviour
{
    public GameObject playerCharacter;
    public bool IsHaunting { get; set; }
    public float killDistance = 1f;
    public float speedMultiplier = 1.2f;

    // NAVMESH AGENT
    public Animator animator;
    public NavMeshAgent agent;
    public float minDistanceToPlayerChar = 3f;
    public float maxDistanceToPlayerChar = 5f;

    private Vector3 destinationPosition;
    private const float navMeshSampleDistance = 4f;

    // ANIMATOR
    private readonly int hashSpeedParam = Animator.StringToHash("Speed");
    private float speedDampTime = 0.1f;

    // GLOBAL CONTROLLERS
    private GameLogicController _glc;

    private void Start()
    {
        //destinationPosition = transform.position;
        IsHaunting = false;

        _glc = FindObjectOfType<GameLogicController>();
    }

    private void Update()
    {
        if (agent.pathPending)
            return;

        float speed = agent.desiredVelocity.magnitude;

        // Set animator parameter to the current speed of the nav mesh agent and damp it out with speedDampTime * Time.deltaTime
        animator.SetFloat(hashSpeedParam, speed, speedDampTime, Time.deltaTime);

        // Check if persecutor should kill player
        CheckPlayerKill();
    }

    public void SpawnAndHaunt()
    {
        if (IsHaunting)
            return;

        Vector3 spawnPosition = SamplePosition(DetermineSpawningPointWorldPos());
        Quaternion rotationToPlayerChar = Quaternion.LookRotation(playerCharacter.transform.position, Vector3.up);

        transform.SetPositionAndRotation(spawnPosition, rotationToPlayerChar);

        InvokeRepeating("FollowAndHauntPlayer", 1f, 1f);
        IsHaunting = true;
    }

    public void StopHaunting()
    {
        IsHaunting = false;
        CancelInvoke();
    }

    private void FollowAndHauntPlayer()
    {
        agent.SetDestination(SamplePosition(playerCharacter.transform.position));
        agent.speed = agent.speed * speedMultiplier;
    }

    private void CheckPlayerKill()
    {
        // Check if persecutor is in killing range of player character
        if (Vector3.Distance(playerCharacter.transform.position, transform.position) <= killDistance)
        {
            _glc.NotifyPlayerDeath();
			// ANIMATION TRIGGER IN ANIMATOR
        }
    }

    private Vector3 DetermineSpawningPointWorldPos()
    {
        Vector3 spawningPosition = playerCharacter.transform.position;

        // Find random point in donut shaped circular object with ranged magnitude
        float angle = Random.Range(0f, 360f);
        float mag = Random.Range(minDistanceToPlayerChar, maxDistanceToPlayerChar);

        return spawningPosition + new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) * mag;
    }

    private Vector3 SamplePosition(Vector3 source)
    {
        // Sample point on nav mesh with source position set to the current player character's position
        NavMeshHit hit;
        if (NavMesh.SamplePosition(source,
                                    out hit,
                                    navMeshSampleDistance,
                                    NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
            return GetRandomLocation();
    }

    // Copied from https://answers.unity.com/questions/857827/pick-random-point-on-navmesh.html
    private Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }
}