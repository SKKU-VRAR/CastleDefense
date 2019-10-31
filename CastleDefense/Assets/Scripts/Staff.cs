//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class Staff : MonoBehaviour
    {
        private float attachTime;

        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
        //private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags;

        private Interactable interactable;

        // 마법구 이펙트 파티클
        public GameObject magicEffect;
        public GameObject magicCirclePrefab;
        public GameObject pointPrefab;
        public LineRenderer line;

        private GameObject magicCircle;
        private GameObject point;
        
        private bool isPointTrigger = false;
        

        //-------------------------------------------------
        void Awake()
        {
            interactable = this.GetComponent<Interactable>();
            magicEffect.SetActive(false);
            line.positionCount = 0;
        }
        //-------------------------------------------------
        // Called when a Hand starts hovering over this object
        //-------------------------------------------------
        private void OnHandHoverBegin(Hand hand)
        {
        }


        //-------------------------------------------------
        // Called when a Hand stops hovering over this object
        //-------------------------------------------------
        private void OnHandHoverEnd(Hand hand)
        {
        }


        //-------------------------------------------------
        // Called every Update() while a Hand is hovering over this object
        //-------------------------------------------------
        private void HandHoverUpdate(Hand hand)
        {
            //Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);
            //Debug.DrawLine(transform.position, transform.position + transform.up, Color.green);
            //Debug.DrawLine(transform.position, transform.position + transform.right, Color.red);
            GrabTypes startingGrabType = hand.GetGrabStarting();
            GrabTypes endingGrabType = hand.GetGrabEnding();
            bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (interactable.attachedToHand == null)
            {
                if (startingGrabType != GrabTypes.None)
                {
                    hand.HoverLock(interactable);
                    hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
                }
            }
            else
            {
                if (startingGrabType == GrabTypes.Pinch)
                {
                    magicCircle = Instantiate(magicCirclePrefab, transform.position + transform.up * 3, Quaternion.identity);
                    magicCircle.transform.LookAt(transform.position);

                    point = Instantiate(pointPrefab, magicCircle.transform.position, Quaternion.identity);
                }
                if (hand.IsGrabbingWithType(GrabTypes.Pinch))
                {
                    // 마법 발동? 그리기
                    // holy 12365416851651561651456561561shit
                    magicEffect.SetActive(true);
                    
                    if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, Mathf.Infinity))
                    {
                        Debug.Log(hit.transform.name);
                        if (hit.transform.CompareTag("paint") || hit.transform.CompareTag("paintTrigger"))
                        {
                            point.transform.position = hit.point;
                            if (hit.transform.CompareTag("paintTrigger") && !isPointTrigger)
                            {
                                isPointTrigger = true;
                                hand.hapticAction.Execute(0, 0.1f, 1, 30, hand.handType);
                                line.positionCount++;
                                line.SetPosition(line.positionCount - 1, hit.transform.position + hit.transform.forward * 0.1f);
                            }
                            else if (hit.transform.CompareTag("paint") && isPointTrigger)
                            {
                                isPointTrigger = false;
                            }
                        }
                    }

                }
                if (endingGrabType == GrabTypes.Pinch)
                {
                    magicEffect.SetActive(false);
                    Destroy(magicCircle.gameObject);
                    Destroy(point.gameObject);
                    line.positionCount = 0;
                }
                if (startingGrabType == GrabTypes.Grip)
                {
                    // 내려놓기
                    // 굳이 없어도 될듯?
                    hand.DetachObject(gameObject);
                    hand.HoverUnlock(interactable);
                }
            }
        }


        //-------------------------------------------------
        // Called when this GameObject becomes attached to the hand
        //-------------------------------------------------
        private void OnAttachedToHand(Hand hand)
        {
        }



        //-------------------------------------------------
        // Called when this GameObject is detached from the hand
        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
        }


        //-------------------------------------------------
        // Called every Update() while this GameObject is attached to the hand
        //-------------------------------------------------
        private void HandAttachedUpdate(Hand hand)
        {
        }

        private bool lastHovering = false;
        private void Update()
        {
            if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
            {
                lastHovering = interactable.isHovering;
            }
        }


        //-------------------------------------------------
        // Called when this attached GameObject becomes the primary attached object
        //-------------------------------------------------
        private void OnHandFocusAcquired(Hand hand)
        {
        }


        //-------------------------------------------------
        // Called when another attached GameObject becomes the primary attached object
        //-------------------------------------------------
        private void OnHandFocusLost(Hand hand)
        {
        }
    }
}
