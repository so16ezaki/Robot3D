using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //•ûŠp‚Ì‰ñ“]
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, player.transform.eulerAngles.y);
    }
}
