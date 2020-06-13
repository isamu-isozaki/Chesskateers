using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonHandler : MonoBehaviour
{
    public Vector3 TargetPos = new Vector3 (5, 5, 5);
    public Quaternion TargetRot = Quaternion.Euler(0, -45, 0);
    public Vector3 OriginalPos = new Vector3(92, 67, -75);
    public Quaternion OriginalRot = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    public float delayTime;

    public void SetCamPos(GameObject TargetCamera)
    {
        Camera.main.transform.position = TargetPos;
        Camera.main.transform.rotation = TargetRot;
        StartCoroutine(ResetCamPos(TargetCamera, delayTime));
        //ResetCamPos();
    }
    public IEnumerator ResetCamPos(GameObject TargetCamera, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Camera.main.transform.position = OriginalPos;
        Camera.main.transform.rotation = OriginalRot;
    }
}
