using HarmonyLib;
using System.Linq;
using TerrainSlabs.Source.Compatibility;
using TerrainSlabs.Source.HarmonyPatches;
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
            RenderersPatch.PatchAllRenderers(harmonyInstance);
            WorldAccessorParticlesPatch.PatchAllParticleCode(harmonyInstance);
            ParticlesManagerPatch.PatchAllParticleCode(harmonyInstance);

            CatchLedgePatch.ApplyIfEnabled(api, harmonyInstance);
        }

        api.RegisterBlockClass(nameof(BlockTrailSlab), typeof(BlockTrailSlab));
    }

    public override void Dispose()
    {
        harmonyInstance.UnpatchAll(harmonyInstance.Id);
    }
}
