using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MakerUI : MonoBehaviour
{
    #region serialize field
    [SerializeField]
    Image markerImage;//敵マーカー画像
    [SerializeField]
    GameObject enemy;
    #endregion

    #region field
    GameObject compass;//コンパスのゲームオブジェクト
    GameObject player;//プレイヤーゲームオブジェクト
  
    Image marker;//表示敵マーカー
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerTarget");


        //マーカーをレーダー上に表示
        compass = GameObject.Find("CompassMask");
        marker = Instantiate(markerImage, compass.transform.position, Quaternion.identity) as Image;
        marker.transform.SetParent(compass.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        //表示敵マーカーをレーダー上の該当位置に
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
