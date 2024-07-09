using System;
using System.Collections.Generic;
using UnityEngine;
using UnityTransform = UnityEngine.Transform;

namespace ATG.Raycasting
{
    public class RaycastService: IRaycastService
    {
        private readonly RaycastData _config;
        private readonly UnityTransform _source;

        private readonly RaycastHit[] _hitInfo;

        public event Action<GameObject> OnRaycastHit;

        private readonly int _layerMask;

        public RaycastService(UnityTransform source, RaycastData config)
        {
            _source = source;
            _config = config;

            _layerMask = config.Layer;

            _hitInfo = new RaycastHit[1];
        }

        public GameObject[] Raycast()
        {
            int hitsCount = Physics.RaycastNonAlloc(_source.position, _source.forward,_hitInfo,_config.RaycastDistance);

            if(hitsCount == 0) return null;

            GameObject[] res = new GameObject[hitsCount];

            for(int i = 0; i < hitsCount; i++)
            {
                res[i] = _hitInfo[i].transform.gameObject;
            }

            return res;
        }
    }
}