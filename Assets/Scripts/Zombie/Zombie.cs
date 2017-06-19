using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IEnemy {
    public GameObject destination;

    public void die()
    {
        throw new NotImplementedException();
    }

    public void hit(string part, float dps)
    {
        Debug.Log(part); 
    }


    void Start () {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
