
using UnityEngine;

using DR.BuildingSystem.Features.Runtime.Buildings.Manager;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Part.Conditions
{
    [ExecuteInEditMode]
    public class BuildingCollisionSurface : MonoBehaviour
    {
        #region Fields

        [SerializeField] string m_SurfaceIdentifier;
        public string SurfaceIdentifier { get { return m_SurfaceIdentifier; } set { m_SurfaceIdentifier = value; } }

        string m_DefaultName;

        #endregion

        #region Unity Methods

        void OnEnable()
        {
            string uniqueName = name.Replace("(Clone)", "");

            m_DefaultName = uniqueName;

            m_SurfaceIdentifier = uniqueName.Replace("(Clone)", "");

            if (BuildingManager.Instance != null)
            {
                if (!BuildingManager.Instance.AllSurfaces.Contains(m_SurfaceIdentifier))
                {
                    BuildingManager.Instance.AllSurfaces.Add(m_SurfaceIdentifier);
                }
            }
        }

        void OnValidate()
        {
            if (m_DefaultName != m_SurfaceIdentifier)
            {
                if (BuildingManager.Instance != null)
                {
                    BuildingManager.Instance.AllSurfaces.Remove(m_DefaultName);

                    if (!BuildingManager.Instance.AllSurfaces.Contains(m_SurfaceIdentifier))
                    {
                        m_DefaultName = m_SurfaceIdentifier;
                        BuildingManager.Instance.AllSurfaces.Add(m_SurfaceIdentifier);
                    }
                }
            }
        }

        void OnDisable()
        {
            if (BuildingManager.Instance != null)
            {
                BuildingManager.Instance.AllSurfaces.Remove(m_SurfaceIdentifier);
            }
        }

        #endregion
    }
}