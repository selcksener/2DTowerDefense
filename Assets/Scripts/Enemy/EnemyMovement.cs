using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private List<GameObject> _enemyWaypointPath = new List<GameObject>();// D��man�n gidece�i yol bilgisi // Enemy waypoint info

    [SerializeField] private GameObject _nextWaypoint;// bir sonraki gidece�i konum
    [SerializeField] private int _nextWaypointIndex = 0;// Gidece�i konum indexi
    [SerializeField] private bool _isWalking = false;// Y�r�y�p y�r�medi�i bilgisi

    [SerializeField] private float _speed = 5f;// H�z
    [SerializeField] private float _distance = 0.1f;//Bir sonraki konumla aras�ndaki uzakl�k kontrol�

    public bool IsWalking { get { return _isWalking; } set { _isWalking = value; } }

    /// <summary>
    /// D��man aktif olduktan sonra bir sonraki gidece�i konum bilgisi g�ncellenip y�r�mesi sa�lan�yor
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
    /// D��man�n bir sonraki gidece�i konumu ve y�r�mesini g�ncelliyor
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
