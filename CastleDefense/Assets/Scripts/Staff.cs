//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        public GameObject linePrefab;

        private GameObject magicCircle;
        private GameObject point;

        private Transform prevPoint;

        private bool isPointTrigger = false;

        private Coroutine runeRotate;
        private Color color = Color.white;
        private List<int> points;

        private Color colorUtil = new Color(0.5f, 1, 0.5f);
        private Color colorAir = new Color(0.5f, 0.75f, 1);
        private Color colorFire = new Color(1, 0.25f, 0);
        private Color colorEarth = new Color(0.75f, 0.5f, 0);
        private Color colorWater = new Color(0f, 0.5f, 1);


        //-------------------------------------------------
        void Awake()
        {
            interactable = this.GetComponent<Interactable>();
            magicEffect.SetActive(false);
            points = new List<int>();
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
                    runeRotate = StartCoroutine(RotateRunes(magicCircle.transform.GetChild(7)));

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
                                if (prevPoint != null)
                                {
                                    if (prevPoint.position != hit.transform.position)
                                    {
                                        points.Add(int.Parse(hit.transform.gameObject.name.Substring(7)));
                                        if (points.Count == 1)
                                        {
                                            switch (points[0])
                                            {
                                                case 1:
                                                    color = colorUtil;
                                                    break;
                                                case 2:
                                                    color = colorAir;
                                                    break;
                                                case 3:
                                                    color = colorFire;
                                                    break;
                                                case 4:
                                                    color = colorEarth;
                                                    break;
                                                case 5:
                                                    color = colorWater;
                                                    break;
                                            }
                                            foreach (SpriteRenderer sr in magicCircle.GetComponentsInChildren<SpriteRenderer>())
                                            {
                                                sr.color = color;
                                            }
                                        }
                                        GameObject newline = Instantiate(linePrefab);
                                        newline.transform.parent = magicCircle.transform.GetChild(8);
                                        newline.transform.localPosition = (prevPoint.localPosition + hit.transform.localPosition) / 2;
                                        Debug.Log((prevPoint.localPosition + hit.transform.localPosition) / 2);

                                        Vector3 diff = prevPoint.transform.localPosition - hit.transform.localPosition;
                                        newline.transform.localScale = Vector3.right * diff.magnitude * 0.2f + Vector3.up * 0.015f;
                                        newline.transform.localRotation = Quaternion.FromToRotation(Vector3.right, diff);

                                        newline.GetComponent<SpriteRenderer>().color = color;
                                    }
                                }

                                prevPoint = hit.transform;
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
                    if (runeRotate != null) StopCoroutine(runeRotate);
                    Destroy(magicCircle.gameObject);
                    Destroy(point.gameObject);
                    points = new List<int>();
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

        IEnumerator RotateRunes(Transform t)
        {
            Vector3 rot = Vector3.zero;
            while (true)
            {
                yield return null;
                rot.z += Time.deltaTime * 30;
                t.transform.localRotation = Quaternion.Euler(rot);
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
