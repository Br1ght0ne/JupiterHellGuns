using System;

namespace XRL.World.Parts
{
    [Serializable]
    public class JHG_ConfuseOnHit : IActivePart
    {
        public int Strength = 25;
        public string Duration = "3-5";
        public int Level = 5;

        public JHG_ConfuseOnHit()
        {
            base.Name = "JHG_ConfuseOnHit";
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
                    if (defender != null && !defender.MakeSave("Willpower", Strength, Vs: "Confusion"))
                    {
                        defender.ApplyEffect(new XRL.World.Effects.Confused(Duration.RollCached(), Level, Level + 2));
                    }
                }
            }
            return base.FireEvent(E);
        }
    }
}

