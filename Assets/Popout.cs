using UnityEngine;
using UnityEngine.SceneManagement;

public class Popout : MonoBehaviour
{
    [SerializeField]
    private string nextLevel;

    [SerializeField]
    private string mainMenu;

    public void OnExitPressed()
    {
        SceneManager.LoadSceneAsync(mainMenu);
    }

    public void OnNextPressed()
    {
        SceneManager.LoadSceneAsync(nextLevel);
    }
}
