using System;
using XRL.Rules;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts;
using XRL.World.Parts.Skill;

namespace XRL.World.Parts
{
    [Serializable]
    public class JHG_HealthAmmoLoader : LiquidAmmoLoader
    {
        public int MaxDamagePerBullet = 5;
        public bool ApplyBleeding = true;

        public JHG_HealthAmmoLoader()
        {
            this.Name = "JHG_HealthAmmoLoader";
            this.Liquid = "blood";
            this.ShowDamage = true;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == CommandReloadEvent.ID;
        }

        public override bool HandleEvent(CommandReloadEvent E)
        {
            if (E.Weapon == null || E.Weapon == ParentObject)
            {
                E.CheckedForReload.Add(this);
                if (!ParentObject.IsEquippedProperly())
                {
                    return true;
                }
                if (!E.Actor.HasHitpoints())
                {
                    return true;
                }
                LiquidVolume liquidVolume = ParentObject.LiquidVolume;
                if (liquidVolume.IsPureLiquid(Liquid) && liquidVolume.Volume >= liquidVolume.MaxVolume)
                {
                    if (E.Actor.IsPlayer())
                    {
                        IComponent<GameObject>.AddPlayerMessage(ParentObject.The + ParentObject.ShortDisplayName + ParentObject.Is + " already full of " + liquidVolume.GetLiquidName() + ".");
                    }
                    return true;
                }
                E.NeededReload.Add(this);
                int dramsToLoad = liquidVolume.MaxVolume - liquidVolume.Volume;
                string damageRoll = $"{dramsToLoad}d{MaxDamagePerBullet}";
                int maxDamage = damageRoll.RollMax();
                if (E.Actor.hitpoints - maxDamage <= 0)
                {
                    if (E.Actor.IsPlayer())
                    {
                        IComponent<GameObject>.AddPlayerMessage("Reloading " + ParentObject.the + ParentObject.ShortDisplayName + " might kill you.");
                    }
                    return true;
                }
                E.TriedToReload.Add(this);
                int totalDamage = damageRoll.Roll();
                E.Actor.TakeDamage(ref totalDamage, "Unavoidable", null, null, E.Actor, null, null, "from " + ParentObject.the + ParentObject.ShortDisplayName + ".");
                if (ApplyBleeding)
                {
                    for (int i = 0; i < dramsToLoad; i++)
                    {
                        E.Actor.ApplyEffect(new Bleeding("1d2"));
                    }
                }
                liquidVolume.MixWith(new LiquidVolume(Liquid, dramsToLoad));
                E.Reloaded.Add(this);
                if (!E.ObjectsReloaded.Contains(ParentObject))
                {
                    E.ObjectsReloaded.Add(ParentObject);
                }
                PlayWorldSound(ParentObject.GetTag("ReloadSound"));
                if (E.Actor.IsPlayer())
                {
                    IComponent<GameObject>.AddPlayerMessage("You " + ((liquidVolume.Volume < liquidVolume.MaxVolume) ? "partially " : "") + "fill " + ParentObject.the + ParentObject.ShortDisplayName + " with your own " + liquidVolume.GetLiquidName() + ".");
                }
            }
            return true;
        }
    }
}


