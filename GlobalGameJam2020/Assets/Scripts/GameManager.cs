using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum Team { Yellow, Red }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform spawnPoint;
    public GameObject[] scraps;

    public float roundTime = 60f;

    public float spawnTime = 5f;
    public int spawnQty = 4;
    public float spread = 15f;
    public Vector2 spawnForceMinMax = new Vector2(7, 14);

    public int scoreIncrease = 100;

    private float timer;

    public ScoreBoard[] scoreText;
    private int[] score;
    private bool roundPlaying;

    public TMP_Text counter;

    private float gameTimer = 0;

    private PlayerCharacterController[] players;
    private Conveyor[] conveyors;
    [SerializeField] private ScreenShaker screenShake;

    public Animator portalAnim;

    [SerializeField, FMODUnity.EventRef] string spitSnd;
    private bool anticipating = false;

    
    public Image endScreen;
    public Sprite yellowWin;
    public Sprite redWin;
    public Sprite draw;

    private void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        players = GameObject.FindObjectsOfType<PlayerCharacterController>();
        conveyors = GameObject.FindObjectsOfType<Conveyor>();

        score = new int[2];
        timer = 4f;

        gameTimer = roundTime;
        //counter.text = Mathf.FloorToInt(gameTimer).ToString();

        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (roundPlaying)
        {
            timer += Time.deltaTime;
            if (!anticipating && spawnTime - timer < 1)
            {
                AnticipatePortal();
            }
            if (timer >= spawnTime)
            {


                SpawnScrap();
                timer = 0f;
            }

            gameTimer = Mathf.Max(0, gameTimer - Time.deltaTime);
            counter.text = Mathf.CeilToInt(gameTimer).ToString();
        }
    }

    private void AnticipatePortal()
    {
        portalAnim.SetTrigger("Anticipate");
        anticipating = true;
    }

    private IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Round start");
        StartRound();
        yield return new WaitForSeconds(roundTime);

        //Round over
        Debug.Log("Round over");
        EndRound();
    }

    private void StartRound()
    {

        counter.text = Mathf.FloorToInt(gameTimer).ToString();
        roundPlaying = true;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Init();
        }

        foreach (Conveyor c in conveyors)
        {
            c.StartRound();
        }
    }

    private void EndRound()
    {
        int winners = -1;
        if (score[(int)Team.Yellow] > score[(int)Team.Red])
        {
            winners = (int)Team.Yellow;
            endScreen.sprite = yellowWin;
        } else if (score[(int)Team.Red] > score[(int)Team.Yellow]) {
            winners = (int)Team.Red;
            endScreen.sprite = redWin;
        } else {
            endScreen.sprite = draw;
        }

        roundPlaying = false;
        endScreen.gameObject.SetActive(true);

        foreach(PlayerCharacterController player in players) {
            player.Disable();
            player.GetComponent<PlayerInput>().EndGameControls();

            if (winners == -1)
            {
                player.PlayAnim("Draw");
            }
            else if (player.team == (Team)winners)
            {
                player.PlayAnim("Victory");
            }
            else if (player.team != (Team)winners)
            {
                player.PlayAnim("Defeat");
            }
        }

        foreach (Conveyor c in conveyors)
        {
            c.EndRound();
        }
    }

    private void SpawnScrap()
    {
        if (scraps.Length == 0)
        {
            return;
        }

        FMODUnity.RuntimeManager.PlayOneShot(spitSnd, transform.position);
        screenShake.AddTrauma(1);
        anticipating = false;

        int spawnIndex = 0;
        for (int i = 0; i < spawnQty; i++)
        {
            Vector3 offset = UnityEngine.Random.onUnitSphere * 0.5f;
            //int spawnIndex = Random.Range(0, scraps.Length);

            Item obj = Instantiate(scraps[spawnIndex], spawnPoint.position + offset, spawnPoint.rotation).GetComponent<Item>();
            obj.GetComponent<Rigidbody>().AddForce(Utils.GetPointOnUnitSphereCap(spawnPoint.forward, spread) * UnityEngine.Random.Range(spawnForceMinMax.x, spawnForceMinMax.y), ForceMode.Impulse);
            obj.Init();

            spawnIndex = (spawnIndex + 1) % scraps.Length;
        }
    }


    public void AddScore(Team team, int s, int combo) {
        score[(int)team] += (s * combo);
        scoreText[(int)team].SetScore(score[(int)team], combo);

    }
}
