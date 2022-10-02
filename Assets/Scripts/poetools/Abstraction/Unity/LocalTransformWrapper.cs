using UnityEngine;

namespace poetools.Abstraction.Unity
{
    public class LocalTransformWrapper : TransformComponent
    {
        public override Vector3 Position
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public override Quaternion Rotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
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
            get => transform.localScale;
            set => transform.localScale = value;
        }
    }
}