using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {
    public Team team;

    public GameObject[] machinePrefabs;
    public List<Machine> machines;

    public float moveSpeed = 2f;

    private bool moving;
    private float distance;

    public float spawnFrequency = 8f;
    public float spawnFrequencyIncrease = 0.04f;
    public float cutoff;

    public ConveyorBelt conveyorBelt;
    public int scoreTwoPieces = 1;
    public int scoreThreePieces = 2;

    void Start() {
        machines.ForEach(m => m.Init(team));
    }

    void Update() {
        if(moving) {
            Vector3 delta = transform.forward * moveSpeed * Time.deltaTime;

            for(int i = machines.Count - 1; i >= 0; i--) {
                machines[i].transform.position += delta;

                if(machines[i].transform.position.z > cutoff) {
                    if(machines[i].IsComplete()) {
                        GameManager.instance.AddScore(team, machines[i].missingPieces.Count == 2 ? scoreTwoPieces : scoreThreePieces);
                    } else {
                    }
                    Destroy(machines[i].gameObject);
                    machines.RemoveAt(i);
                }
            }

            distance += moveSpeed * Time.deltaTime;

            if(distance > spawnFrequency) {
                SpawnMachine();
                distance = 0;
            }

            spawnFrequency = Mathf.Max(3f, spawnFrequency - spawnFrequencyIncrease * Time.deltaTime);
        }
    }

    public void Toggle() {
        moving = !moving;
        conveyorBelt.ToggleBelt(moving);
    }

    public bool Moving { get{return moving; } }

    private void SpawnMachine() {
        Machine machine = Instantiate(machinePrefabs[Random.Range(0, machinePrefabs.Length)], transform.position, transform.rotation, transform).GetComponent<Machine>();
        machine.Init(team);
        machines.Add(machine);
    }

    public void StartRound() {
        moving = true;
        conveyorBelt.ToggleBelt(moving);
    }

    public void EndRound() {
        moving = false;
        conveyorBelt.ToggleBelt(moving);
    }
}
