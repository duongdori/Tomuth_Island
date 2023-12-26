
using DR.BuildingSystem.Features.Runtime.Buildings.Part;
using UnityEngine;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Part.Conditions
{
    public class BuildingCondition : MonoBehaviour
    {
        #region Fields

        BuildingPart m_BuildingPart;
        public BuildingPart GetBuildingPart
        {
            get
            {
                if (m_BuildingPart == null)
                {
                    m_BuildingPart = GetComponent<BuildingPart>();
                }

                return m_BuildingPart;
            }
        }

        #endregion

        #region Methods

        public virtual void EnableCondition() { }

        public virtual void DisableCondition() { }

        public virtual bool CheckPlacingCondition() { return true; }

        public virtual bool CheckDestroyCondition() { return true; }

        public virtual bool CheckEditingCondition() { return true; }

        #endregion
    }
}