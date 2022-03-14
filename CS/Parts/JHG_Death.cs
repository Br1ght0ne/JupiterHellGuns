using System;

namespace XRL.World.Parts
{
    [Serializable]
    public class JHG_Death : IActivePart
    {
        public JHG_Death()
        {
            base.Name = "JHG_Death";
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
                    if (defender.GetIntProperty("Inorganic") == 0 && defender.HasStat("Hitpoints"))
                    {
                        defender.ApplyEffect(new Effects.JHG_Death(Owner: attacker), attacker);
                    }
                }
            }
            return base.FireEvent(E);
        }
    }
}

