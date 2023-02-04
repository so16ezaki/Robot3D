using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;  
using UnityStandardAssets.ImageEffects;

public class PlayerBehaviour : MonoBehaviour
{
    #region public field
    //アーマー系
    public int _ArmorPoint;//アーマーポイント
    public int armorPointMax = 5000;//アーマーポイント最大値


    //ブースト系
    public float boostPoint;//現在のブースト量
    public float boostPointMax = 100;//ブースト容量
    public float overHeatTime = 5;//オーバーヒート時間
    public float boostCoolingSpeed = 1.5f;//ブースト回復速度
    public float boostConsumSpeed = 1;//ブースト消費速度

    public bool isBoost;//ブースト状態
    public bool isOverHeat;//オーバーヒート状態

    public float boostTimer;//ブースト時間計測タイマー
    public float overHeatTimer;//オーバーヒート時間計測タイマー

    public GameObject audioSoruceWarning;//オーバーヒート時の警告音


    //旋回系
    public int rotateSpeed = 2;//旋回速度


    //ロックオン系
    public float LookAtEnemyDistance = 100;//敵を自動で向く限界距離
    public float LockOnDistance = 500;//敵をロックオンする限界距離

    public GameObject target = null;//ターゲット

    public bool isLockOn;//ロックオンモード状態


    //射撃系
    public GameObject shot;//弾
    public GameObject muzzle;//銃口オブジェクト
    public GameObject muzzleFlash;//マズルフラッシュエフェクトsss
    
    public float shotIntervalMax = 0.4f;//射撃間隔

    public AudioSource shootAudioSource;//発射音

    public GameObject cameraPivot;
    #endregion

    #region define
    const float gravity = 20f;//重力
    const float addNormalSpeed = 1; //通常時の加算速度
    const float addBoostSpeed = 2; //ブースト時の加算速度
    const float normalMaxSpeed = 20; //通常時の最大速度
    const float boostMaxSpeed = 40; //ブースト時の最大速度
    #endregion

    #region field
    //速度ベクトル
    float shotIntervalTimer = 0;//射撃間隔タイマー

    Vector3 moveDirection = Vector3.zero;
    Vector3 moveSpeed = Vector3.zero;
    CharacterController controller;//プレイヤーコントローラー
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _ArmorPoint = armorPointMax;//アーマーポイント初期設定
        boostPoint = boostPointMax;//ブーストポイント初期設定
        controller = GetComponent<CharacterController>();

        isLockOn = false;//
        shootAudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //水平移動処理
        MoveFunction();
        //回転処理
        RotateFunction();
        //ジャンプ処理
        JumpFunction();
     
        //ローカル座標のベクトルmoveDirectionをワールド座標系に変換
        moveDirection = transform.TransformDirection(moveDirection);
        //機体のコントローラーに移動ヘクトルを適用
        controller.Move(moveDirection * Time.deltaTime);

        //ブースト処理
        BoostFunction();
        //オーバーヒート処理
        OverHeatFunction();

        //モーションブラー処理
        motionBlurFunction();
        //カメラ振動処理
        CameraVibrationFunction();

     
        //ロックモード切替処理
        ChangeLockModeFunction();

        //ロックモードで敵がいれば敵の方向を見る
        if (isLockOn == true)
        {
            if (target != null)
                LookAtEnemyFunction();
            else
                target = FindClosestEnemy();//ロックオンモードでなければ敵を探す
        }

        //射撃処理
        ShootFunction();

    }
    #endregion

    #region private function
    /// <summary> 被弾処理 </summary>
    private void OnCollisionEnter(Collision collider)
    {
        //敵の弾と衝突    
        if (collider.gameObject.tag == "ShotEnemy")
        {
            //被弾ダメージ
            int damage = 0;
            //敵弾からダメージの値をもらう
            damage = collider.gameObject.GetComponent<EnemyShot>().damage;

            _ArmorPoint -= damage;
            //Mathf.Clamp(対象の変数,下限,上限)で指定の値に限度を調整する
            _ArmorPoint = Mathf.Clamp(_ArmorPoint, 0, armorPointMax);
        }
    }

    /// <summary> 水平移動処理 </summary>
    private void MoveFunction ()
    {
        float acceleration;//加速度
        Vector3 maxSpeed = Vector3.zero;//限界速度
       
        

        

        maxSpeed = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));//キーによる限界速度のベクトル取得
        maxSpeed = maxSpeed.normalized;//限界速度正規化

        //ブーストしているときと　していないときで加速度と限界速度を変える
        if (isBoost)
        {
            acceleration = addBoostSpeed;
            maxSpeed *= boostMaxSpeed; 
        }
        else
        {
            acceleration = addNormalSpeed;
            maxSpeed *= normalMaxSpeed;
        }

        //限界速度へ加速度をつかい加速
        moveSpeed.x = Mathf.MoveTowards(moveSpeed.x, maxSpeed.x, acceleration);
        moveSpeed.z = Mathf.MoveTowards(moveSpeed.z, maxSpeed.z, acceleration);
        

        moveDirection = new Vector3(moveSpeed.x, moveDirection.y, moveSpeed.z);
        
    }

    /// <summary> 回転処理 </summary>
    void RotateFunction()
    {
        //水平マウス操作
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

        //垂直マウス操作
        cameraPivot.transform.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
    }

    /// <summary> ジャンプ処理 </summary>
    void JumpFunction()
    {
        float maxAltitude = 100;//限界高度

        //地上にいるとき重力を発生させない
        if (controller.isGrounded)
            moveDirection.y = 0;

        //ジャンプキーが押されたとき　かつ　オーバーヒートでないときジャンプ
        if (Input.GetButton("Jump") && !isOverHeat)
        {
            if (transform.position.y > maxAltitude)
                moveDirection.y = 0;
            else
                moveDirection.y += gravity * Time.deltaTime;

            isBoost = true;

        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    /// <summary> ブースト処理 </summary>
    void BoostFunction()
    {
        //ブーストの状態をisBoostに定義
        if ((Input.GetButton("Boost") || Input.GetButton("Jump")) && !isOverHeat)
        {
            isBoost = true;
        }
        else
        {
            isBoost = false;
        }



        //ブースト消費と回復処理
        if (!isBoost && !isOverHeat)
            boostPoint += boostCoolingSpeed;
        else
            boostPoint -= boostConsumSpeed;

        boostPoint = Mathf.Clamp(boostPoint, 0, boostPointMax);
        
    }

    /// <summary> オーバーヒート処理 </summary>
    void OverHeatFunction()
    {
        //オーバーヒート状態をisOverHeatに定義
        if (boostPoint <= 1)
        {
            isOverHeat = true;
        }
        else
        {
            isOverHeat = false;
        }


        float warnigBoostRate = 0.3f;//警告音を出し始めるブースト割合
        //オーバーヒート警告音
        if (isOverHeat || boostPoint / boostPointMax < warnigBoostRate)
            audioSoruceWarning.SetActive(true);
        else
            audioSoruceWarning.SetActive(false);


        if (isOverHeat)
        {
            overHeatTimer += Time.deltaTime;
            //オーバーヒート解除処理
            if (overHeatTimer >= overHeatTime)
            {
                isOverHeat = false;
                overHeatTimer = 0f;
                boostPoint = boostPointMax;
            }
        }
    }

    /// <summary> モーションブラー処理 </summary>
    void motionBlurFunction()
    {

        //移動速度に合わせてモーションブラーの値を変える
        float motionBlurValue = Mathf.Max(Mathf.Abs(moveSpeed.x), Mathf.Abs(moveSpeed.z)) / 20;
        motionBlurValue = Mathf.Clamp(motionBlurValue, 0, 5) * 0.5f;


        Camera.main.GetComponent<CameraMotionBlur>().velocityScale = motionBlurValue;

    }

    /// <summary> カメラ振動処理 </summary>
    void CameraVibrationFunction()
    {

        //ブースト中画面を揺らす処理
        if (isBoost)
        {
            boostTimer += Time.deltaTime;

            //vibration送る
            CamVibrationManager.vibration = Mathf.Clamp(0.5f - (boostTimer), 0.05f, 0.3f);

        }
        else
            boostTimer = 0f;


    }

    /// <summary> 近距離の敵をターゲットにする処理 </summary>
    GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;

            }
        }



        return closest;
    }

    /// <summary> ロックオンモード切替処理 </summary>
    void ChangeLockModeFunction()
    {
        //キーによるロック切り替え
        if (Input.GetButtonDown("Lock"))
        {
            //ロックモード切替
            isLockOn = !isLockOn;

            //ロック解除
            if (!isLockOn)
                target = null;

        }



        //敵表示の距離によるロック解除
        if (target != null)
        {
            //ターゲットとの距離
            float targetDistance = Vector3.Distance(target.transform.position, transform.position);

            if (targetDistance > LockOnDistance)
            {
                target = null;
                isLockOn = false;

            }
        }

    }

    /// <summary> 敵へ自動で向く処理 </summary>
    void LookAtEnemyFunction()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < LookAtEnemyDistance)
        {
            //ターゲットに向く



            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);



            //機体を中心にマズルが敵を向くように横回転させる
            var addRotationY = Mathf.Asin(3.63f / (Vector3.Distance(target.transform.position, transform.position)));
            Quaternion QAddRotation = Quaternion.AngleAxis(-addRotationY * Mathf.Rad2Deg, Vector3.up);
            targetRotation *= QAddRotation;



            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

            //カメラピボットを中心に敵を向くように縦回転
            Quaternion targetRotation2 = Quaternion.LookRotation(target.transform.position - cameraPivot.transform.position);
            cameraPivot.transform.localRotation = Quaternion.Slerp(cameraPivot.transform.localRotation, targetRotation2, Time.deltaTime * 10);

            cameraPivot.transform.localRotation = new Quaternion(cameraPivot.transform.localRotation.x, 0, 0, cameraPivot.transform.localRotation.w);
        }
    }

    /// <summary> 射撃処理 </summary>
    void ShootFunction()
    {
        shotIntervalTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && !AllSceneManager.isEnd)
        {
            if (shotIntervalTimer > shotIntervalMax)
            {
                //弾をうつ
                Instantiate(shot, muzzle.transform.position, Camera.main.transform.rotation);
                shotIntervalTimer = 0;

                //まずるフラッシュだす
                Instantiate(muzzleFlash, muzzle.transform.position, muzzle.transform.rotation);

                //発射音
                shootAudioSource.PlayOneShot(shootAudioSource.clip);
            }
        }
    }

    #endregion
}
