using UnityEngine;

namespace poetools.Abstraction
{
    public interface IPositionComponent
    {
        Vector3 Position { get; set; }
    }

    public interface IScaleComponent
    {
        Vector3 Scale { get; set; }
    }

    public interface IRotationComponent
    {
        Vector3 Forward { get; set; }
        Vector3 Right { get; set; }
        Quaternion Rotation { get; set; }
    }

    public abstract class TransformComponent : MonoBehaviour,
        IPositionComponent, IRotationComponent, IScaleComponent
    {
        public abstract Vector3 Position { get; set; }
        public abstract Vector3 Forward { get; set; }
        public abstract Vector3 Right { get; set; }
        public abstract Quaternion Rotation { get; set; }
        public abstract Vector3 Scale { get; set; }
    }
}