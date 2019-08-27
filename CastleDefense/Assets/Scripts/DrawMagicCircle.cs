using UnityEngine;
using System.Collections;
namespace Valve.VR.InteractionSystem
{
    public class DrawMagicCircle : MonoBehaviour
    {
        const int MAX_POSITIONS = 1000;
        const float paper_distance = 20.0f;

        TrailRenderer trail = null;
        bool wasTouchpadPressed;
        bool wasDrawing;
        Plane plane;

        public TrailRenderer casted_trail = null;

        public Camera cam;
        Vector3 cam_pos;

        // Use this for initialization
        void Start()
        {
            trail = this.GetComponentInChildren<TrailRenderer>();
            trail.enabled = false;
            wasTouchpadPressed = false;
            wasDrawing = false;
            SteamVR_Actions._default.GrabPinch[SteamVR_Input_Sources.RightHand].onStateDown += FinishMagicCircle;
            SteamVR_Actions._default.TouchpadPress[SteamVR_Input_Sources.RightHand].onStateDown += StartDrawing;
            SteamVR_Actions._default.TouchpadPress[SteamVR_Input_Sources.RightHand].onStateUp += EndDrawing;
        }

        private void StartDrawing(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            cam_pos = cam.transform.position;
            casted_trail.enabled = true;
            //casted_trail.emitting = false;
            trail.enabled = true;
            if (!wasDrawing)
            {
                // Todo: 마법진을 그릴 평면을 만든다.
                Vector3 forward = cam.transform.forward;
                forward.y = 0.0f; forward.Normalize();
                forward.Scale(new Vector3(paper_distance, paper_distance, paper_distance));
                Vector3 paperpos = forward + cam.transform.position;

                plane = new Plane(forward.normalized, paperpos);

                wasDrawing = true;
            }
        }

        private void EndDrawing(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {

            Vector3[] positions = new Vector3[MAX_POSITIONS];
            Vector3[] castedpos = new Vector3[MAX_POSITIONS];
            int num_points = trail.GetPositions(positions);
            trail.enabled = false;
            trail.Clear();

            // Todo: positions 배열 가지고 평면에 그린다
            for(int i=0; i<num_points; ++i)
            {
                Vector3 p = positions[i];
                Ray ray = new Ray(cam_pos, (p - cam_pos).normalized);
                float distance = 0.0f;
                if(plane.Raycast(ray, out distance))
                {
                    castedpos[i] = ray.GetPoint(distance);
                    Debug.Log(castedpos[i]);
                    if(i>0) Debug.DrawLine(castedpos[i-1], castedpos[i], Color.green, 10);
                    //casted_trail.AddPosition(castedpos[i]);
                }
            }

            //casted_trail.enabled = false;
        }

        // Fire Magic Circle
        void FinishMagicCircle(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (!wasDrawing) return;
            wasDrawing = false;
            // Todo: 평면에 그려진 trail의 결과를 보고 마법진을 인식한다.
        }

        private void FixedUpdate()
        {
            bool tp = SteamVR_Actions._default.TouchpadPress[SteamVR_Input_Sources.RightHand].state;
            if (tp)
            {
                Vector3 pos = trail.GetPosition(trail.positionCount - 1);
                float dist;
                Ray ray = new Ray(cam_pos, (pos - cam_pos).normalized);
                if (plane.Raycast(ray, out dist))
                {
                    Debug.Log("raycast success");
                    casted_trail.AddPosition(ray.GetPoint(dist));
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            SteamVR_Actions._default.GrabPinch[SteamVR_Input_Sources.RightHand].onStateDown -= FinishMagicCircle;
            SteamVR_Actions._default.TouchpadPress[SteamVR_Input_Sources.RightHand].onStateDown -= StartDrawing;
            SteamVR_Actions._default.TouchpadPress[SteamVR_Input_Sources.RightHand].onStateUp -= EndDrawing;
        }
    }
}