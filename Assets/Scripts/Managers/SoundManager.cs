using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using INeverFall.Util;
using UnityEngine;

namespace INeverFall
{
    public class SoundManager : Singleton<SoundManager>
    {
        private List<string> _playingAudioList = new();

        public float Volume = 0.7f;

        private AudioSource _GetAudio(string name)
        {
            return transform.FindChildRecursively(name).GetComponent<AudioSource>();
        }

        public void PlayAudio(string name, string prevName = null)
        {
            if (prevName != null)
            {
                var prevAudio = _GetAudio(prevName);

                if (prevAudio != null)
                {
                    prevAudio.DOFade(0f, 0.5f).OnComplete(() =>
                    {
                        prevAudio.Stop();
                        prevAudio.volume = Volume;
                        
                        var audio = _GetAudio(name);
                        audio.Play();
                        audio.volume = Volume;
                        _playingAudioList.Add(name);
                    });
                }
            }
            else
            {
                var audio = _GetAudio(name);
                audio.Play();
                audio.volume = Volume;
                _playingAudioList.Add(name);
            }

        }

        public void StopAudio(string name)
        {
            var audio = _GetAudio(name);
            audio.Stop();
            _playingAudioList.Remove(name);
        }

        public void StopAll()
        {
            foreach (var audioName in _playingAudioList)
            {
                var audio = _GetAudio(audioName);
                audio.Stop();
            }

            _playingAudioList.Clear();
        }
    }
}