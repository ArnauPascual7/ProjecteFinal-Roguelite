using UnityEngine;
using Roguelite.Systems;
using Roguelite.Behaviours;

namespace Roguelite.Player
{
    public class PlayerStatsManager : MonoBehaviour
    {
        [SerializeField] private UpgradeData[] _allUpgrades; // ScriptableObjects

        private void Start()
        {
            ApplyUpgrades();
            //
            PrintStatsSummary();
            //
        }

        public void ApplyUpgrades()
        {
            // Carregar dades del json
            SaveSystem.SaveData saveData = SaveSystem.LoadRaw();

            if (saveData.ObjectData.upgrades == null)
            {
                return;
            }

            // Per cada millora guardada, buscar el seu valor i aplicar-lo
            foreach (var savedUpgrade in saveData.ObjectData.upgrades)
            {
                // Buscar el SO que coincideix amb la ID guardada
                UpgradeData data = System.Array.Find(_allUpgrades, u => u.upgradeName == savedUpgrade.upgradeID);

                if (data != null)
                {
                    // Obtenim el valor segons el nivell comprat
                    // Si el nivell és 0 (base), potser el valor és el de la columna 1 de l'excel
                    float finalValue = data.values[Mathf.Clamp(savedUpgrade.level - 1, 0, data.values.Length - 1)];

                    InjectStat(data.statType, finalValue);
                }
            }
        }

        private void InjectStat(StatType type, float value)
        {
            WeaponController wc = GetComponent<WeaponController>();

            switch (type)
            {
                // Vida màxima
                case StatType.MaxHealth:
                    GetComponent<PlayerHealth>().SetMaxHealth(value);
                    break;

                // Velocitat Màxima
                case StatType.MoveSpeed:
                    // Suposem que el teu MoveBehaviour té una funció SetSpeed
                    if (TryGetComponent(out MoveBehaviour move)) move.UpdateBaseSpeed(value);
                    break;

                // Energia màxima
                case StatType.MaxEnergy:
                    if (TryGetComponent(out StaminaBehaviour stamina)) stamina.SetMaxStamina(value);
                    break;

                // Dany
                case StatType.AttackDamage:
                    if (wc != null) wc.SetDamageBonus(value);
                    break;

                // Velocitat de projectil
                case StatType.ProjectileSpeed:
                    if (wc != null) wc.SetProjectileSpeedBonus(value);
                    break;

                // Velocitat de recàrrega
                case StatType.ReloadSpeed:
                    if (wc != null) wc.SetReloadSpeedBonus(value);
                    break;

            }
            Debug.Log($"Millora aplicada: {type} amb valor {value}");
        }

        // Per aplicar millores:
        //  Seleccionar Player.
        //  Clicar 4 vegades a allUpgrades
        //  Arrossegar scripatble objects
        //  Revisar i assignar state types dels scritableObjects

        private void PrintStatsSummary()
        {
            Debug.Log("<color=green>=== RESUM DE MILLORES APLICADES ===</color>");
            if (TryGetComponent(out PlayerHealth ph)) Debug.Log($"Vida Màxima: {ph.Health}");
            if (TryGetComponent(out MoveBehaviour mb)) Debug.Log($"Velocitat actual: {mb.Speed}");
            if (TryGetComponent(out WeaponController wc)) Debug.Log($"Bonus Dany: x{wc.DamageMultiplier}");
            Debug.Log("<color=green>==================================</color>");
        }
    }
}
