using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInOut : MonoBehaviour
{
    private string playerTag = "Player";
    public bool isGetin = false;
    public GameObject fpsPlayer;
    public Camera mainCam;
    PlayerCar playerCar;

    void Start()
    {
        fpsPlayer = GameObject.FindWithTag(playerTag);
        mainCam = Camera.main;
        playerCar = GetComponentInParent<PlayerCar>();
        isGetin = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isGetin = true;
        }
        isGetin = false;
    }

    private void PlayerGetOut()
    {
        isGetin = false;
        //fpsPlayer.transform.GetComponentInChildren<Camera>().depth = 0f;
        fpsPlayer.transform.position = transform.position;
        fpsPlayer.transform.position = new Vector3(fpsPlayer.transform.position.x
            - Random.Range(5f, 10f), fpsPlayer.transform.position.y,
            fpsPlayer.transform.position.z);
        fpsPlayer.SetActive(true);
        AudioListener listener = mainCam.GetComponent<AudioListener>();
        listener.enabled = false;
        listener.transform.GetComponentInChildren<AudioListener>().enabled = true;
        playerCar.Brake();
    }

    private void PlayerGetIn()
    {
        isGetin = true;
        fpsPlayer.SetActive(false);
        //mainCam.depth = 0f;
        mainCam.GetComponent<AudioListener>().enabled = false;
        AudioListener listener = mainCam.GetComponent<AudioListener>();
        listener.enabled = true;
        listener.transform.GetComponentInChildren<AudioListener>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            PlayerGetIn();
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            PlayerGetOut();
        }
    }
}
