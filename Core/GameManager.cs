namespace ECS.Core;

// We need to be cautious with this class.
// It seems necessary to move logic out of Game1 but
// it can become bloated super easily.
// This should ONLY be used for tracking GLOBAL state
// necessary for the ENTIRE game to function
public class GameManager
{
    Game1 Game;
    Engine Engine;

    readonly Dictionary<string, Action> Actions;

    string CurrentLevel;

    public GameManager(Game1 game, Engine engine)
    {
        Game = game;
        Engine = engine;

        Actions = new Dictionary<string, Action>
        {
            {CONSTANTS.ACTION.QUIT, () => Game.Exit()},
            {CONSTANTS.ACTION.RESET, () => ChangeLevel(CurrentLevel)},
            {CONSTANTS.ACTION.PACHINKO_LEVEL, () => ChangeLevel(CONSTANTS.LEVEL.PACHINKO_LEVEL)},
            {CONSTANTS.ACTION.MARIO_LEVEL, () => ChangeLevel(CONSTANTS.LEVEL.MARIO1_1)},
        };
    }

    public void ChangeLevel(string level)
    {
        SoundPlayer.StopAll();
        CurrentLevel = level;
        LevelLoader.Load(Engine, level);
    }

    public void Initialize(string startingLevel)
    {
        Engine.PubSub.CreateSubscription<InputActionEvent>(OnAction);

        ChangeLevel(startingLevel);
    }

    private void OnAction(IEvent evt)
    {
        var inputEvent = (InputActionEvent)evt;

        // Only care about pressed for the first time
        if (inputEvent.Phase != InputPhase.Press)
            return;

        if (Actions.TryGetValue(inputEvent.Action, out var action))
            action();
    }
}