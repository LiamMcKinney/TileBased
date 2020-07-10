using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;
    public Vector3 position;

    public int totalRedFrames;
    int redFramesLeft;
    public SpriteRenderer redFlash;

    void Start()
    {
        Shaking = false;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = position;

        if (ShakeIntensity > 0)
        {
            transform.position = position + Random.insideUnitSphere * ShakeIntensity;

            ShakeIntensity -= ShakeDecay;
        }
        else if (Shaking)
        {
            Shaking = false;
        }

        redFramesLeft--;
        if(redFramesLeft < 0)
        {
            redFlash.enabled = false;
        }
    }





    public void Shake()
    {
        Shake(.3f, .02f);
    }

    public void Shake(float intensity)
    {
        Shake(intensity, .02f);
    }

    public void Shake(float intensity, float decay)
    {
        ShakeIntensity = intensity;
        ShakeDecay = decay;
        Shaking = true;
    }


    public void FlashRed()
    {
        redFlash.enabled = true;
        redFramesLeft = totalRedFrames;
    }


}
