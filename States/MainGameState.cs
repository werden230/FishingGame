using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishingGame;

public class MainGameState : GameState
{
    private Game1 game;
    
    public MainGameState(Game1 game)
    {
        this.game = game;
    }
    
    public override void Update(GameTime gameTime)
    {
        // Проверяем нажатие пробела
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            game.ChangeState(new FishingMiniGameState(game));
        }
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        // spriteBatch.Draw(fishingBGSprite.texture, fishingBGSprite.position, Color.White);
        // Рисуйте остальные элементы основной игры
    }
}
