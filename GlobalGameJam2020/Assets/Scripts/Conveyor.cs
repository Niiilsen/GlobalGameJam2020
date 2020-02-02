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

    public float spawnFrequency;
    public float cutoff;

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
                        Debug.Log("Score");
                        GameManager.instance.AddScore(team);
                    } else {
                        Debug.Log("Fail");
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
        }
    }

    public void Toggle() {
        moving = !moving;
    }

    private void SpawnMachine() {
        Machine machine = Instantiate(machinePrefabs[Random.Range(0, machinePrefabs.Length)], transform.position, transform.rotation, transform).GetComponent<Machine>();
        machine.Init(team);
        machines.Add(machine);
    }
}
