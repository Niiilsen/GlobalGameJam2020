using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Transform spawnPoint;
    public GameObject[] scraps;

    public float spawnTime = 5f;
    public int spawnQty = 4;
    public float spread = 15f;
    public float spawnForce = 10f;

    private float timer;

    void Start() {
        timer = 4f;
    }

    void Update() {
        timer += Time.deltaTime;

        if(timer >= spawnTime) {
            SpawnScrap();
            timer = 0f;
        }
    }

    private void SpawnScrap() {
        if(scraps.Length == 0) {
            return;
        }

        for(int i = 0; i < spawnQty; i++) {
            Item obj = Instantiate(scraps[Random.Range(0, scraps.Length)], spawnPoint.position, spawnPoint.rotation).GetComponent<Item>();
            obj.GetComponent<Rigidbody>().AddForce(Utils.GetPointOnUnitSphereCap(spawnPoint.forward, spread) * spawnForce, ForceMode.Impulse);
            obj.Init();
        }
    }
}
