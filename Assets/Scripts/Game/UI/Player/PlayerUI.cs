using UnityEngine;

public class PlayerUI : UIWindowParent
{
    GameObject _player = null;
    public override void SetUp()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        base.SetUp();
    }

    public override void UpDate()
    {
        if (_player == null) return;
        base.UpDate();
    }

    public override void CallBack(int id, object[] data)
    {
        if (_player == null) return;
        base.CallBack(id, data);
    }
}
