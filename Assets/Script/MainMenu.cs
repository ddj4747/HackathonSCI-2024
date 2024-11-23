using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
    [SerializeField]
    private string playScene; 

    [SerializeField]
    private Button[] buttons;

    [SerializeField]    
    int selectedButton = 0;

    public void Start()
    {
        buttons[selectedButton].Select();

        SoundManager.PlayMusic(Music.Hackathon2);
    }

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadSceneAsync(playScene);
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
    }

    public void OnTutoButtonPressed()
    {

    }
}
