using Microsoft.Xna.Framework.Audio;

namespace ECS.Util;

public class Assets
{
    private readonly Dictionary<string, Texture2D> Textures = [];
    private readonly Dictionary<string, SpriteFont> Fonts = [];
    private readonly Dictionary<string, SoundEffect> Sounds = [];
    private readonly Dictionary<string, Effect> Shaders = [];
    private readonly Dictionary<string, AnimationConfig> SpriteAtlases = [];
    private readonly Dictionary<string, InputBindings> InputConfigs = [];
    private readonly Dictionary<string, LevelConfig> LevelConfigs = [];
    private readonly Dictionary<string, EntityConfig> EntityConfigs = [];

    public void LoadTexture(ContentManager content, string key, string path)
    {
        Textures[key] = content.Load<Texture2D>(path);
    }

    public void LoadFont(ContentManager content, string key, string path)
    {
        Fonts[key] = content.Load<SpriteFont>(path);
    }

    public void LoadSound(ContentManager content, string key, string path)
    {
        Sounds[key] = content.Load<SoundEffect>(path);
    }

    public void LoadShader(ContentManager content, string key, string path)
    {
        Shaders[key] = content.Load<Effect>(path);
    }

    public void LoadSpriteAtlas(string key, string path)
    {
        SpriteAtlases[key] = SpriteSheetLoader.Load(File.ReadAllText(path));
    }

    public void LoadInputConfig(string key, string path)
    {
        InputConfigs[key] = InputDefinitionLoader.LoadFromJson(File.ReadAllText(path));
    }

    public void LoadLevelConfig(string key, string path)
    {
        LevelConfigs[key] = LevelConfigLoader.Load(File.ReadAllText(path));
    }

    public void LoadEntityConfig(string key, string path)
    {
        EntityConfigs[key] = EntityLoader.Load(File.ReadAllText(path));
    }

    public Texture2D GetTexture(string key) => Textures[key];
    public SpriteFont GetFont(string key) => Fonts[key];
    public SoundEffect GetSound(string key) => Sounds[key];
    public Effect GetShader(string key) => Shaders[key];
    public AnimationConfig GetSpriteAtlas(string key) => SpriteAtlases[key];
    public LevelConfig GetLevelConfig(string key) => LevelConfigs[key];
    public InputBindings GetInputConfig(string key) => InputConfigs[key];
    public EntityConfig GetEntityConfig(string key) => EntityConfigs[key];
}