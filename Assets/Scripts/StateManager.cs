using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public enum State : ushort
{
    Intro,
    Mannequin,
    Head,
    Face,
    Shoulders,
    Arms,
    Chest,
    Stomach,
    Body,
    Emojis,
    Eword,
    Resize,
    End,
}

public class StateManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] GameObject mannequin;
    [SerializeField] TMP_Text tmpText;

    private AudioSource audioSource;
    private State prevState = 0;
    private State currState = 0;
    private bool playOneShot = false;
    private float timer = 0f;


    void Start(){
        tmpText.text = "";
        audioSource = this.GetComponent<AudioSource>();
        
        Debug.Log("STATE: "  + currState); // STATE: Intro
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    void Update(){
        timer += Time.deltaTime;
        // Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);

        if(currState == State.Intro && !audioSource.isPlaying){
            currState = State.Mannequin;
            playOneShot = true;
            timer = 0f;
        }
        else if(currState == State.Mannequin && timer > 3f){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Mannequin
                mannequin.SetActive(true);
                audioSource.clip = audioClips[1];
                audioSource.Play();
                playOneShot = false;
            }
            
            if(!audioSource.isPlaying){
                currState = State.Head;
                //end of audio put purple subtitles
                playOneShot = true;
                timer = 0f;
            }
        }
        else if(currState == State.Head && timer > 3f){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Head
                audioSource.clip = audioClips[2];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > 54f){
                //make green player prompt appear

                // if B clicked
                    // currState = State.Face;
                    // turn prompt off
                    // playOneShot = true;
                    // timer = 0f;
            }
        }


        else if(currState == State.Face && timer > 3f){
            Debug.Log("STATE: "  + currState); // STATE: Face
            prevState = currState;
        }
        else if(currState == State.Shoulders){
            Debug.Log("STATE: "  + currState); // STATE: Shoulders
            prevState = currState;
        }
        else if(currState == State.Arms){
            Debug.Log("STATE: "  + currState); // STATE: Arms
            prevState = currState;
        }
        else if(currState == State.Chest){
            Debug.Log("STATE: "  + currState); // STATE: Chest
            prevState = currState;
        }
        else if(currState == State.Stomach){
            Debug.Log("STATE: "  + currState); // STATE: Stomach
            prevState = currState;
        }
        else if(currState == State.Body){
            Debug.Log("STATE: "  + currState); // STATE: Body
            prevState = currState;
        }
        else if(currState == State.Emojis){
            Debug.Log("STATE: "  + currState); // STATE: Emojis
            prevState = currState;
        }
        else if(currState == State.Eword){
            Debug.Log("STATE: "  + currState); // STATE: Eword
            prevState = currState;
        }
        else if(currState == State.Resize){
            Debug.Log("STATE: "  + currState); // STATE: Resize
            prevState = currState;
        }
        else if(currState == State.End){
            Debug.Log("STATE: "  + currState); // STATE: End
            prevState = currState;
        }
    }
}
