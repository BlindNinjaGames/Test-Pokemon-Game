using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrayscaleImage : MonoBehaviour
{

    Image gsImage;

    [SerializeField] float duration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        gsImage = GetComponent<Image>();
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

    private IEnumerator GrayscaleRoutine(float duration, bool isGrayscale)
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
        gsImage.material.SetFloat("_GrayscaleAmount", amount);
    }

}