using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{

    string name; 

    // Start is called before the first frame update
    void Start()
    {
        if (name == null)
        {
            var name = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
