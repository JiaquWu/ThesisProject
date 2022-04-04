using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonManager<AudioManager> {
    [SerializeField]
    private AudioSource playerMoveAudio;
    [SerializeField]
    private AudioSource iceBreakAudio;
    [SerializeField]
    private AudioSource playerDrownAudio;
    [SerializeField]
    private AudioSource playerPickUpAnimalAudio;
    [SerializeField]
    private AudioSource playerFinishLevelAudio;
    [SerializeField]
    private AudioSource playerPutDownAnimalOnBoatAudio;
    [SerializeField]
    private AudioSource uiClickAudio;
    
    public void PlayPlayerMoveAudio() {
        if(playerMoveAudio != null) playerMoveAudio.Play();
    }
    public void PlayIceBreakAudio() {
        if(iceBreakAudio != null) iceBreakAudio.Play();
    }
    public void PlayPlayerDrownAudio() {
        if(playerDrownAudio != null) playerDrownAudio.Play();
    }
    public void PlayPlayerPickUpAnimalAudio() {
        if(playerPickUpAnimalAudio != null) playerPickUpAnimalAudio.Play();
    }
    public void PlayplayerFinishLevelAudio() {
        if(playerFinishLevelAudio != null) playerFinishLevelAudio.Play();
    }
    public void PlayplayerPutDownAnimalOnBoatAudio() {
        if(playerPutDownAnimalOnBoatAudio != null) playerPutDownAnimalOnBoatAudio.Play();
    }
    public void PlayUIClickAudio() {
        if(uiClickAudio != null) uiClickAudio.Play();
    }

}
