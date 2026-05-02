using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Units
{
    /// <summary>
    /// Shared visual wiring for any unit's 3D representation.
    /// Lives on the root of a unit prefab. Combat-specific components go on CombatUnitView.
    /// </summary>
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private Transform _visualsRoot;
        [SerializeField] private Transform _center;

        public Transform VisualsRoot => _visualsRoot;
        public Transform Center => _center;

        private HashSet<GameObject> _renderers;
        public HashSet<GameObject> Renderers => _renderers;

        protected virtual void Awake()
        {
            RefreshRendererCache();
        }

        public void RefreshRendererCache()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>();
            _renderers = new HashSet<GameObject>(rendererComponents.Length);
            for (int i = 0; i < rendererComponents.Length; i++)
                _renderers.Add(rendererComponents[i].gameObject);
        }

        public void SetVisible(bool visible)
        {
            if (_renderers == null) return;
            foreach (var rendererGO in _renderers)
            {
                if (rendererGO != null)
                {
                    Renderer renderer = rendererGO.GetComponent<Renderer>();
                    if (renderer != null)
                        renderer.enabled = visible;
                }
            }
        }
    }
}
