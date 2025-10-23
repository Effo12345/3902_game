using System.Text.Json.Serialization;

namespace ECS.Util;

public static class LevelLoader
{
    private sealed class LevelData
    {
        public Vector2 PlayerStart { get; set; }
        public List<BlockData> Blocks { get; set; }
        public List<EnemyData> Enemies { get; set; }
        public List<CoinData> Coins { get; set; }
    }
    private sealed class BlockData
    {
        public string BlockType { get; set; }
        public bool IsBreakable { get; set; }
        public bool IsBumpable { get; set; }
        public Vector2 Position { get; set; }
        public ItemKind Kind { get; set; }
    }
    private sealed class EnemyData
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; } = new();
        public Vector2 Velocity { get; set; } = new();
    }
    public sealed class CoinData
    {
        public Vector2 Position;
    }
    public static void Load(Engine engine, ContentManager content, string levelJson)
    {
        string json = File.ReadAllText(levelJson);
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
        opts.Converters.Add(new JsonStringEnumConverter());
        var lvl = JsonSerializer.Deserialize<LevelData>(json, opts);

        var marioTexture = content.Load<Texture2D>("Sprites/mario");

        var player = PlayerFactory.MakePlayer(
            engine,
            marioTexture,
            "Content/Config/mario_spritesheet.json",
            "Content/Config/player_input.json");
        player.GetComponent<Position>().Value = lvl.PlayerStart;

        foreach (var e in lvl.Enemies)
        {
            EnemyFactory.MakeEnemy(engine, content, e.Name, e.Position, e.Velocity);
        }

        var itemTexture = content.Load<Texture2D>("Sprites/tiles");
        var itemsAnimCfg = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/tiles_spritesheet.json"));

        foreach (var b in lvl.Blocks)
        {
            var cfg = new TileEntityConfigData
            {
                Engine = engine,
                Pos = b.Position,
                Texture = itemTexture,
                Animation = itemsAnimCfg
            };
            if (string.Equals(b.BlockType, "UnusedBlock", StringComparison.OrdinalIgnoreCase))
            {
                var itemKind = b.Kind.ToString();
                TileEntityFactory.MakeItemBlock(cfg, b.BlockType, b.Kind, itemKind, "UsedBlock");
            }
            else if (b.IsBreakable)
            {
                TileEntityFactory.MakeBreakBlock(cfg, b.BlockType);
            }
            else if (b.IsBumpable)
            {
                TileEntityFactory.MakeBumpBlock(cfg, b.BlockType);
            }
            else
            {
                TileEntityFactory.MakeBlock(cfg, b.BlockType);
            }
        }
        foreach (var c in lvl.Coins)
        {
            var cfg = new TileEntityConfigData
            {
                Engine = engine,
                Pos = c.Position,
                Texture = itemTexture,
                Animation = itemsAnimCfg
            };
            TileEntityFactory.MakeCoin(cfg);
        }
    }
}