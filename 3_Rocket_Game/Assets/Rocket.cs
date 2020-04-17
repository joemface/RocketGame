using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 200f;//SerializeField creates an adjustable number in unity
    [SerializeField] float upThrust = 2f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip alsoDeath;
    [SerializeField] AudioClip win;
    Rigidbody rigidBody;
    AudioSource audio;
    bool isTransitioning = false;
    bool collisionsAreDisabled = false;
    // Start is called before the first frame update
    /*******************************************
* START
*
*
*
************************************************/
    void Start()
    {
        // audio.PlayOneShot(ambiance);
        audio = GetComponent<AudioSource>();

        //reference to rigidbody. Rigidbody here is generics
        rigidBody = GetComponent<Rigidbody>();
    }
    /*******************************************
    * UPDATE
    *
    *
    *
    ************************************************/
    // Update is called once per frame
    void Update()
    {
        // todo stop sound after death
        if (!isTransitioning)
        {

            RespondToThrustInput();
            RespondToRotateInput();
            if (Debug.isDebugBuild)
                RespondToDevMode();

        }

    }

    /*******************************************
    * RESPOND TO DEVELOPER MODE
    *
    *
    *
    ************************************************/
    private void RespondToDevMode()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            // state = State.Transcending;
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsAreDisabled = !collisionsAreDisabled;//toggle on and off
        }
    }

    /*******************************************
    * COLLISION DETECTOR
    *
    *
    *
    ************************************************/
    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsAreDisabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Launch":
                print("Prepare for launch");
                break;
            case "Danger":
                StartDeathSequence();
                break;
            case "Finish":
                StartWinSequence();
                break;
        }
    }

    /*******************************************
    * START DEATH SEQUENCE
    *
    *
    *
    ************************************************/
    private void StartDeathSequence()
    {
        isTransitioning = true;
        print("Huston, we have a problem. . .");
        // SceneManager.LoadScene(0);

        audio.Stop();
        audio.pitch = 1.3f;
        audio.PlayOneShot(alsoDeath);

        audio.PlayOneShot(death);

        Invoke("ReloadLevel", levelLoadDelay);


    }
    /*******************************************
    * START WIN SEQUENCE
    *
    *
    *
    ************************************************/
    private void StartWinSequence()
    {
        isTransitioning = true;
        audio.Stop();

        audio.PlayOneShot(win);
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    /*******************************************
    * LOAD NEXT LEVEL
    *
    *
    *
    ************************************************/
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;
        SceneManager.LoadScene(nextSceneIndex);

    }
    /*******************************************
    * RELOAD LEVEL
    *
    *
    *
    ************************************************/
    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void RespondToThrustInput()
    {

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            ApplyThrust();

        }
        else
        {
            audio.Stop();

        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * (upThrust * Time.deltaTime));
        ThrustSound();


    }
    private void ThrustSound()
    {
        if (!audio.isPlaying)
        {
            audio.PlayOneShot(mainEngine);
        }
    }
    private void RespondToDeath()
    {
        audio.PlayOneShot(death);
    }
    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero; //takes manual control of rotation
        //f to let the program know ahead of time it's a float 10
        float rotateThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotateThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotateThisFrame);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.right);
        }
        else if (Input.GetKey(KeyCode.R))
        {
            ReloadLevel();
        }


        rigidBody.freezeRotation = false; //resume physics control of rotation
    }


}
