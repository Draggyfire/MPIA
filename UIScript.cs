using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScript : MonoBehaviour
{
    public Button relaunchButton;
    public Button nextButton;
    public Button previousButton;
    public TMP_Text title;
    public string textTitle;

    private int previousSceneIndex;
    private int nextSceneIndex;

    void Awake()
    {

        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (previousSceneIndex >= 0)
        {
            previousButton.gameObject.SetActive(true);
            //previousButton.onClick.AddListener(LoadPreviousScene);
        }
        else
        {
            previousButton.gameObject.SetActive(false);

        }

        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;


        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            nextButton.gameObject.SetActive(true);
            //nextButton.onClick.AddListener(LoadNextScene);
        }
        else
        {
            nextButton.GetComponentInChildren<TMP_Text>().text = "Quitter";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(Quitter);
        }
        relaunchButton.onClick.AddListener(Relaunch);
        title.text= textTitle;
    }

    public void Relaunch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Vérifie si la scène suivante existe
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Dernière scène atteinte !");
        }
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(previousSceneIndex);
    }

    void Quitter()
    {
        Debug.Log("Bouton Quitter pressé !");
        Application.Quit();
    }
}
