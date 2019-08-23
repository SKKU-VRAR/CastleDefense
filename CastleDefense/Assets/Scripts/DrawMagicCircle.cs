using UnityEngine;
using System.Collections;
namespace Valve.VR.InteractionSystem
{
    public class DrawMagicCircle : MonoBehaviour
    {
        const int MAX_POSITIONS = 1000;
        const float paper_distance = 1.0f;

        TrailRenderer trail = null;
        bool wasTouchpadPressed;
        bool wasDrawing;

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
            trail.enabled = true;
            if (!wasDrawing)
            {
                // Todo: 마법진을 그릴 평면을 세팅한 후, 위치를 고정한다.
                Camera cam = GetComponent<Camera>();
                Vector3 forward = cam.transform.forward;
                forward.y = 0.0f; forward.Normalize();
                forward.Scale(new Vector3(paper_distance, paper_distance, paper_distance));
                Vector3 paperpos = forward + cam.transform.position;

                wasDrawing = true;
            }
        }

        private void EndDrawing(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Vector3[] positions = new Vector3[MAX_POSITIONS];
            trail.GetPositions(positions);
            trail.enabled = false;
            trail.Clear();

            // Todo: positions 배열 가지고 평면에 그린다
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