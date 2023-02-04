using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

    /// <summary> ‰æ–Ê”­Œõˆ— </summary>
public class ScreenOverlayManager : MonoBehaviour
{
    ScreenOverlay screenOverlay;
    public static float intencity;

    // Start is called before the first frame update
    void Start()
    {
        screenOverlay = GetComponent<ScreenOverlay>();
        intencity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        screenOverlay.intensity = intencity;

        intencity -= Time.deltaTime;
        intencity = Mathf.Clamp(intencity, 0, 1.0f);
    }
}
