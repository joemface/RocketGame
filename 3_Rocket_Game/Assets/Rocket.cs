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


    [SerializeField] int currentState;
    [SerializeField] int nextState;
    Rigidbody rigidBody;
    AudioSource audio;
    enum State { Alive, Dying, Transcending, Developer }
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        // audio.PlayOneShot(ambiance);
        audio = GetComponent<AudioSource>();

        //reference to rigidbody. Rigidbody here is generics
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // todo stop sound after death
        if (state == State.Alive || state == State.Developer)
        {

            RespondToThrustInput();
            RespondToRotateInput();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
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
            case "Friendly":
                print("That tickles");
                break;
        }
    }
    private void StartDeathSequence()
    {
        if (state != State.Developer)
        {
            print("Huston, we have a problem. . .");
            // SceneManager.LoadScene(0);
            state = State.Dying;

            audio.Stop();
            audio.pitch = 1.3f;
            audio.PlayOneShot(alsoDeath);

            audio.PlayOneShot(death);

            Invoke("LoadFirstLevel", levelLoadDelay);
        }


    }
    private void StartWinSequence()
    {
        state = State.Transcending;
        audio.Stop();

        audio.PlayOneShot(win);
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextState);

    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(currentState);
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
    private void RespondToDevMode()
    {
        if (Input.GetKey(KeyCode.L))
        {
            state = State.Transcending;
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {

            state = State.Developer;

        }
        else if (Input.GetKey(KeyCode.V))
        {
            state = State.Alive;
        }
    }
    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //takes manual control of rotation
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

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }


}
