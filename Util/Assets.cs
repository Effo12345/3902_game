namespace ECS.Util; 

public class Assets
{
    private Dictionary<string, Texture2D> Textures = new();
    private Dictionary<string, AnimationConfig> SpriteAtlases = new();
    private Dictionary<string, InputConfig> InputConfigs = new();
    private Dictionary<string, LevelConfig> LevelConfigs = new();
    private Dictionary<string, EntityConfig> EntityConfigs = new();

    public void LoadTexture(ContentManager content, string key, string path)
    {
        Textures[key] = content.Load<Texture2D>(path);
    }

    public void LoadSpriteAtlas(string key, string path)
    {
        SpriteAtlases[key] = SpriteSheetLoader.Load(File.ReadAllText(path));
    }

    public void LoadInputConfig(string key, string path)
    {
        InputConfigs[key] = InputDefinitionLoader.Load(File.ReadAllText(path));
    }

    public void LoadLevelConfig(string key, string path)
    {
        LevelConfigs[key] = LevelLoader.Load(File.ReadAllText(path));
    }

    public void LoadEntityConfig(string key, string path)
    {
        EntityConfigs[key] = EntityLoader.Load(File.ReadAllText(path));
    }

    public Texture2D GetTexture(string key) => Textures[key];
    public AnimationConfig GetSpriteAtlas(string key) => SpriteAtlases[key];
    public InputConfig GetInputConfig(string key) => InputConfigs[key];
    public LevelConfig GetLevelConfig(string key) => LevelConfigs[key];
    public EntityConfig GetEntityConfig(string key) => EntityConfigs[key];
}