using Photon.Voice;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGMicWrapper : IAudioReader<float>
{

    public int SamplingRate
    {
        get
        {
            AudioClip clip = GGMicrophone.Instance.GetMicrophoneAudioClip();
            return clip.frequency;
        }
    }

    public int Channels
    {
        get
        {
            AudioClip clip = GGMicrophone.Instance.GetMicrophoneAudioClip();
            return clip.channels;
        }
    }

    public string Error { get; private set; }

    public void Dispose()
    {
        GGMicrophone.Instance.StopMicrophone();
    }

    private int micPrevPos;
    private int micLoopCnt;
    private int readAbsPos;

    public bool Read(float[] buffer)
    {
        Debug.Log("OLILOG Reading GGMicWrapper");
        if (Error != null)
        {
            return false;
        }
        int micPos = GGMicrophone.Instance.GetPosition();
        // loop detection
        if (micPos < micPrevPos)
        {
            micLoopCnt++;
        }
        micPrevPos = micPos;

        AudioClip clip = GGMicrophone.Instance.GetMicrophoneAudioClip();

        var micAbsPos = micLoopCnt * clip.samples + micPos;

        if (clip.channels == 0)
        {
            Error = "Number of channels is 0 in Read()";
            //logger.LogError("[PV] MicWrapper: " + Error);
            return false;
        }
        var bufferSamplesCount = buffer.Length / clip.channels;

        var nextReadPos = this.readAbsPos + bufferSamplesCount;
        if (nextReadPos < micAbsPos)
        {
            clip.GetData(buffer, this.readAbsPos % clip.samples);
            this.readAbsPos = nextReadPos;
            return true;
        }
        else
        {
            return false;
        }
    }


}
