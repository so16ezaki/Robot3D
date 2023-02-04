using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGrassDistanceInEditor : MonoBehaviour
{
    [SerializeField] float detailDistance = 250;//草の表示距離

    public Terrain terrain;//地形
    public Terrain terrain2;//地形
    public Terrain terrain3;//地形
    public Terrain terrain4;//地形
    

    // Start is called before the first frame update
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
        //草の表示距離処理
        terrain.detailObjectDistance = detailDistance;
        terrain2.detailObjectDistance = detailDistance;
        terrain3.detailObjectDistance = detailDistance;
        terrain4.detailObjectDistance = detailDistance;
    }
}
