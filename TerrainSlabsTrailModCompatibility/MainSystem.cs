using HarmonyLib;
using System.Linq;
using Vintagestory.API.Common;

namespace TerrainSlabsTrailModCompatibility;

public class MainSystem : ModSystem
{
    private Harmony harmonyInstance = null!;

    public override void StartPre(ICoreAPI api)
    {
        harmonyInstance = new(Mod.Info.ModID);
        if (!harmonyInstance.GetPatchedMethods().Any())
        {
            harmonyInstance.PatchAll();
        }

        api.RegisterBlockClass(nameof(BlockTrailSlab), typeof(BlockTrailSlab));
    }

    public override void Dispose()
    {
        harmonyInstance.UnpatchAll(harmonyInstance.Id);
    }
}
