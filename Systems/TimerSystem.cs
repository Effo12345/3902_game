namespace ECS.Systems;

// current timer entity:
/*
        {
            "key": "timer",
            "components": {
                "TextRender": {
                    "Text": "timer",
                    "Font": "HUDFont",
                    "Color": [ 0, 0, 0, 0 ]
                }
            }
        }
 */
// Initially invisible. The text will be replaced by number.

public class TimerSystem : AbstractSystem
{

    private static readonly int fps = 60;

    private static readonly int _default_time_limit = 40;

    private int remainingTime;


    public TimerSystem()
    {
        remainingTime = -1;
    }



    override public void Update(Engine engine, GameTime gameTime)
    {

        Entity timer = null;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<TextRender>(out var text) ||
                (text.Text != "timer" && !(entity.TryGetComponent<Tag>(out var tag) && tag.Value == "timer")) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Rotation>(out var rotation) ||
                !entity.TryGetComponent<Scale>(out var scale)
            ) continue;
            timer = entity;
            break;
        }
        if (timer == null) return;
        TextRender textRender = timer.GetComponent<TextRender>();
        if (!timer.TryGetComponent<Tag>(out _))
        {
            timer.AddComponent<Tag>(new Tag { Value = "timer" });
            remainingTime = _default_time_limit * fps;
            textRender.Text = remainingTime.ToString();
            textRender.Color = Color.White;
        }

        if (remainingTime > 0)
        {
            remainingTime -= 1;
            textRender.Text = (remainingTime / fps).ToString();
            /* --- warning effects --- */
            if (remainingTime > 10 * fps)
            {
                textRender.Color = Color.White;
            }
            else if (remainingTime > 5 * fps)
            {
                textRender.Color = (remainingTime % fps >= fps / 2) ? Color.Red : Color.White;
            }
            else
            {
                textRender.Color = (remainingTime % (fps / 2) >= fps / 4) ? Color.Red : Color.White;
            }
            /* ----------------------- */
        }
        else if (remainingTime == 0)
        {
            // TODO: YOU DIE!
            // placeholder: display the text "fail"
            textRender.Text = "fail";
            textRender.Color = Color.Red;
        }
    }
}
