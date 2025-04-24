using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    //��Ч
    public AudioSource dropItemSound; //������Ʒ
    public AudioSource craftingSound; //��Ʒ����
    public AudioSource toolsSwingSound; //����ҡ��
    public AudioSource chopSound; //����
    public AudioSource pickupItemSound; //ʰȡ��Ʒ
    public AudioSource grassWalkSound;//�ݵ�����

    //����
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
