using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour {
    private PlayerCharacterController playerCharacterController;

    private Vector3 moveVector;

    Player player;

    public enum Players { Player0, Player1, Player2, Player3 }
    public Players playerId;

    private bool gameEnd;


    void Start() {
        playerCharacterController = GetComponent<PlayerCharacterController>();

        Init(playerId);
    }

    public void Init(Players id) {
        playerId = id;
        player = ReInput.players.GetPlayer((int)playerId);
    }

    void Update() {
        moveVector = new Vector3(player.GetAxis("LeftHorizontal"), 0f, player.GetAxis("LeftVertical"));

        if(player.GetButtonDown("PickUp")) {
            playerCharacterController.ItemInteraction();
        } 
        
        if(player.GetButtonUp("Throw")) {
            playerCharacterController.ThrowItem();
        }

        if(player.GetButtonUp("Use")) {
            //playerCharacterController.UseItem();
        }

        if(player.GetButtonDown("Dash")) {
            playerCharacterController.Dash(transform.forward);
        }

        if(gameEnd && playerId == Players.Player0) {
            if(player.GetButtonDown("Start")) {
                Debug.Log("Reload scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void FixedUpdate() {
        if(moveVector.sqrMagnitude > 0.01f) {
            playerCharacterController.Move(moveVector);
        }
    }

    public void EndGameControls() {
        gameEnd = true;
    }
}
