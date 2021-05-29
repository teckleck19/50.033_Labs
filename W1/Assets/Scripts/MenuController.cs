using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{   
    private Vector3 originalPosition;
    void Awake()
    {
        Time.timeScale = 0.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked()
    {
        foreach (Transform eachChild in transform)
        {   
            eachChild.gameObject.SetActive(false);
            if (eachChild.name == "Score" || eachChild.name == "Lives" )
            {
                Debug.Log("Child found. Name: " + eachChild.name);
                // disable them
                eachChild.gameObject.SetActive(true);
                Time.timeScale = 1.0f;
            }
        }
    }

    // public void ReplayButtonClicked()
    // {
    //     foreach (Transform eachChild in transform)
    //     {
    //         if (eachChild.name != "Score")
    //         {
    //             Debug.Log("Child found. Name: " + eachChild.name);
    //             // disable them
    //             eachChild.gameObject.SetActive(false);
    //             Time.timeScale = 1.0f;
    //         }
    //     }
    // }
}
