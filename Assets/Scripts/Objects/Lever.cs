using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lever : MonoBehaviour
{
    public TMP_Text F;
    public TextMeshProUGUI OpenExit;
    public GameObject Exit;

    private float Turn = 90f;
    private bool _OnLever=false;
    private bool _triggerLever = false;
    private bool _textOn = false;
    private float _textAlpha = 0;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (_OnLever)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!_triggerLever)
                {
                    _triggerLever = true;
                    F.text = "";
                }
            }
        }
    }
    private void FixedUpdate()
    {

        if (transform.rotation.eulerAngles.x < 90)
        {
            if (_triggerLever)
            {
                transform.Rotate(new Vector3(Turn*Time.deltaTime,0,0));
                Exit.SetActive(true);
                _textOn = true;
            }
        }
        if(_textOn)
        {
            _textAlpha = _textAlpha + 1 * Time.deltaTime;
            if (_textAlpha < 1)
            {
                OpenExit.color = new Color(1, 1, 1, _textAlpha);
            }
            else
            {
                OpenExit.color = new Color(1, 1, 1, 1);
            }
            if(1.5f<=_textAlpha)
            {
                _textOn = false;
            }
        }
        if(!_textOn && 0<_textAlpha)
        {
            _textAlpha = _textAlpha - 1 * Time.deltaTime;
            OpenExit.color = new Color(1, 1, 1, _textAlpha);
            if(_textAlpha<=0)
            {
                Destroy(OpenExit);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggerLever)
        {
            if (other.CompareTag("Player"))
            {
                F.text = "F";
                _OnLever = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!_triggerLever)
        {
            if (other.CompareTag("Player"))
            {
                F.text = "";
                _OnLever = false;
            }
        }
    }
}
