using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleScene : MonoBehaviour
{
    public Text blinkText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�����L�[���������烁�C���V�[�����Ăяo��
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Main");
        }

        //���b�Z�[�W�_��
        blinkText.color = new Color(1,1,1,Mathf.PingPong(Time.time,1));


    }
}
