using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class SaveMenu : MonoBehaviour
{
    SaveLoad SaveLoad;
	PauseMenu PM;
	LoadMenu LM;
    GameObject savePanel;
    public static string saveLocation;
	public Button save1;
	public Button save2;
	public Button save3;
	public Button save4;
	public Button save5;

    void Start() {
		LM = GameObject.Find("LoadMenu").GetComponent<LoadMenu>();
		PM = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
		SaveLoad = GameObject.Find("Game").GetComponent<SaveLoad>();
        savePanel = GameObject.Find("SavePanel");

// actually reference the button
		Button btn1 = save1.GetComponent<Button>();
		save1.onClick.AddListener(SaveButton1);
		Button btn2 = save2.GetComponent<Button>();
		btn2.onClick.AddListener(SaveButton2);
		Button btn3 = save3.GetComponent<Button>();
		btn3.onClick.AddListener(SaveButton3);
		Button btn4 = save4.GetComponent<Button>();
		btn4.onClick.AddListener(SaveButton4);
		Button btn5 = save5.GetComponent<Button>();
		btn5.onClick.AddListener(SaveButton5);
        RefreshSaves();
    }
	
    void Update(){
        RefreshSaves();
    }

	void RefreshSaves() 
	{
        
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
                    string number = finalStr.Replace("SaveData", "");
                    int childNumber = int.Parse(number);

                    Transform child = savePanel.GetComponent<Transform>().GetChild(childNumber - 1);
                    child.GetChild(0).GetComponent<TextMeshProUGUI>().text = finalStr;
                }
            }
        }
	}

	void SaveButton1(){
		Debug.Log ("You have clicked the SaveData1!");
        saveLocation = "SaveData1";
        SaveLoad.SaveGame(saveLocation);
		LM.RefreshSaves();
		RefreshSaves();
	}

	void SaveButton2(){
		Debug.Log ("You have clicked the SaveData2!");
        saveLocation = "SaveData2";
        SaveLoad.SaveGame(saveLocation);
		LM.RefreshSaves();
		RefreshSaves();
	}
	void SaveButton3(){
		Debug.Log ("You have clicked the SaveData3!");
        saveLocation = "SaveData3";
        SaveLoad.SaveGame(saveLocation);
		LM.RefreshSaves();
		RefreshSaves();
	}
	void SaveButton4(){
		Debug.Log ("You have clicked the SaveData4!");
        saveLocation = "SaveData4";
        SaveLoad.SaveGame(saveLocation);
		LM.RefreshSaves();
		RefreshSaves();
	}
	void SaveButton5(){
		Debug.Log ("You have clicked the SaveData5!");
        saveLocation = "SaveData5";
        SaveLoad.SaveGame(saveLocation);
		LM.RefreshSaves();
		RefreshSaves();
	}
}
