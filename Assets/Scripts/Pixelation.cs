using UnityEngine;


// http://iaindowling.weebly.com/blog/unity-pixelated-render-effect
// Answer by Cherno · Mar 06, 2015 at 01:43 PM
// http://answers.unity3d.com/questions/916790/is-it-possible-to-make-unitys-camera-render-in-chu.html
public class Pixelation : MonoBehaviour {

    public RenderTexture renderTexture;

    void Start() {
        int realRatio = Mathf.RoundToInt(Screen.width / Screen.height);
        renderTexture.width = NearestSuperiorPowerOf2(Mathf.RoundToInt(renderTexture.width * realRatio));
        Debug.Log("(Pixelation)(Start)renderTexture.width: " + renderTexture.width);
    }

    void OnGUI() {
        GUI.depth = 20;
        GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);
    }

    int NearestSuperiorPowerOf2( int n ) {
        return (int) Mathf.Pow( 2, Mathf.Ceil( Mathf.Log( n ) / Mathf.Log( 2 ) ) );
    } 
}
