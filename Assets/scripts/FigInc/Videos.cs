using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        string url;

        #if UNITY_EDITOR
            string filePath = Path.Combine(Application.streamingAssetsPath + "/video/FigInc/", urlVideo + ".mp4");
        #elif UNITY_IOS
                        string filePath = Path.Combine (Application.streamingAssetsPath + "/Raw", urlVideo); 
        #elif UNITY_ANDROID
            string filePath = Path.Combine ("jar:file://" + Application.dataPath + "!assets/video/FigInc/", urlVideo + ".mp4");
        #endif
        /*
        if (Application.isMobilePlatform)
        {
          //  url = "jar:file://" + Application.persistentDataPath + "/StreamingAssets/video/FigInc/" + urlVideo + ".mp4";
            url = "jar:file://" + Application.streamingAssetsPath + "/video/FigInc/" + urlVideo + ".mp4";
        }
        else
        {
            url = "File://" + Application.dataPath + "/StreamingAssets/video/FigInc/" + urlVideo + ".mp4";
        }*/


        using (var www = new WWW(filePath)) {
            yield return www;          
            videoPlayer.GetComponent<VideoPlayer>().url = filePath;        
            videoPlayer.GetComponent<VideoPlayer>().playOnAwake = false;

            videoPlayer.GetComponent<VideoPlayer>().renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.GetComponent<VideoPlayer>().targetMaterialRenderer = GetComponent<Renderer>();
            videoPlayer.GetComponent<VideoPlayer>().targetMaterialProperty = "_MainTex";
            videoPlayer.GetComponent<VideoPlayer>().audioOutputMode = VideoAudioOutputMode.None;

            // Skip the first 100 frames.
            videoPlayer.GetComponent<VideoPlayer>().frame = 100;
            videoPlayer.GetComponent<VideoPlayer>().isLooping = true;
            videoPlayer.GetComponent<VideoPlayer>().playbackSpeed = 3f;
            videoPlayer.GetComponent<VideoPlayer>().audioOutputMode = VideoAudioOutputMode.None;
        
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
