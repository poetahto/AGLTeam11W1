using UnityEngine;

namespace poetools.Abstraction.Unity
{
    public class AbsoluteTransformWrapper : TransformComponent
    {
        public override Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public override Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public override Vector3 Forward
        {
            get => transform.forward;
            set => transform.forward = value;
        }

        public override Vector3 Right
        {
            get => transform.right;
            set => transform.right = value;
        }

        public override Vector3 Scale
        {
            get => transform.lossyScale;
            set => Debug.LogError("Cannot set absolute scale.");
        }
    }
}