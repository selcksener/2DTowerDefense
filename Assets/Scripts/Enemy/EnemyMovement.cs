using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private List<GameObject> _enemyWaypointPath = new List<GameObject>();// Düþmanýn gideceði yol bilgisi // Enemy waypoint info

    [SerializeField] private GameObject _nextWaypoint;// bir sonraki gideceði konum
    [SerializeField] private int _nextWaypointIndex = 0;// Gideceði konum indexi
    [SerializeField] private bool _isWalking = false;// Yürüyüp yürümediði bilgisi

    [SerializeField] private float _speed = 5f;// Hýz
    [SerializeField] private float _distance = 0.1f;//Bir sonraki konumla arasýndaki uzaklýk kontrolü

    public bool IsWalking { get { return _isWalking; } set { _isWalking = value; } }

    /// <summary>
    /// Düþman aktif olduktan sonra bir sonraki gideceði konum bilgisi güncellenip yürümesi saðlanýyor
    /// </summary>
    public void StartWalking()
    {
        _enemyWaypointPath = GameManager.Instance.levelManager.EnemyWaypointInfo;
        _nextWaypointIndex = 0;
        _nextWaypoint = _enemyWaypointPath[_nextWaypointIndex];
        transform.position = _nextWaypoint.transform.position;
        _isWalking = true;
        
    }
    /// <summary>
    /// Düþmanýn bir sonraki gideceði konumu ve yürümesini güncelliyor
    /// </summary>
    private void Update()
    {
        if (_isWalking == false) return;

        transform.position = Vector2.MoveTowards(transform.position, _nextWaypoint.transform.position, _speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, _nextWaypoint.transform.position) < _distance)
        {
            _nextWaypointIndex++;
            _nextWaypoint = _enemyWaypointPath[(_nextWaypointIndex) % _enemyWaypointPath.Count];
        }
    }
}
