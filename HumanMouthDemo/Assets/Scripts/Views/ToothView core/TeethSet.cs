using System.Collections.Generic;
using ATG.Activation;
using UnityEngine;

namespace ATG.Views
{
    public sealed class TeethSet : ActivateObject
    {
        private readonly IReadOnlyDictionary<int, ToothView> _teethes;

        private readonly HashSet<ToothView> _choosedTeeth;

        public ToothView LastSelectedTooth { get; private set; }

        public int ChoosedCount => _choosedTeeth.Count;

        public TeethSet(IReadOnlyDictionary<int, ToothView> teethes)
        {
            _teethes = teethes;
            _choosedTeeth = new HashSet<ToothView>();

            LastSelectedTooth = null;
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            foreach (var teeth in _teethes.Values)
            {
                teeth.SetActive(isActive);
            }
        }
        public void Reset()
        {
            _choosedTeeth.Clear();
            LastSelectedTooth = null;
        }

        public void ClearSelectedTooth() => LastSelectedTooth = null;

        public void ChoosedAdd(ToothView i) => _choosedTeeth.Add(i);
        public bool ChoosedContains(ToothView i) => _choosedTeeth.Contains(i);
        public void ChoosedRemove(ToothView i) => _choosedTeeth.Remove(i);
        public void ChoosedClear() => _choosedTeeth.Clear();

        public IEnumerable<ToothView> GetChoosedEnumerable() => _choosedTeeth;
        public IEnumerable<ToothView> GetEnumerable() => _teethes.Values;

        public void SelectTooth(GameObject hittedObject)
        {
            if (hittedObject == null || _teethes.ContainsKey(hittedObject.GetHashCode()) == false)
            {
                LastSelectedTooth?.Unselect();
                LastSelectedTooth = null;

                return;
            }

            var newSelected = _teethes[hittedObject.GetHashCode()];

            if (ReferenceEquals(newSelected, LastSelectedTooth)) return;

            LastSelectedTooth?.Unselect();
            LastSelectedTooth = newSelected;

            LastSelectedTooth.Select();
        }

    }
}