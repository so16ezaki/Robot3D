using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> ��ʂ��h��鏈�� </summary>
public class CamVibrationManager : MonoBehaviour
{
    public static float vibration;//�h���x����

    Vector3 defaultPosition;//���̈ʒu

    // Start is called before the first frame update
    void Start()
    {
        vibration = 0;

        //�����l�L��
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //�h��x�����ɐ�����݂���
        vibration = Mathf.Clamp(vibration, 0,0.5f);

        VibrationFunction();

    }

    void VibrationFunction()
    {
        if (vibration > 0)
        {
            //�J�����|�W�V�����������_���ɗh�炷
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
