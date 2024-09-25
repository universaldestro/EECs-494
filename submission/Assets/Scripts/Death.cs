using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {

    }
    public void HandleDeath(char type)
    {
        if (type == 'p')
        {//handle player Death
            SceneManager.LoadScene("SampleScene");
        }
        else
        {//implement enemy death 

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}