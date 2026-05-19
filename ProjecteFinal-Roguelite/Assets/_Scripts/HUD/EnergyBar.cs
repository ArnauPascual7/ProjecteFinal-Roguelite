using UnityEngine;
using UnityEngine.UI;

namespace Roguelite
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private Slider sliderEnergy;

        public void StartEnergyBar(float energyMax, float actualEnergy)
        {
            sliderEnergy.maxValue = energyMax;
            sliderEnergy.value = actualEnergy;
        }

        public void UpdateEnergyBar(float actualEnergy)
        {
            sliderEnergy.value = actualEnergy;
        }
    }
}
