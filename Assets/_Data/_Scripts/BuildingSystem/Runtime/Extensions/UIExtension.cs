﻿
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace DR.BuildingSystem.Features.Runtime.Extensions
{
    public static class UIExtension
    {
        public static bool IsPointerOverUIElement()
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

#if EBS_INPUT_SYSTEM_SUPPORT
            eventDataCurrentPosition.position = new Vector2(UnityEngine.InputSystem.Mouse.current.position.x.ReadValue(), 
                UnityEngine.InputSystem.Mouse.current.position.y.ReadValue());
#else
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
