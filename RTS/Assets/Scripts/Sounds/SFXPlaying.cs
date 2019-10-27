using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlaying : MonoBehaviour
{
    public AudioSource Bang;
    public AudioSource Beep;
    public AudioSource Ding;
    public AudioSource GunShot;

    public void PlayBang()
    {
        Bang.Play();
    }

    public void PlayDing()
    {
        Ding.Play();
    }

    public void PlayBeep()
    {
        Beep.Play();
    }

    public void PlayGunShot()
    {
        GunShot.Play();
    }
}
