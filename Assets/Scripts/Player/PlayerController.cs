using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    Vector3 move;
    public float MoveSpeed = 100f;
    public GameObject Head;
    public float MouseSensitivity = 100f;
    public PulseSpawner PulseSpawners;
    public EffectSpawner ShotEffectSpawner;
    public EffectSpawner HitEffectSpawner;

    private float _playerXRot = 0f;
    private float _playerYRot = 0f;
    private bool _run = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        _playerYRot += mouseX;
        _playerXRot -= mouseY;
        _playerXRot = Mathf.Clamp(_playerXRot, -90f, 90f);
        move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            move = move + transform.forward*Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move = move + transform.forward *-1 * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move = move + transform.right * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move = move + transform.right*-1 * Time.deltaTime * 10;
        }
        transform.rotation = Quaternion.Euler(0, _playerYRot, 0);
        Head.transform.localRotation = Quaternion.Euler(_playerXRot, 0, 0);
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _run = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _run = false;
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PulseSpawners.PulseActive(Head.transform.position);
        }
        if(Input.GetMouseButtonDown(0))
        {
            ShotEffectSpawner.EffectActive();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 100f))
            {
                Debug.Log("Hit Object: " + hit.collider.name);
                HitEffectSpawner.TargetEffectActive(hit.point);
                if(hit.collider.CompareTag("Enemy"))
                {
                    //hit.collider.gameObject.SetActive(false);
                    hit.collider.transform.parent.gameObject.GetComponent<Enemy>().HitGun();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        //transform.Translate(move);
        if (_run)
        {
            gameObject.GetComponent<Rigidbody>().velocity = move * MoveSpeed*2;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().velocity = move * MoveSpeed;
        }
        //transform.position = transform.position + move;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOver");
        }
        if (collision.gameObject.CompareTag("Exit"))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Win");
        }
    }
}
