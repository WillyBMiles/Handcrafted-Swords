using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(1);
        }
    }
}
