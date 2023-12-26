
using System.Collections.Generic;

using UnityEngine;

using DR.BuildingSystem.Features.Runtime.Buildings.Part;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Manager.Collection
{
    [HelpURL("https://polarinteractive.gitbook.io/easy-build-system/components/building-manager/building-collection")]
    public class BuildingCollection : ScriptableObject
    {
        #region Fields

        [SerializeField] List<BuildingPart> m_BuildingParts = new List<BuildingPart>();
        public List<BuildingPart> BuildingParts { get { return m_BuildingParts; } set { m_BuildingParts = value; } }

        #endregion
    }
}