using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGrassDistanceInEditor : MonoBehaviour
{
    [SerializeField] float detailDistance = 250;//���̕\������

    public Terrain terrain;//�n�`
    public Terrain terrain2;//�n�`
    public Terrain terrain3;//�n�`
    public Terrain terrain4;//�n�`
    

    // Start is called before the first frame update
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
        //���̕\����������
        terrain.detailObjectDistance = detailDistance;
        terrain2.detailObjectDistance = detailDistance;
        terrain3.detailObjectDistance = detailDistance;
        terrain4.detailObjectDistance = detailDistance;
    }
}
