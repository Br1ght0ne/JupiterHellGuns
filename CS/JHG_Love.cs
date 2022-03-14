using System;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Skill;

namespace XRL.World.Parts
{
    [Serializable]
    public class JHG_Love : IPart
    {
        public string Amount;

        public JHG_Love()
        {
            this.Name = "JHG_Love";
        }

        public override void Register(GameObject obj)
        {
            obj.RegisterPartEvent(this, "WeaponMissileWeaponHit");
            base.Register(obj);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "WeaponMissileWeaponHit")
            {
                GameObject attacker = E.GetGameObjectParameter("Attacker");
                GameObject defender = E.GetGameObjectParameter("Defender");
                if (defender.GetIntProperty("Inorganic") == 0 && defender.HasStat("Hitpoints"))
                {
                    attacker.Heal(Amount.RollCached(), Message: true, FloatText: true);
                }
            }
            return base.FireEvent(E);
        }
    }
}

