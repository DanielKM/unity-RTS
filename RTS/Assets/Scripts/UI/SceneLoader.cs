using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour {
    public bool loadScene = false;

    [SerializeField]
    private int scene;
    [SerializeField]
    private Text loadingText;
    public GameObject instructions;
    private CanvasGroup hindegardeMap;
    
    Color zm;

    void Start() {
        hindegardeMap = GameObject.Find("Hindegarde").GetComponent<CanvasGroup>();
        instructions.SetActive(false);
    }

    public void Update() {
        if (loadScene) {
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
    }

    public void PlayGame()
    {
        instructions.SetActive(true);
        hindegardeMap.alpha = 1;
        loadingText.text = "Approaching Hindegarde Village...";
        loadScene = true;
        StartCoroutine(LoadNewScene());
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene() {
        
        if(loadScene) {
            
        }

        yield return new WaitForSeconds(3);
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }
        Debug.Log(loadScene);

    }

}