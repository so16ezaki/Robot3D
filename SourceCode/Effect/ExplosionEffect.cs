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

    //“GŒ‚”j‚Ì‹——£‚É‰‚¶‚Ä‰æ–Ê‚ğ—h‚ç‚·ˆ—
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            //‹——£‚É‚æ‚Á‚Äindenciry‚©‚¦‚é ”­Œõ
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            ScreenOverlayManager.intencity += Mathf.Clamp(1 - (distance / 200), 0, 1);

            //vibration‘—‚é@‰æ–Ê‚ä‚ê
            CamVibrationManager.vibration += Mathf.Clamp(0.5f - (distance / 200), 0, 0.5f);
           

            Destroy(gameObject);
        }
    }
}
