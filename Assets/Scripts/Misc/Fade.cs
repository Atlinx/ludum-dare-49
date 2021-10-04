using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public SpriteRenderer sprite;
    public AnimationCurve fadeOverTime;
    public float duration;
    private float time = 0;

    private void Update()
    {
        if (time < duration)
        {
            time += Time.deltaTime;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, fadeOverTime.Evaluate(time / duration));
        } else
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, fadeOverTime.Evaluate(1));
    }
}
