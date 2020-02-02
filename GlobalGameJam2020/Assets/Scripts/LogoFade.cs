using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoFade : MonoBehaviour {
    Image image;
    float timer = 0f;
    public float fadeDelay = 1f;
    public float fadeDuration = 2f;

    bool active = false;

    void Start() {
        image = GetComponent<Image>();
        Invoke("Activate", fadeDelay);
    }

    private void Activate() {
        active = true;
    }

    void Update() {
        if(!active)
            return;

        timer = Mathf.Min(timer + Time.deltaTime, fadeDuration);
        float t = 1 - (timer / fadeDuration);

        Color color = image.color;
        color.a = t;
        image.color = color;

        if(timer == fadeDuration) {
            Destroy(gameObject);
        }
    }
}
