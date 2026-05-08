using System.Collections;
using System.Threading;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class MagicPointsBehaviour : MonoBehaviour
    {
        public float currentMagicPoints;

        [SerializeField] private float _maxMagicPoints = 10f;
        [SerializeField] private float _regenerationTime = 50f;

        private bool _regenerate = false;
        private Coroutine _coroutine = null;
        private float _timer = 0f;

        private void Awake()
        {
            //_currentMagicPoints = _maxMagicPoints;
            currentMagicPoints = 0;
        }

        public bool HasMagicPoints(float magicPointsCost)
        {
            return currentMagicPoints >= magicPointsCost;
        }

        public void ConsumeMagicPoints(float magicPointsCost)
        {
            currentMagicPoints -= magicPointsCost;
            _timer = Time.time;
        }

        public void RegenerateMagicPoints()
        {
            if (Time.time >= _timer && currentMagicPoints < _maxMagicPoints)
            {
                _regenerate = true;

                _coroutine ??= StartCoroutine(MagicPointsRegeneration());

                _timer = Time.time + _regenerationTime;
            }
        }

        private void CancelRegeneration()
        {
            _regenerate = false;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
        private IEnumerator MagicPointsRegeneration()
        {
            while (_regenerate)
            {
                currentMagicPoints += (_maxMagicPoints / _regenerationTime) * Time.deltaTime;

                if (currentMagicPoints >= _maxMagicPoints)
                {
                    currentMagicPoints = _maxMagicPoints;
                    CancelRegeneration();
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}