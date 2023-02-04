using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 画面が揺れる処理 </summary>
public class CamVibrationManager : MonoBehaviour
{
    public static float vibration;//揺れる度合い

    Vector3 defaultPosition;//元の位置

    // Start is called before the first frame update
    void Start()
    {
        vibration = 0;

        //初期値記憶
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //揺れ度合いに制限を設ける
        vibration = Mathf.Clamp(vibration, 0,0.5f);

        VibrationFunction();

    }

    void VibrationFunction()
    {
        if (vibration > 0)
        {
            //カメラポジションをランダムに揺らす
            Vector3 randomPosition;
            randomPosition.x = Random.Range(-vibration, vibration);
            randomPosition.y = Random.Range(-vibration, vibration);
            randomPosition.z = Random.Range(-vibration, vibration);

            transform.localPosition = defaultPosition + randomPosition;

            vibration -= Time.deltaTime / 2;

        }
        else
        {
            transform.localPosition = defaultPosition;
        }
    }
}
