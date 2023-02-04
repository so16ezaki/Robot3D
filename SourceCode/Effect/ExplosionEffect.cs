using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�G���j���̋����ɉ����ĉ�ʂ�h�炷����
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            //�����ɂ����indenciry������ ����
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            ScreenOverlayManager.intencity += Mathf.Clamp(1 - (distance / 200), 0, 1);

            //vibration����@��ʂ��
            CamVibrationManager.vibration += Mathf.Clamp(0.5f - (distance / 200), 0, 0.5f);
           

            Destroy(gameObject);
        }
    }
}
