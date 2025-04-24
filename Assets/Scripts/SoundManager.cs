using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    //音效
    public AudioSource dropItemSound; //放置物品
    public AudioSource craftingSound; //物品制作
    public AudioSource toolsSwingSound; //工具摇摆
    public AudioSource chopSound; //砍伐
    public AudioSource pickupItemSound; //拾取物品
    public AudioSource grassWalkSound;//草地漫步

    //音乐
    public AudioSource startingZoneBGMusic; 


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }


}
