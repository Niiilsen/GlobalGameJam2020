using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScrapType { Red, Blue, Green, Yellow }

[CreateAssetMenu(fileName = "NewScrap")]
public class ScrapInfo : ScriptableObject {
    public ScrapType scrapType;
    public GameObject prefab;
    public Sprite image;
}
