using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour

{
    public Slider volumeSlider;

    private void Update()
    {
        AudioListener.volume = volumeSlider.value;
    }
}
