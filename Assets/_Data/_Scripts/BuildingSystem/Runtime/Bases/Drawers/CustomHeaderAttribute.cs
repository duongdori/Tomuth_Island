
using System;

using UnityEngine;

namespace DR.BuildingSystem.Features.Runtime.Bases.Drawers
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CustomHeaderAttribute : PropertyAttribute
    {
        public string Text { get; private set; }

        public string Description { get; private set; }
    }
}