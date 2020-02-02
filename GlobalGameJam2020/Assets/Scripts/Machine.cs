using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Machine : MonoBehaviour {
    public Transform canvas;
    public GameObject iconPrefab;

    public List<ScrapInfo> missingPieces;
    private Transform[] currentPieces;
    public List<Transform> anchorPoints;

    public Color BlueColor;
    public Color GreenColor;
    public Color YellowColor;
    public Color RedColor;

    public Team team;

    public void Init(Team t) {
        team = t;

        for(int i = 0; i < missingPieces.Count; i++) {
            Image icon = Instantiate(iconPrefab, canvas).GetComponent<Image>();
            icon.sprite = missingPieces[i].image;

            //TEMP
            if(missingPieces[i].scrapType == ScrapType.Red) {
                icon.color = RedColor;
            }else if(missingPieces[i].scrapType == ScrapType.Blue) {
                icon.color = BlueColor;
            } else if(missingPieces[i].scrapType == ScrapType.Green) {
                icon.color = GreenColor;
            } else if(missingPieces[i].scrapType == ScrapType.Yellow) {
                icon.color = YellowColor;
            }
        }

        currentPieces = new Transform[missingPieces.Count];
    }

    public bool Use(Team t, Item item) {
        if(t != team) {
            return false;
        }

        for(int i = 0; i < missingPieces.Count; i++) {
            if(missingPieces[i].scrapType == item.scrapType) {
                item.transform.parent = transform;
                item.transform.position = anchorPoints[i].position;
                item.transform.rotation = anchorPoints[i].rotation;

                currentPieces[i] = item.transform;

                Transform icon = canvas.GetChild(i);
                icon.GetChild(0).gameObject.SetActive(true);
                icon.GetChild(1).gameObject.SetActive(true);

                return true;
            }
        }
        return false;
    }

    public bool IsComplete() {
        bool test = true;
        foreach(Transform t in currentPieces) {
            if(t == null) {
                test = false;
                break;
            }
        }

        return test;
    }
}
