using XRL.Wish;
using XRL.World;
using XRL.World.Parts;

[HasWishCommand]
public class JHG_Gunbox
{
    [WishCommand(Command = "jhg_gunbox")]
    public static void SpawnGunbox()
    {
        GameObject player = IPart.ThePlayer;
        GameObject box = GameObjectFactory.Factory.CreateObject("Chest");
        foreach (string itemId in GameObjectFactory.Factory.Blueprints.Keys)
        {
            if (itemId.StartsWith("jhg_"))
            {
                GameObjectBlueprint blueprint = GameObjectFactory.Factory.Blueprints[itemId];
                // TODO: change this when melee weapons are added
                if (blueprint.HasPart("MissileWeapon"))
                {
                    GameObject gun = GameObjectFactory.Factory.CreateObject(itemId);
                    gun.MakeUnderstood();
                    box.Inventory.AddObject(gun);
                }
            }
        }
        // leadslugs
        player.TakeObject("Lead Slug", 10000);
        player.GetCurrentCell().GetCellFromDirection("N").AddObject(box);
        return;
    }
}
