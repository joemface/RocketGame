using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;//SerializeField creates an adjustable number in unity
    [SerializeField] float upThrust = 2f;
    Rigidbody rigidBody;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        
        //reference to rigidbody. Rigidbody here is generics
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            
            rigidBody.AddRelativeForce(Vector3.up * upThrust);
            if (!audio.isPlaying)
            {
                GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().Play(44100);
            }
        }
        else
        {
            audio.Stop();
        }
    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true; //takes manual controal of rotation
        
        //f to let the program know ahead of time it's a float 10
        float rotateThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotateThisFrame);

            //this rotates the ship towards you..easter egg?
            //transform.Rotate(Vector3.left);
        }else if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(Vector3.back * rotateThisFrame);
        }
        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

    
}
