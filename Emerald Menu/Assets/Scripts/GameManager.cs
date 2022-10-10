using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject playPanel;
    public GameObject optionsPanel;

    [Header("Buttons")]
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject quitButton;

    [Header("Objects")]
    public GameObject wing1;
    public GameObject wing2;
    public GameObject audioToggleBall;
    public GameObject fsToggleBall;
    public GameObject fixedReboot;
    public GameObject rebootObject;
    public GameObject rebootOutlineObject;

    [Header("UI")]
    public Slider volumeSlider;
    public Toggle audioToggle;
    public Toggle fsToggle;
    public TMP_Dropdown resolutionDD;
    public TMP_InputField usernameInput;
    public Image unamePromptImage;
    public TextMeshProUGUI unamePromptText;
    public Sprite promptTrue;
    public Sprite promptFalse;
    public Image rebootBar;
    public TextMeshProUGUI deluxePromptText;
    public TextMeshProUGUI connectingPromptText;
    public Button rebootButton;
    public Button deluxeButton;

    [Header("SFX")]
    public AudioClip flapSFX;
    public AudioClip birdSFX;
    public AudioClip button1_SFX;
    public AudioClip button2_SFX;

    [Header("Others")]
    public Transform flyPosRef;
    public Transform playOriPos;
    public Transform optionsPosRefOff;
    public Transform optionsPosRefOn;
    private Tweener animTweener;
    public CanvasGroup mainMenuGroup;
    public CanvasGroup optionsGroup;
    private bool fadeAlphaGroup = false;
    private AudioSource audioSource;
    public Transform audioTogPosRefOff;
    public Transform audioTogPosRefOn;
    public Transform fsTogPosRefOff;
    public Transform fsTogPosRefOn;
    private Color32 colorToggleON = new Color(64f / 255f, 255f / 255f, 148f / 255f);
    private Color32 colorToggleOFF = new Color(255f / 255f, 86f / 255f, 64f / 255f);
    private RectTransform unamePromptRect;
    private Color32 colorPromptTrue = new Color(0f / 255f, 255f / 255f, 155f / 255f);
    private Color32 colorPromptFalse = new Color(255f / 255f, 0f / 255f, 87f / 255f);
    private bool rebootAct = false;
    private float rebootTimer = 0f;
    private bool rebootFinished = false;
    private bool deluxeAct = false;
    private float deluxeTimer = 0f;
    private float deluxeDots = 0f;
    public AudioSource SFXsource;
    private bool wasUnder4 = true;
    private bool rebootBleep = true;
    private bool deluxeBleep = true;

    [Header("Extra")]
    public CustomCursor customCursor;
    public SaveLoad SaveLoad;

    void Start()
    {
        optionsGroup.alpha = 0;
        optionsGroup.blocksRaycasts = false;
        optionsGroup.transform.position = optionsPosRefOff.position;

        audioSource = GetComponent<AudioSource>();

        audioToggle.isOn = true;
        audioToggleBall.transform.position = audioTogPosRefOn.position;
        animTweener = audioToggle.image
                .DOColor(colorToggleON, 0.5f);

        fsToggle.isOn = true;
        fsToggleBall.transform.position = fsTogPosRefOn.position;
        animTweener = fsToggle.image
                .DOColor(colorToggleON, 0.5f);

        unamePromptRect = unamePromptImage.gameObject.GetComponent<RectTransform>();

        rebootBar.fillAmount = 1;

        SaveLoad.LoadData();
    }

    void Update()
    {
        if (fadeAlphaGroup && (mainMenuGroup.alpha > 0 || optionsGroup.alpha < 1))
        {
            mainMenuGroup.alpha -= 3 * Time.deltaTime;
            optionsGroup.alpha += 3 * Time.deltaTime;
        }
        else if (!fadeAlphaGroup && (mainMenuGroup.alpha < 1 || optionsGroup.alpha > 0))
        {
            mainMenuGroup.alpha += 3 * Time.deltaTime;
            optionsGroup.alpha -= 3 * Time.deltaTime;
        }

        audioSource.volume = volumeSlider.value;
        SFXsource.volume = volumeSlider.value;

        if (usernameInput.text == "")
        {
            unamePromptImage.sprite = promptFalse;
            unamePromptText.text = "Username can't be empty!";
            unamePromptText.color = colorPromptFalse;
            unamePromptRect.anchoredPosition = new Vector2(55f, 2.5f);
        }
        else
        {
            if (usernameInput.text.Length < 4)
            {
                unamePromptImage.sprite = promptFalse;
                unamePromptText.text = "Must be at least 4 characters!";
                unamePromptText.color = colorPromptFalse;
                unamePromptRect.anchoredPosition = new Vector2(36f, 2.5f);
                wasUnder4 = true;
            }
            else
            {
                unamePromptImage.sprite = promptTrue;
                unamePromptText.text = "Username OK!";
                unamePromptText.color = colorPromptTrue;
                unamePromptRect.anchoredPosition = new Vector2(120f, 2.5f);
                if (wasUnder4)
                {
                    SFX_Button_2();
                    wasUnder4 = false;
                }
            }
        }

        if (rebootAct)
        {
            if (rebootBar.fillAmount > 0 && !rebootFinished)
            {
                rebootBar.fillAmount -= Time.deltaTime / 3;
            }
            else
            {
                if (rebootTimer < 2f)
                {
                    rebootTimer += Time.deltaTime;
                    rebootBar.fillAmount = 1;
                    fixedReboot.SetActive(true);
                    rebootFinished = true;
                    if (rebootBleep)
                    {
                        SFX_Button_2();
                        rebootBleep = false;
                    }
                }
                else
                {
                    rebootAct = false;
                    rebootTimer = 0f;
                    rebootFinished = false;
                    fixedReboot.SetActive(false);
                    PlayReturn();
                    rebootButton.interactable = true;
                    rebootBleep = true;
                }
            }
        }

        if (deluxeAct)
        {
            if (deluxeTimer < 6.5f)
            {
                deluxeTimer += Time.deltaTime;
            }
            else
            {
                deluxeAct = false;
                deluxeTimer = 0f;
                deluxeDots = 0f;
                deluxeButton.interactable = true;
                deluxeBleep = true;
                Debug.Log("You're not cool enough.");
            }
        }

        if (deluxeTimer <= 0)
        {
            deluxePromptText.text = "(Not purchased yet)";
        }
        else if (deluxeTimer < 3.6f)
        {
            deluxePromptText.gameObject.SetActive(false);
            connectingPromptText.gameObject.SetActive(true);
            deluxeDots += Time.deltaTime;

            if (deluxeDots < 0.3f)
            {
                connectingPromptText.text = "Connecting to store";
            }
            else if (deluxeDots < 0.6f)
            {
                connectingPromptText.text = "Connecting to store.";
            }
            else if (deluxeDots < 0.9f)
            {
                connectingPromptText.text = "Connecting to store..";
            }
            else if (deluxeDots < 1.2f)
            {
                connectingPromptText.text = "Connecting to store...";
            }
            else
            {
                deluxeDots = 0f;
            }
        }
        else if (deluxeTimer < 6.5f)
        {
            deluxePromptText.text = "You're not cool enough.\nTry again later!";
            connectingPromptText.gameObject.SetActive(false);
            deluxePromptText.gameObject.SetActive(true);
            if (deluxeBleep == true)
            {
                SFX_Button_2();
                deluxeBleep = false;
            }
        }
    }

    public void PlayFly()
    {
        if (mainMenuGroup.alpha == 1)
        {
            playButton.GetComponent<Button>().interactable = false;
            wing1.SetActive(true);
            wing2.SetActive(true);
            animTweener = playPanel.transform
                .DOMove(flyPosRef.position, 5f);
            Debug.Log("Hey..! Come back...!");
        }
    }

    public void PlayReturn()
    {
        playButton.GetComponent<Button>().interactable = true;
        wing1.SetActive(false);
        wing2.SetActive(false);
        animTweener = playPanel.transform
            .DOMove(playOriPos.position, 0.1f);
        Debug.Log("The 'Play' button has been reset.");
    }

    public void ActivateMainMenu()
    {
        if (!rebootAct && !deluxeAct)
        {
            if (optionsGroup.alpha == 1)
            {
                mainMenuGroup.blocksRaycasts = true;
                optionsGroup.blocksRaycasts = false;
                fadeAlphaGroup = false;
                animTweener = optionsGroup.transform
                    .DOMove(optionsPosRefOff.position, 0.5f);
                SaveLoad.SaveData();
                SFX_Button_2();
            }
        }
    }

    public void ActivateOptions()
    {
        if (mainMenuGroup.alpha == 1)
        {
            optionsGroup.blocksRaycasts = true;
            mainMenuGroup.blocksRaycasts = false;
            fadeAlphaGroup = true;
            animTweener = optionsGroup.transform
                .DOMove(optionsPosRefOn.position, 0.5f);
            SFX_Button_1();
        }
    }

    public void ToggleAudio()
    {
        if (audioToggle.isOn)
        {
            animTweener = audioToggleBall.transform
                .DOMove(audioTogPosRefOn.position, 0.5f);
            animTweener = audioToggle.image
                .DOColor(colorToggleON, 0.5f);
            audioSource.mute = false;
            SFXsource.mute = false;
            SFX_Button_1();
            Debug.Log("Audio = ON");
        }
        else
        {
            animTweener = audioToggleBall.transform
                .DOMove(audioTogPosRefOff.position, 0.5f);
            animTweener = audioToggle.image
                .DOColor(colorToggleOFF, 0.5f);
            audioSource.mute = true;
            SFXsource.mute = true;
            Debug.Log("Audio = OFF");
        }
    }

    public void ToggleFullscreen()
    {
        if (fsToggle.isOn)
        {
            animTweener = fsToggleBall.transform
                .DOMove(fsTogPosRefOn.position, 0.5f);
            animTweener = fsToggle.image
                .DOColor(colorToggleON, 0.5f);
            SFX_Button_1();
            Debug.Log("Fullscreen = ON");
        }
        else
        {
            animTweener = fsToggleBall.transform
                .DOMove(fsTogPosRefOff.position, 0.5f);
            animTweener = fsToggle.image
                .DOColor(colorToggleOFF, 0.5f);
            SFX_Button_1();
            Debug.Log("Fullscreen = OFF");
        }
    }

    public void RebootButton()
    {
        if (!rebootAct)
        {
            rebootButton.interactable = false;
            rebootAct = true;
            customCursor.CursorPointer();
            SFX_Button_1();
        }
    }

    public void DeluxeButton()
    {
        if (!deluxeAct)
        {
            deluxeButton.interactable = false;
            deluxeAct = true;
            customCursor.CursorPointer();
            SFX_Button_1();
        }
    }

    public void SFX_Flap()
    {
        SFXsource.clip = birdSFX;
        SFXsource.Play();
    }

    public void SFX_Button_1()
    {
        SFXsource.clip = button1_SFX;
        SFXsource.Play();
    }
    public void SFX_Button_2()
    {
        SFXsource.clip = button2_SFX;
        SFXsource.Play();
    }

    public void DebugVolume()
    {
        SFX_Button_1();
        Debug.Log("Volume = " + Mathf.RoundToInt(audioSource.volume * 100) + "%");
    }

    public void DebugResolution()
    {
        SFX_Button_1();
        Debug.Log("Resolution = " + resolutionDD.options[resolutionDD.value].text);
    }

    public void DebugUsername()
    {
        Debug.Log("Username = " + usernameInput.text);
    }

    public void DebugAllOptions()
    {
        if (!rebootAct && !deluxeAct)
        {
            string audio, fullscreen;
            string volume = "Volume        =   " + Mathf.RoundToInt(audioSource.volume * 100) + "%";
            string resolution = "Resolution   = " + resolutionDD.options[resolutionDD.value].text;
            string username = "Username   =   " + usernameInput.text;

            if (audioToggle.isOn)
            {
                audio = "Audio           =   ON";
            }
            else
            {
                audio = "Audio           =   OFF";
            }
            if (fsToggle.isOn)
            {
                fullscreen = "Fullscreen   =   ON";
            }
            else
            {
                fullscreen = "Fullscreen   =   OFF";
            }

            Debug.Log("[ Data Saved! Click here to see all options status ]\n\n====================\n" + audio + "\n" + volume + "\n" + fullscreen + "\n" + resolution + "\n" + username + "\n====================\n");
        }
    }

    public void Quit()
    {
        SFX_Button_2();
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
