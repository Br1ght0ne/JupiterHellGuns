using System;

namespace XRL.World.Parts
{
    [Serializable]
    public class JHG_EMPOnHit : IActivePart
    {
        public string Damage;
        public string Duration;

        public JHG_EMPOnHit()
        {
            base.Name = "JHG_EMPOnHit";
            base.WorksOnEquipper = true;
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "WeaponMissileWeaponHit");
            base.Register(Object);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "WeaponMissileWeaponHit")
            {
                GameObject attacker = E.GetGameObjectParameter("Attacker");
                if (IsObjectActivePartSubject(attacker))
                {
                    GameObject defender = E.GetGameObjectParameter("Defender");
                    if (defender.HasPart("Metal"))
                    {
                        // TODO: Discharge electric damage maybe?
                        defender.ForceApplyEffect(new Effects.ElectromagneticPulsed(Rules.Stat.Roll(Duration)));
                    }
                }
            }
            return base.FireEvent(E);
        }
    }
}

