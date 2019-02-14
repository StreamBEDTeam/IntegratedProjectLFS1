using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TourController : MonoBehaviour
{

    private SceneComposer sceneComposer;

    public GameObject cameraPanel;

    public void Start()
    {
        sceneComposer = new SceneComposer();    
    }

    public void setCameraMode(bool isEnabled)
    {
        cameraPanel.SetActive(isEnabled);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            onSceneLoaded(sceneName);
            yield return null;
        }
    }

    private void onSceneLoaded(string sceneName)
    {
        sceneComposer.onSceneLoaded(sceneName);
    }

}