using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogicScript : MonoBehaviour
{

    public GameObject gameoverScreen;
    void Start()
    {
        gameoverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void gameOver()
    {
        gameoverScreen.SetActive(true);
        StopTime();
    }
}
