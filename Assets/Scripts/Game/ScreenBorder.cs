using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ScreenBorder : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out BallMover ball))
            if (ball.IsQueue != true)
                ball.ReturnBall();
    }
}
