using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Character Player;

    [SerializeField]
    private float foffsetX = 0f;

    [SerializeField]
    private float foffsetY = 30f;
    
    [SerializeField]
    private float foffsetZ = -35f;

    Vector3 CameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //추적 카메라, 절차 애니메이션, 마지막으로 알려진 수집 정보를 이용하려면 LateUpdate를 이용하는것이 좋다.
    void LateUpdate()
    {
        if (Player == null)
            Player = BattleManager.GetInstance().player;

        if (Player != null)
        {
            CameraPosition.x = Player.transform.position.x + foffsetX;

            CameraPosition.y = Player.transform.position.y + foffsetY;

            CameraPosition.z = Player.transform.position.z + foffsetZ;

            transform.position = CameraPosition;
        }
    }
}
