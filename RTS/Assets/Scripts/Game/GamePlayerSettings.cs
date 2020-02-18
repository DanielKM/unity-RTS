using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerSettings : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private AudioSource myAudio;
    // Start is called before the first frame update
    public void Awake ()
    {
        // 1
        // if (!PlayerPrefs.HasKey("music"))
        // {
        // PlayerPrefs.SetInt("music", value);
        // slider.value = true;
        // myAudio.enabled = true;
        // PlayerPrefs.Save ();
        // }
        // // 2
        // else
        // {
        // if (PlayerPrefs.GetInt ("music") == 0)
        // {
        //     myAudio.enabled = false;
        //     slider.isOn = false;
        // }
        // else
        // {
        //     myAudio.enabled = true;
        //     slider.isOn = true;
        // }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
