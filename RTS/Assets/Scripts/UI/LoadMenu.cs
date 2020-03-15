using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class LoadMenu : MonoBehaviour
{
    SaveLoad SaveLoad;
	PauseMenu PM;
	LoadMenu LM;
    GameObject loadPanel;
	public Button load1;
	public Button load2;
	public Button load3;
	public Button load4;
	public Button load5;

    void Start() {
        SaveLoad = GameObject.Find("Game").GetComponent<SaveLoad>();
        loadPanel = GameObject.Find("LoadPanel");
		LM = GameObject.Find("LoadMenu").GetComponent<LoadMenu>();

        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
		    PM = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        }
// actually reference the button
		Button btn1 = load1.GetComponent<Button>();
		load1.onClick.AddListener(Loadbutton1);
		Button btn2 = load2.GetComponent<Button>();
		btn2.onClick.AddListener(Loadbutton2);
		Button btn3 = load3.GetComponent<Button>();
		btn3.onClick.AddListener(Loadbutton3);
		Button btn4 = load4.GetComponent<Button>();
		btn4.onClick.AddListener(Loadbutton4);
		Button btn5 = load5.GetComponent<Button>();
		btn5.onClick.AddListener(Loadbutton5);

      RefreshSaves();

    }

    void Update(){
        RefreshSaves();
    }

    public void RefreshSaves() {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.es3");
        string path = Application.persistentDataPath.ToString();
        string newPath = path.Replace("/", "\\");
        string newestPath = newPath.Replace("/", @"\");

        if(info.Length > 0) {
            for (int i = 0; i < info.Length; i++)
            {
                if(info[i].Exists) {
                    string str = info[i].ToString();
                    string cutStr = str.Replace(newestPath + "\\", "");
                    string finalStr = cutStr.Replace(".es3", "");
                    string number = finalStr.Replace("SaveGame", "");
                    int childNumber = int.Parse(number);

                    Transform child = loadPanel.GetComponent<Transform>().GetChild(childNumber - 1);
                    child.GetChild(0).GetComponent<TextMeshProUGUI>().text = finalStr;
                }
            }
        }
    }

	void Loadbutton1(){
        Resume();
        SaveLoad.load = true;
        SaveLoad.saveLocation = "SaveGame1";
        SceneManager.LoadScene("Level 1");
	}

	void Loadbutton2(){
        Resume();
        SaveLoad.load = true;
        SaveLoad.saveLocation = "SaveGame2";
        SceneManager.LoadScene("Level 1");
	}
	void Loadbutton3(){
        Resume();
        SaveLoad.load = true;
        SaveLoad.saveLocation = "SaveGame3";
        SceneManager.LoadScene("Level 1");
	}
	void Loadbutton4(){
        Resume();
        SaveLoad.load = true;
        SaveLoad.saveLocation = "SaveGame4";
        SceneManager.LoadScene("Level 1");
	}
	void Loadbutton5(){
        Resume();
        SaveLoad.load = true;
        SaveLoad.saveLocation = "SaveGame5";
        SceneManager.LoadScene("Level 1");
	}

    void Resume() {
        Time.timeScale = 1f;
    }
}
