
using UnityEngine;

using System.Collections.Generic;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Manager
{
    public class BuildingType : ScriptableObject
    {
        #region Fields

        public static BuildingType Instance
        {
            get
            {
                return Resources.Load<BuildingType>("Building Types");
            }
        }

        [SerializeField] List<string> m_BuildingTypes = new List<string>();
        public List<string> BuildingTypes { get { return m_BuildingTypes; } set { m_BuildingTypes = value; } }

        #endregion
    }
}