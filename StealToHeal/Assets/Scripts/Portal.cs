using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}