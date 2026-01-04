using TerrainSlabs.Source.Blocks;
using TerrainSlabs.Source.Utils;
using TrailModUpdated;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace TerrainSlabsTrailModCompatibility;

public class BlockTrailSlab : BlockTrail
{
    private Block? fullBlock;

    public override void OnLoaded(ICoreAPI api)
    {
        base.OnLoaded(api);

        AssetLocation fullBlockCode = Code.UseFirstPartAsDomain();
        fullBlock = api.World.GetBlock(fullBlockCode);
        if (fullBlock is null)
        {
            api.Logger.Warning("Unable to get full block by code {0}", fullBlockCode);
        }
    }

    public override bool CanAcceptFallOnto(IWorldAccessor world, BlockPos pos, Block fallingBlock, TreeAttribute blockEntityAttributes)
    {
        if (fullBlock is not null)
        {
            return true;
        }
        return base.CanAcceptFallOnto(world, pos, fallingBlock, blockEntityAttributes);
    }

    public override bool OnFallOnto(IWorldAccessor world, BlockPos pos, Block block, TreeAttribute blockEntityAttributes)
    {
        return BlockTerrainSlab.OnFallOnto(this, fullBlock, world, pos, block, blockEntityAttributes);
    }

    protected override AssetLocation GetDevolveBlockAsset(string code)
    {
        AssetLocation fullBlockDevolveCode = new(Code.Domain, "trailmodupdated-" + code);
        return fullBlockDevolveCode;
    }

    // Get the third part, because the code format is like: terrainslabs:soil-trailmodupdated-fertility-new
    protected override string GetFertilityVariantCode()
    {
        int firstHyphenIndex = Code.Path.IndexOf('-');
        if (firstHyphenIndex < 0)
        {
            return Code.Path;
        }

        int secondHyphenIndex = Code.Path.IndexOf('-', firstHyphenIndex + 1);
        if (secondHyphenIndex < 0)
        {
            return Code.Path;
        }

        int thirdHyphenIndex = Code.Path.IndexOf('-', secondHyphenIndex + 1);
        if (thirdHyphenIndex >= 0)
        {
            // There's a fourth part, so extract the third part
            return Code.Path.Substring(secondHyphenIndex + 1, thirdHyphenIndex - secondHyphenIndex - 1);
        }

        // No fourth part, so take everything after the second hyphen
        return Code.Path[(secondHyphenIndex + 1)..];
    }
}
