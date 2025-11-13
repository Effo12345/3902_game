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

    string CurrentLevel;

    public GameManager(Game1 game, Engine engine)
    {
        Game = game;
        Engine = engine;
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

        switch (inputEvent.Action)
        {
            case "Quit":
                Game.Exit();
                break;
            case "Reset":
                ChangeLevel(CurrentLevel);
                break;
            case "TestLevel":
                ChangeLevel("TestLevel");
                break;
            case "MarioLevel":
                ChangeLevel("Mario1_1");
                break;
        }

    }
}