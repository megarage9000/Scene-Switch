using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public enum State : short
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

    [SerializeField] State currState = State.Intro;
    [SerializeField] float timer;

    private AudioSource audioSource;
    private bool playOneShot = true;
    private PhotonView pv;


    [PunRPC]
    void ChangeState(short s){
        timer = 0f;
        playOneShot = true;
        audioSource.Stop();
        currState = (State)s;
    }

    void Start(){
        tmpText.text = "";
        audioSource = GetComponent<AudioSource>();
        pv = GetComponent<PhotonView>();
        
        Debug.Log("STATE: "  + currState); // STATE: Intro
        audioSource.clip = audioClips[3];
        timer = 0f;
    }

    void Update(){
        timer += Time.deltaTime;
        Debug.Log("CURRENT STATE: " + currState); // STATE: Intro

        if(currState == State.Intro && timer > 3f){
            if(playOneShot){
                audioSource.Play();
                playOneShot = false;
            }

            if(!audioSource.isPlaying){
                // currState = State.Mannequin;
                pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Mannequin);
            }
        }
        else if(currState == State.Mannequin && timer > 3f){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Mannequin
                mannequin.SetActive(true);
                audioSource.clip = audioClips[4];
                audioSource.Play();
                playOneShot = false;
            }
            
            if(!audioSource.isPlaying){
                // currState = State.Head;
                pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Head);
                if(PhotonNetwork.LocalPlayer.ActorNumber > 1)
                    tmpText.text = "Place an emoji on the mannequin";
            }
        }
        else if(currState == State.Head && timer > 3f){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Head
                mannequin.transform.Find("Head").GetComponent<Outline>().enabled = true;
                audioSource.clip = audioClips[5];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > /*30f*/ + 35f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to the next body part";

                if(HandPresence.bPressed){
                    // currState = State.Face;
                    pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Face);
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                }
            }
        }
        else if(currState == State.Face && timer > 3f){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Face
                audioSource.clip = audioClips[6];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > /*30f*/ + 23f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to the next body part";

                if(HandPresence.bPressed){
                    // currState = State.Shoulders;
                    pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Shoulders);
                    mannequin.transform.Find("Head").GetComponent<Outline>().enabled = false;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                }
            }
        }
        else if(currState == State.Shoulders){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Shoulders
                audioSource.clip = audioClips[7];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > /*30f*/ + 20f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to the next body part";

                if(HandPresence.bPressed){
                    // currState = State.Arms;
                    pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Arms);
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                }
            }
        }
        else if(currState == State.Arms){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Arms
                mannequin.transform.Find("Arms").GetComponent<Outline>().enabled = true;
                audioSource.clip = audioClips[8];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > /*30f*/ + 7f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to the next body part";

                if(HandPresence.bPressed){
                    // currState = State.Chest;
                    pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Chest);
                    mannequin.transform.Find("Arms").GetComponent<Outline>().enabled = false;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                }
            }
        }
        else if(currState == State.Chest){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Chest
                mannequin.transform.Find("Torso").GetComponent<Outline>().enabled = true;
                audioSource.clip = audioClips[9];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > /*30f*/ + 17f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to the next body part";

                if(HandPresence.bPressed){
                    // currState = State.Stomach;
                    pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Stomach);
                    mannequin.transform.Find("Torso").GetComponent<Outline>().enabled = false;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                }
            }
        }
        else if(currState == State.Stomach){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Stomach
                mannequin.transform.Find("Stomach").GetComponent<Outline>().enabled = true;
                audioSource.clip = audioClips[10];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > /*30f*/ + 17f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to move to the next body part";

                if(HandPresence.bPressed){
                    // currState = State.Body;
                    pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Body);
                    mannequin.transform.Find("Stomach").GetComponent<Outline>().enabled = false;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                }
            }
        }
        else if(currState == State.Body){
            if(playOneShot){
                Debug.Log("STATE: "  + currState); // STATE: Body
                mannequin.transform.Find("Arms").GetComponent<Outline>().enabled = true;
                mannequin.transform.Find("Extras").GetComponent<Outline>().enabled = true;
                mannequin.transform.Find("Head").GetComponent<Outline>().enabled = true;
                mannequin.transform.Find("Joints").GetComponent<Outline>().enabled = true;
                mannequin.transform.Find("Legs").GetComponent<Outline>().enabled = true;
                mannequin.transform.Find("Stomach").GetComponent<Outline>().enabled = true;
                mannequin.transform.Find("Torso").GetComponent<Outline>().enabled = true;
                audioSource.clip = audioClips[11];
                audioSource.Play();
                playOneShot = false;
            }

            if(timer > /*30f*/ + 17f){
                if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    tmpText.text = "Click (B) to go to the next exercise";

                if(HandPresence.bPressed){
                    // currState = State.Emojis;
                    pv.RPC("ChangeState", RpcTarget.AllBufferedViaServer, (short)State.Emojis);
                    mannequin.transform.Find("Arms").GetComponent<Outline>().enabled = false;
                    mannequin.transform.Find("Extras").GetComponent<Outline>().enabled = false;
                    mannequin.transform.Find("Head").GetComponent<Outline>().enabled = false;
                    mannequin.transform.Find("Joints").GetComponent<Outline>().enabled = false;
                    mannequin.transform.Find("Legs").GetComponent<Outline>().enabled = false;
                    mannequin.transform.Find("Stomach").GetComponent<Outline>().enabled = false;
                    mannequin.transform.Find("Torso").GetComponent<Outline>().enabled = false;
                    if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
                        tmpText.text = "";
                }
            }
        }



        else if(currState == State.Emojis){
            Debug.Log("STATE: "  + currState); // STATE: Emojis
        }
        else if(currState == State.Eword){
            Debug.Log("STATE: "  + currState); // STATE: Eword
        }
        else if(currState == State.Resize){
            Debug.Log("STATE: "  + currState); // STATE: Resize
        }
        else if(currState == State.End){
            Debug.Log("STATE: "  + currState); // STATE: End
        }
    }
}
