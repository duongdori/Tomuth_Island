
using UnityEngine;

using DR.BuildingSystem.Features.Runtime.Buildings.Part;

using DR.BuildingSystem.Features.Runtime.Extensions;

namespace DR.BuildingSystem.Packages.Addons.SurvivalBuildings
{
    public class InstantiateFracture : MonoBehaviour
    {
        [SerializeField] GameObject m_BuildingDebris;
        [SerializeField] float m_BuildingDebrisLifetime = 15f;

        BuildingPart m_BuildingPart;
        public BuildingPart GetBuildingPart
        {
            get
            {
                if (m_BuildingPart == null)
                {
                    m_BuildingPart = GetComponentInParent<BuildingPart>();
                }

                return m_BuildingPart;
            }
        }

        void Start()
        {
            if (GetBuildingPart != null)
            {
                if (GetBuildingPart.TryGetPhysicsCondition != null)
                {
                    GetBuildingPart.TryGetPhysicsCondition.FallingTime = 0f;

                    GetBuildingPart.TryGetPhysicsCondition.OnFallingBuildingPartEvent.AddListener(() =>
                    {
                        Destroy(Instantiate(m_BuildingDebris,
                            transform.position, transform.rotation), m_BuildingDebrisLifetime);
                    });
                }
            }
        }

        void OnDestroy()
        {
            if (!this.gameObject.scene.isLoaded) return;

            if (!gameObject.activeSelf) return;

            if (GetBuildingPart == null)
            {
                return;
            }

            if (GetBuildingPart.State == BuildingPart.StateType.PREVIEW)
            {
                return;
            }

            GameObject instancedFracture = Instantiate(m_BuildingDebris,
                        transform.position, transform.rotation);

            instancedFracture.SetLayerRecursively(LayerMask.NameToLayer("Ignore Raycast"));

            Destroy(instancedFracture, m_BuildingDebrisLifetime);
        }
    }
}