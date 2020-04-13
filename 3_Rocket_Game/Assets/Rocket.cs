using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 200f;//SerializeField creates an adjustable number in unity
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

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Launch":
                print("Prepare for launch");
                break;
            case "Danger":
                print("Huston, we have a problem. . .");
                break;
            case "Finish":
                //do nothing
                print("Mission Success!");
                break;
            case "Fuel":
                print("Used");
                break;
        }
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
        rigidBody.freezeRotation = true; //takes manual control of rotation
        
        //f to let the program know ahead of time it's a float 10
        float rotateThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotateThisFrame);

            
        }else if (Input.GetKey(KeyCode.D))
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
