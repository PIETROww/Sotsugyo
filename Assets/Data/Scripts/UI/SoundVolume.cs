using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;

    void Start()
    {
        //�X���C�_�[�𓮂��������̏�����o�^
        BGMSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        SESlider.onValueChanged.AddListener(SetAudioMixerSE);
    }

    //BGM
    public void SetAudioMixerBGM(float value)
    {
        //5�i�K�␳
        value /= 5;
        //-80~0�ɕϊ�
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        //audioMixer�ɑ��
        audioMixer.SetFloat("BGM", volume);
        Debug.Log($"BGM:{volume}");
    }

    //SE
    public void SetAudioMixerSE(float value)
    {
        //5�i�K�␳
        value /= 5;
        //-80~0�ɕϊ�
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        //audioMixer�ɑ��
        audioMixer.SetFloat("SE", volume);
        Debug.Log($"SE:{volume}");
    }
}
