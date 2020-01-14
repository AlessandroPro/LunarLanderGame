using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Represents a LunarLander, which is controllable by the user
public class LunarLander : MonoBehaviour
{
    public float forceRate;
    public float torqueRate;
    public ParticleSystem thruster;
    public ParticleSystem crashSmoke;
    public float fuel;
    public float fuelRate;
    public GameObject mainCam;
    public GameUIManager gameUI;
    public StageBuilder stageBuilder;

    private ConstantForce constForce;
    private float validTimeOnPlatform;
    private int score;

    //flags
    private bool isControllable;
    private bool hasFuel;
    private bool isDestroyed;
    private bool isOnPlatform;

    //audio
    public AudioSource audioSource;
    public AudioClip thrusterSound;
    public AudioClip crashSound;
    public AudioClip landSuccesSound;


    // Start is called before the first frame update
    void Start()
    {
        constForce = GetComponent<ConstantForce>();
        
        validTimeOnPlatform = 0f;
        score = 0;
        gameUI.UpdateFuelText(fuel);
        gameUI.UpdateScoreText(score);

        isControllable = true;
        hasFuel = true;

        // Set position to random spot above the moon's surface
        transform.position = new Vector3(Random.Range(10f, 60f), 35f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasFuel && isControllable)
        {
            HandleInput();
        }

        if(hasFuel && fuel <= 0)
        {
            HandleEmptyFuel();
        }
    }

    public void Crash()
    {
        if(isControllable)
        {
            isControllable = false;
            crashSmoke.Play();

            foreach (Transform child in transform)
            {
                LanderComponent lc = child.gameObject.GetComponent<LanderComponent>();

                if(lc != null)
                {
                    lc.BreakOff();
                }
            }
            mainCam.GetComponent<CameraController>().StartFocusing();

            thruster.Stop();
            audioSource.clip = crashSound;
            audioSource.Play();
            gameUI.SetupEndGame();
        }
    }


    public void WinPoints(int points)
    {
        score += points;
        gameUI.UpdateScoreText(score);
        gameUI.UpdateMessageText("+ " + points.ToString() + " POINTS!");

        validTimeOnPlatform = 0f;
        isControllable = false;
        mainCam.GetComponent<CameraController>().StartFocusing();
        audioSource.clip = landSuccesSound;
        audioSource.Play();

        Invoke("Reset", 3f);
    }

    public void Reset()
    {
        // Continue playing, but if there is no more fuel, just end the game
        if (hasFuel)
        {
            constForce.force = Vector3.zero;
            constForce.torque = Vector3.zero;

            // Set position to random spot above the moon's surface
            transform.position = new Vector3(Random.Range(10f, 60f), 35f, 0);

            validTimeOnPlatform = 0;
            mainCam.GetComponent<CameraController>().ResetFocus();
            stageBuilder.RebuildStage();
            isControllable = true;
            gameUI.ShowMessageText(false);
        }
        else
        {
            gameUI.SetupEndGame();
        }
    }

    // Handle user input
    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            constForce.force = transform.up * forceRate;
            fuel += fuelRate * Time.deltaTime;
            gameUI.UpdateFuelText(fuel);
        }
        else
        {
            constForce.force = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            constForce.torque = torqueRate * Vector3.forward;
            fuel += (fuelRate / 10f) * Time.deltaTime;
            gameUI.UpdateFuelText(fuel);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            constForce.torque = torqueRate * Vector3.back;
            fuel += (fuelRate / 10f) * Time.deltaTime;
            gameUI.UpdateFuelText(fuel);
        }
        else
        {
            constForce.torque = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            thruster.Play();
            audioSource.clip = thrusterSound;
            audioSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            thruster.Stop();
            audioSource.Stop();
            if (audioSource.clip == thrusterSound)
            {
                audioSource.Stop();
            }
        }
    }

    private void HandleEmptyFuel()
    {
        fuel = 0;
        hasFuel = false;
        thruster.Stop();
        constForce.force = Vector3.zero;
        constForce.torque = Vector3.zero;
        gameUI.UpdateFuelText(fuel);

        if (audioSource.clip == thrusterSound)
        {
            audioSource.Stop();
        }

        gameUI.UpdateMessageText("OUT OF FUEL");
    }

    public void OnTriggerEnter(Collider other)
    {
        var platform = other.gameObject.GetComponent<Platform>();
        if (platform)
        {
            isOnPlatform = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var platform = other.gameObject.GetComponent<Platform>();
        if (platform)
        {
            validTimeOnPlatform = 0;
            isOnPlatform = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var platform = other.gameObject.GetComponent<Platform>();
        if (platform)
        {
            float angle = transform.rotation.eulerAngles.z;

            // ensures the lander is not rotated much when touching a platform
            if (angle > 359f || angle < 1f)
            {
                validTimeOnPlatform += Time.deltaTime;
            }
            else
            {
                validTimeOnPlatform = 0;
            }

            // the landing is considered successful if the lander
            // has remained safely on the platform for at least 2 seconds
            if (validTimeOnPlatform > 2f && isControllable)
            {
                WinPoints(platform.value);
            }
        }
        else if (other.gameObject.GetComponent<Collidable>())
        {
            // If the lander has ran out of fuel and lands on a non-platform
            // surface, then just crash the lander
            if(!hasFuel && !isOnPlatform && isControllable)
            {
                Crash();
            }
        }
    }
}
