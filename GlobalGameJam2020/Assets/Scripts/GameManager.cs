using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team { Yellow, Red }

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public Transform spawnPoint;
    public GameObject[] scraps;

    public float roundTime = 60f;

    public float spawnTime = 5f;
    public int spawnQty = 4;
    public float spread = 15f;
    public float spawnForce = 10f;

    private float timer;

    public Text[] scoreText;
    private float[] score;
    private bool roundPlaying;

    public Text counter;

    private float gameTimer = 0;

    private PlayerCharacterController[] players;

    private void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    void Start() {
        players = GameObject.FindObjectsOfType<PlayerCharacterController>();
        score = new float[2];
        timer = 4f;

        gameTimer = roundTime;
        counter.text = Mathf.FloorToInt(gameTimer).ToString();

        StartCoroutine(GameLoop());
    }

    void Update() {
        if(roundPlaying) {
            timer += Time.deltaTime;

            if(timer >= spawnTime) {
                SpawnScrap();
                timer = 0f;
            }

            gameTimer = Mathf.Max(0, gameTimer - Time.deltaTime);
            counter.text = Mathf.CeilToInt(gameTimer).ToString();
        }
    }

    private IEnumerator GameLoop() {
        yield return new WaitForSeconds(3f);
        Debug.Log("Round start");
        StartRound();
        yield return new WaitForSeconds(roundTime);

        //Round over
        Debug.Log("Round over");
        EndRound();
    }

    private void StartRound() {
        roundPlaying = true;
        for(int i = 0; i < players.Length; i++) {
            players[i].Init();
        }
    }

    private void EndRound() {
        roundPlaying = false;
        foreach(PlayerCharacterController player in players) {
            player.Disable();
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

    public void AddScore(Team team) {
        score[(int)team]++;
        scoreText[(int)team].text = score[(int)team].ToString();

    }
}
