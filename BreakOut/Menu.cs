using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace BreakOut;

/// <summary>
/// The Menu class represents a collection of menu buttons with one being active at any time.
/// </summary>
public class Menu : IGameEventProcessor
{
    private Text[] menuButtons;
    private int buttonAmmount;
    private int activeMenuButton;
    public Menu(Text[] buttons)
    {
        menuButtons = buttons;
        buttonAmmount = menuButtons.Length;
        activeMenuButton = 0;
        BreakOutBus.GetBus().Subscribe(GameEventType.GraphicsEvent, this);
        ResetMenu();
    }

    /// <summary>
    /// Returns the number of the currently active menu button
    /// </summary>
    public int ActiveButton()
    {
        return activeMenuButton;
    }

    /// <summary>
    /// Sets the colors of the menu buttons to white, except for the active one which is set to green.
    /// </summary>
    public void UpdateMenu()
    {
        foreach (Text text in menuButtons)
        {
            text.SetColor(System.Drawing.Color.White);
        }
        menuButtons[activeMenuButton].SetColor(System.Drawing.Color.Green);
    }

    public void ResetMenu()
    {
        activeMenuButton = 0;
        UpdateMenu();
    }

    public void RenderMenu()
    {
        foreach (Text text in menuButtons)
        {
            text.RenderText();
        }
    }

    /// <summary>
    /// Scrolls between menu buttons.
    /// </summary>
    public void ProcessEvent(GameEvent gameEvent)
    {
        switch (gameEvent.Message)
        {
            case ("SWITCH_UP"):
                if (activeMenuButton > 0) activeMenuButton--;
                break;
            case ("SWITCH_DOWN"):
                if (activeMenuButton < buttonAmmount - 1) activeMenuButton++;
                break;
        }
    }
}