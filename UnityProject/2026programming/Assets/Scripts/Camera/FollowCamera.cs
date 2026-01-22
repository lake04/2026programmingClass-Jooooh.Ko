using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Start()
    {
        target = GameManager.Instance.player.transform;
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y,-10);
    }
}
