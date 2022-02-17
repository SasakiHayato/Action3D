using UnityEngine;
using BehaviourAI;

public class SetBossShadow : IAction
{
    [SerializeField] GameObject _setPrefab;
    [SerializeField] int _count;

    [SerializeField] float _range;

    public GameObject Target { get; set; }

    GameObject _player = null;

    public void SetUp()
    {
        if (_player == null) _player = GameObject.FindWithTag("Player");
    }

    public bool Execute()
    {
        for (int i = 0; i < _count; i++)
        {
            float x = Random.Range(-1, 1);
            float y = Random.Range(-1, 1);

            Vector3 setVec = new Vector3(x, 0, y) * _range;

            GameObject obj = Object.Instantiate(_setPrefab);
            UIManager.CallBack(UIType.Game, 3, new object[] { 4 });
            obj.transform.position = _player.transform.position + setVec;
        }

        return true;
    }
}
