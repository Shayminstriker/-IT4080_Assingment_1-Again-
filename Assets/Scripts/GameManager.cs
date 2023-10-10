using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        //Debug.Log("start of start");
      //  if (Application.isEditor) {
       // }
        //else {
         //   Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            //Debug.Log("after disabled");
       // }
        //Debug.Log("end of start");
    }

   private void OnGUI() {
        NetworkHelper.GUILayoutNetworkControls();
    }
}
