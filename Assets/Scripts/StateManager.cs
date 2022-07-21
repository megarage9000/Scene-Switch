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
        // audioSource.Play();
    }

    void Update(){
        timer += Time.deltaTime;

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
                if(PhotonNetwork.LocalPlayer.ActorNumber > 1)
                    tmpText.text = "Place emoji on mannequin";
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

            if(timer > 24f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to next body part";

                if(HandPresence.bPressed){
                    currState = State.Face;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                    playOneShot = true;
                    timer = 0f;
                }
            }
        }
        else if(currState == State.Face && timer > 3f){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Face
                audioSource.clip = audioClips[3];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > 32f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to next body part";

                if(HandPresence.bPressed){
                    currState = State.Shoulders;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                    playOneShot = true;
                    timer = 0f;
                }
            }
        }


        else if(currState == State.Shoulders){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Shoulders
                audioSource.clip = audioClips[4];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > 15f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to next body part";

                if(HandPresence.bPressed){
                    currState = State.Arms;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                    playOneShot = true;
                    timer = 0f;
                }
            }
        }
        else if(currState == State.Arms){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Arms
                audioSource.clip = audioClips[5];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > 2f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to next body part";

                if(HandPresence.bPressed){
                    currState = State.Chest;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                    playOneShot = true;
                    timer = 0f;
                }
            }
        }
        else if(currState == State.Chest){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Chest
                audioSource.clip = audioClips[6];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > 11f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to next body part";

                if(HandPresence.bPressed){
                    currState = State.Stomach;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                    playOneShot = true;
                    timer = 0f;
                }
            }
        }
        else if(currState == State.Stomach){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Stomach
                audioSource.clip = audioClips[7];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > 12f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to next body part";

                if(HandPresence.bPressed){
                    currState = State.Body;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                    playOneShot = true;
                    timer = 0f;
                }
            }
        }
        else if(currState == State.Body){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Body
                audioSource.clip = audioClips[8];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > 12f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to next body part";

                if(HandPresence.bPressed){
                    currState = State.Emojis;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                    playOneShot = true;
                    timer = 0f;
                }
            }
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
