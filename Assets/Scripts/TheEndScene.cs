using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEndScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}