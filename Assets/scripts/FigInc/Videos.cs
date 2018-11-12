using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Videos : MonoBehaviour {
    VideoClip video;

    public void AbrirVideo() {
        StartCoroutine("Cargar");
    }

    public IEnumerator Cargar(){
        string urlVideo = GameObject.Find("txtFind").GetComponent<Text>().text;
        Debug.Log("URLVideo: " + urlVideo);
        GameObject videoPlayer = GameObject.Find("Visor");
        
        videoPlayer.AddComponent<VideoPlayer>();
        
        string url = "File://" + Application.dataPath + "/StreamingAssets/video/FigInc/" + urlVideo + ".wmv";

        using (var www = new WWW(url)) {
            yield return www;
            videoPlayer.GetComponent<VideoPlayer>().url = url;        
            videoPlayer.GetComponent<VideoPlayer>().playOnAwake = false;

            videoPlayer.GetComponent<VideoPlayer>().renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.GetComponent<VideoPlayer>().targetMaterialRenderer = GetComponent<Renderer>();
            videoPlayer.GetComponent<VideoPlayer>().targetMaterialProperty = "_MainTex";
            videoPlayer.GetComponent<VideoPlayer>().audioOutputMode = VideoAudioOutputMode.None;

            // Skip the first 100 frames.
            videoPlayer.GetComponent<VideoPlayer>().frame = 100;
            videoPlayer.GetComponent<VideoPlayer>().isLooping = true;
            videoPlayer.GetComponent<VideoPlayer>().playbackSpeed = 3f;
            // Each time we reach the end, we slow down the playback by a factor of 10.
            //videoPlayer.GetComponent<VideoPlayer>().loopPointReached += EndReached;
            videoPlayer.GetComponent<VideoPlayer>().Play();
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }

    public void DestruirVideo() {
        Destroy(GameObject.Find("Visor").GetComponent<VideoPlayer>());        
    }
}
