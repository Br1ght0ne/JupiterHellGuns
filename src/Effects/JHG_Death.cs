using System;

namespace XRL.World.Effects
{
    [Serializable]
    public class JHG_Death : Effect
    {
        public int StartDamage = 2;
        public int SaveTarget = 20;
        public string SaveStat = "Toughness";
        public GameObject Owner;

        public JHG_Death()
        {
            base.DisplayName = "{{G|death}}";
            base.Duration = StartDamage;
        }

        public JHG_Death(int StartDamage = 2, int SaveTarget = 20, string SaveStat = "Toughness", GameObject Owner = null) : this()
        {
            this.StartDamage = StartDamage;
            this.SaveTarget = SaveTarget;
            this.SaveStat = SaveStat;
            this.Owner = Owner;
        }

        public override string GetDetails()
        {
            return $"Takes {Duration} damage each turn, double that next turn.";
        }

        public override int GetEffectType()
        {
            return 1;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) ||
                ID == EndTurnEvent.ID ||
                ID == GeneralAmnestyEvent.ID;
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            GameObject.validate(ref Owner);
            if (base.Duration > 0 && !RecoveryChance())
            {
                int damage = base.Duration;
                base.Object.TakeDamage(ref damage, Attacker: Owner, Message: DamageMessage(), Attributes: "Unavoidable");
                base.Duration *= 2;
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GeneralAmnestyEvent E)
        {
            Owner = null;
            return base.HandleEvent(E);
        }

        public bool RecoveryChance()
        {
            if (base.Object.MakeSave(SaveStat, SaveTarget, null, null, base.DisplayNameStripped))
            {
                base.Duration = 0;
                return true;
            }
            SaveTarget--;
            return false;
        }

        public string DamageMessage()
        {
            return $"from {base.DisplayName}.";
        }
    }
}
