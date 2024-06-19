using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraGizmos : MonoBehaviour
    {

        [SerializeField, HideInInspector]
        private new Camera camera;

        [SerializeField]
        private float cubePosition = 5.0f;

        [SerializeField]
        private Color frustumColor = Color.red;

        [SerializeField]
        private Color viewportColor = new Color(200, 0, 0, 80);

        [SerializeField]
        private bool alwaysDrawGizmos = true;

        [SerializeField]
        private bool alwaysDrawFrustum = false;

        [SerializeField]
        private bool alwaysDrawCube = true;

        private Vector3[] frustumCorners = new Vector3[4];
        private Rect viewport = new Rect(0, 0, 1, 1);

        public Camera Camera
        {
            get
            {
                if (camera == null)
                    camera = GetComponent<Camera>();
                return camera;
            }
        }

        void OnDrawGizmos()
        {
            if (alwaysDrawGizmos)
                DrawCameraGizmos(Camera);
        }

        void OnDrawGizmosSelected()
        {
            if (!alwaysDrawGizmos)
                DrawCameraGizmos(Camera);
        }

        public void DrawCameraGizmos(Camera camera)
        {
            if (alwaysDrawGizmos)
            {
                // Save gizmos settings
                Color color = Gizmos.color;
                Matrix4x4 matrix = Gizmos.matrix;

                // Set gizmos matrix
                Gizmos.matrix = Matrix4x4.TRS(camera.transform.position, camera.transform.rotation, Vector3.one);

                if (alwaysDrawCube)
                {
                    // Draw viewport
                    Gizmos.color = viewportColor;
                    camera.CalculateFrustumCorners(viewport, camera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

                    float width = Vector3.Distance(frustumCorners[2], frustumCorners[1]);
                    float height = Vector3.Distance(frustumCorners[1], frustumCorners[0]);
                    Gizmos.DrawCube(Vector3.forward * (camera.nearClipPlane + cubePosition), new Vector3(width, height, 0.01f));
                }

                // Draw frustum
                if (alwaysDrawFrustum)
                {
                    Gizmos.color = frustumColor;
                    Gizmos.DrawFrustum(Vector3.forward * camera.nearClipPlane, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
                }

                // Restore gizmos settings
                Gizmos.color = color;
                Gizmos.matrix = matrix;
            }
        }
    }
}