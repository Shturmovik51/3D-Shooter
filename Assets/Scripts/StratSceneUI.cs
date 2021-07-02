using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StratSceneUI : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button quiteBtn;
    [SerializeField] private Transform mainCam;
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;
    [SerializeField] private Transform upPos;
    [SerializeField] private Transform downPos;
    [SerializeField] private Slider musicSlider;
    //[SerializeField] private AudioSource musicAudioSource;

    private void Awake()
    {       
        startBtn.onClick.AddListener(OnClickStartBtn);
        quiteBtn.onClick.AddListener(OnClickQuiteBtn);
        musicSlider.onValueChanged.AddListener(MusicVolume);
    }
    private void Start()
    {
        musicSlider.value = BackGroundSound.instance.musicAudioSource.volume;
    }

    private void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float z = Input.GetAxis("Mouse Y");

        if(x < 0)
        {
            var pos = Vector3.Lerp(mainCam.position, leftPos.position, 4f*Time.deltaTime);
            mainCam.position = pos;
        }
        if (x > 0)
        {
            var pos = Vector3.Lerp(mainCam.position, rightPos.position, 4f * Time.deltaTime);
            mainCam.position = pos;
        }
        if (z < 0)
        {
            var pos = Vector3.Lerp(mainCam.position, downPos.position, 4f * Time.deltaTime);
            mainCam.position = pos;
        }
        if (z > 0)
        {
            var pos = Vector3.Lerp(mainCam.position, upPos.position, 4f * Time.deltaTime);
            mainCam.position = pos;
        }
    }

    private void OnClickStartBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
    private void OnClickQuiteBtn()
    {
        Application.Quit();
    }

    private void MusicVolume(float value)
    {
        BackGroundSound.instance.musicAudioSource.volume = value;
    }
}