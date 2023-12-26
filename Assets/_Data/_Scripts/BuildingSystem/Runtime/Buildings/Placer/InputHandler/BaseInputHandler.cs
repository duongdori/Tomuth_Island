
using DR.BuildingSystem.Features.Runtime.Buildings.Placer;
using UnityEngine;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Placer.InputHandler
{
    public class BaseInputHandler : MonoBehaviour
    {
        #region Fields

        BuildingPlacer m_Placer;
        public BuildingPlacer Placer
        {
            get
            {
                if (m_Placer == null)
                {
                    m_Placer = BuildingPlacer.Instance;
                }

                return m_Placer;
            }
        }

        #endregion
    }
}