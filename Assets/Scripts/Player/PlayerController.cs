using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector3 move;
    public float MoveSpeed = 100f;
    public GameObject Head;
    public float MouseSensitivity = 100f;
    public PulseSpawner PulseSpawners;
    public EffectSpawner ShotEffectSpawner;
    public EffectSpawner HitEffectSpawner;
    public LayerMask layerToIgnore;
    public GameObject Gun;

    public float PulseBattery = 8f;
    public float PulseBatteryNow = 8f;
    public Image PulseBatteryUI;

    public float CoolTime = 3f;
    public float CoolTimeNow = 0;
    public Image CoolTimeUI;

    //private bool _haveGun = false;
    private Rigidbody _rigidbody;
    private float _playerXRot = 0f;
    private float _playerYRot = 0f;
    private bool _run = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Move();
        
        if(Input.GetMouseButtonDown(0))
        {
            ShotEffectSpawner.EffectActive();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int mask = ~layerToIgnore;
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 100f, mask))
            {
                Debug.Log("Hit Object: " + hit.collider.name);
                HitEffectSpawner.TargetEffectActive(hit.point);
                if(hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.transform.parent.gameObject.GetComponent<Enemy>().HitGun();
                }
            }
        }
    }

    void Move()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        _playerYRot += mouseX;
        _playerXRot -= mouseY;
        _playerXRot = Mathf.Clamp(_playerXRot, -90f, 90f);
        move = Vector3.zero;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(h, 0, v);
            move = transform.TransformDirection(movement) * Time.deltaTime * 10;

        }
        else
        {
            _rigidbody.velocity = new Vector3(0, 0, 0);
        }
        transform.rotation = Quaternion.Euler(0, _playerYRot, 0);
        Head.transform.localRotation = Quaternion.Euler(_playerXRot, 0, 0);
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _run = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _run = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (CoolTimeNow <= 0 && 0<PulseBatteryNow)
            {
                PulseSpawners.PulseActive(Head.transform.position);
                CoolTimeNow = CoolTime;
                PulseBatteryNow--;
                if (0 < PulseBatteryNow)
                {
                    PulseBatteryUI.fillAmount = PulseBatteryNow / PulseBattery;
                }
                else
                {
                    PulseBatteryUI.fillAmount = 0;
                }
            }
        }
    }


private void FixedUpdate()
    {
        //transform.Translate(move);
        if (_run)
        {
            _rigidbody.velocity = move * MoveSpeed*2;
        }
        else
        {
            _rigidbody.velocity = move * MoveSpeed;
        }
        //transform.position = transform.position + move;

        
        if (0 < CoolTimeNow)
        {
            CoolTimeNow = CoolTimeNow - 1 * Time.deltaTime;
            if(CoolTimeNow<=0)
            {
                CoolTimeUI.fillAmount = 0;
                CoolTimeNow = 0;
            }
            else
            {
                CoolTimeUI.fillAmount = CoolTimeNow / CoolTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOver");
        }
    }
}
