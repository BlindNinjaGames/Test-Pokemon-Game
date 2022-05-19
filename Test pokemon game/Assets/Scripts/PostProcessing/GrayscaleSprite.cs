using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayscaleSprite : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] float duration = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartGrayscaleRoutine();
    }

    public void StartGrayscaleRoutine()
    {
        StartCoroutine(GrayscaleRoutine(duration, true));
    }

    public void GrayscaleRoutineReset()
    {
        StartCoroutine(GrayscaleRoutine(duration, false));
    }

    IEnumerator GrayscaleRoutine(float duration, bool isGrayscale)
    {
        float time = 0;

        while (duration > time)
        {
            float durationFrame = Time.deltaTime;
            float ratio = time / duration;
            float grayAmount = isGrayscale
                ? ratio
                : 1 - ratio;
            SetGrayscale(grayAmount);
            time += durationFrame;
            yield return null;
        }

        SetGrayscale(isGrayscale ? 1 : 0);
    }

    public void SetGrayscale(float amount = 1)
    {
        spriteRenderer.material.SetFloat("_GrayscaleAmount", amount);
    }
}
