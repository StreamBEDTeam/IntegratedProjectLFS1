using UnityEngine;

public class SceneComposer : MonoBehaviour
{

    private ArduinoSerial arduinoSerial = new ArduinoSerial();

    static string SCENE1_CONFIG = "Hello World";
    static string SCENE2_CONFIG = "";
    static string SCENE3_CONFIG = "";
    static string SCENE4_CONFIG = "";
    static string SCENE5_CONFIG = "";
    static string SCENE6_CONFIG = "";
    static string SCENE7_CONFIG = "";

    // Start is called before the first frame update
    void Start()
    {
        arduinoSerial.ConnectToArduino();
    }

    // Update is called once per frame
    void Update()
    {
        arduinoSerial.SendText("Unity says Hello!");
    }

    public void onSceneLoaded(string sceneName)
    {
        Debug.Log("SceneComposer: Loaded new scene: " + sceneName);
        if (sceneName == "Scene1")
        {
            arduinoSerial.SendText(SCENE1_CONFIG);
        }
        else if (sceneName == "Scene2")
        {
            arduinoSerial.SendText(SCENE2_CONFIG);
        }
        
    }
}

