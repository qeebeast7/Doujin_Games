using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData{
    private static UserData instance;
    public static UserData Instance
    {
        get
        {
            if (instance == null) instance = new UserData();
            return instance;
        }
    }

    public Vector3 petPos;
    public Vector3 setPos;
    public float scale;
    public float walkSpeed;
    public float wkMin;
    public float wkMax;
    public float tkMin;
    public float tkMax;
    public bool isRw;
    public bool isTs;
    public int turn;

    public UserData()
    {

    }
    public UserData(Vector3 _petPos,Vector3 _setPos,float _scale,float _walkSpeed,
        float _wkMin,float _wkMax,float _tkMin,float _tkMax,bool _isRw,bool _isTs,int _turn)
    {
        petPos = _petPos;
        setPos = _setPos;
        scale = _scale;
        walkSpeed = _walkSpeed;
        wkMin = _wkMin;
        wkMax = _wkMax;
        tkMin = _tkMin;
        tkMax = _tkMax;
        isRw = _isRw;
        isTs = _isTs;
        turn = _turn;
    }
}
