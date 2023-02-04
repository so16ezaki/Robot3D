using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnGroundCamera : MonoBehaviour
{
    [SerializeField]
    bool isOnTrigger = false;

    [SerializeField]
    GameObject cameraPosionObj;

    [SerializeField]
    GameObject camera1;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = camera1.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float cameraOffset = 4f;
        var targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + cameraOffset);
        if (isOnTrigger)
        {
            camera1.transform.localPosition = Vector3.MoveTowards(camera1.transform.localPosition, targetPosition, 0.2f);
        }
        else
        {
            camera1.transform.localPosition = Vector3.MoveTowards(camera1.transform.localPosition, startPosition, 0.2f);
        }

        //地面検知トリガーをカメラに合わせて動かす
        transform.position = cameraPosionObj.transform.position;
        transform.rotation = cameraPosionObj.transform.rotation;

        if (!isOnTrigger)
            isOnTrigger = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Terrain")
            isOnTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
            isOnTrigger = false;
    }

}
