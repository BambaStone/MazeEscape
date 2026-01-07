using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EXIT : MonoBehaviour
{
    public TMP_Text F;
    private bool _onExit=false;


    private void Start()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (_onExit)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("Win");
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            F.text = "F";
            _onExit = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            F.text = "";
            _onExit = false;
        }
    }
}
