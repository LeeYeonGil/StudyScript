using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineControl : Singleton<TimeLineControl>
{
    public PlayableDirector playableDirector;

    public void PDPlayer()
    {
        playableDirector.Play();
    }
}
