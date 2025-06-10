using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>();

    public void AddAbility(Ability ability)
    {
        abilities.Add(ability);
        // Lógica para activar la habilidad
    }
}
