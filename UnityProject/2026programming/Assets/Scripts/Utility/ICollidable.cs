using UnityEngine;

public interface ICollidable 
{
    Vector2 Position { get; }
    float Radius { get; }
    void  OnCollide(ICollidable other);
}
