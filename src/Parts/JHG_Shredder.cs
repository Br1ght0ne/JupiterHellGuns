using System;

namespace XRL.World.Parts
{
    [Serializable]
    public class JHG_Shredder : IActivePart
    {
        public string Damage = "1d2+1";

        public JHG_Shredder()
        {
            base.Name = "JHG_Shredder";
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
                    if (defender.HasEffect("Bleeding"))
                    {
                        E.SetParameter("Critical", 1);
                    }
                    GameObject.validate(ref attacker);
                    defender.ApplyEffect(new XRL.World.Effects.Bleeding(Damage, Owner: attacker), attacker);
                }
            }
            return base.FireEvent(E);
        }
    }
}

