using System.Collections.Generic;
using UnityEngine;

namespace ATG.Update
{
    public sealed class UpdateService : MonoBehaviour, IUpdateExecutor
    {
        public bool IsActive {get; private set;}

        private HashSet<IUpdateable> _updateable;

        private void Awake()
        {
            SetActive(false);
            _updateable = new HashSet<IUpdateable>();
        }

        private void Update()
        {
            if(IsActive == false) return;
            if(_updateable == null) return;

            foreach(var upd in _updateable)
            {
                upd.Update();
            }
        }

        public void Add(IUpdateable update)
        {
            _updateable.Add(update);
        }

        public void Remove(IUpdateable update)
        {
            if(_updateable.Contains(update))
            {
                _updateable.Remove(update);
            }
        }

        public void SetActive(bool isActive)
        {
            IsActive = enabled = isActive;
        }
    }
}