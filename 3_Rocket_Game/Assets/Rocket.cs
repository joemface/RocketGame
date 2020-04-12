using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Rocket : MonoBehaviour
{
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
        ProcessInput();
    }

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            rigidBody.AddRelativeForce(Vector3.up);
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
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
            
            //this rotates the ship towards you..easter egg?
            //transform.Rotate(Vector3.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back);
        }
    }

}
