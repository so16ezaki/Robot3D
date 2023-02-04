using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MakerUI : MonoBehaviour
{
    #region serialize field
    [SerializeField]
    Image markerImage;//�G�}�[�J�[�摜
    [SerializeField]
    GameObject enemy;
    #endregion

    #region field
    GameObject compass;//�R���p�X�̃Q�[���I�u�W�F�N�g
    GameObject player;//�v���C���[�Q�[���I�u�W�F�N�g
  
    Image marker;//�\���G�}�[�J�[
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerTarget");


        //�}�[�J�[�����[�_�[��ɕ\��
        compass = GameObject.Find("CompassMask");
        marker = Instantiate(markerImage, compass.transform.position, Quaternion.identity) as Image;
        marker.transform.SetParent(compass.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        //�\���G�}�[�J�[�����[�_�[��̊Y���ʒu��
        Vector3 position = enemy.transform.position - player.transform.position;
        marker.transform.localPosition = new Vector3(position.x, position.z, 0);

    }
    #endregion

    #region private function
    private void OnDestroy()
    {
        Destroy(marker);
    }
    #endregion
}
