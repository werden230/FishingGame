using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishingMiniGame.MiniGames;

namespace FishingGame;

public class FishingMiniGameState : GameState
{
    private Game1 game;
    private FishingMiniGameLogic fishingMiniGame;
    private Vector2 position = new Vector2(100, 50);
    
    public FishingMiniGameState(Game1 game)
    {
        this.game = game;
    }
    
    public override void Enter()
    {
        // Загружаем ресурсы для мини-игры
        Texture2D fishingBar = game.Content.Load<Texture2D>("Fishing_Bar");
        Texture2D fishingFish = game.Content.Load<Texture2D>("Fishing_Fish");
        Texture2D fishingBGTexture = game.Content.Load<Texture2D>("Fishing_BG");
        Texture2D whiteTexture = new Texture2D(game.GraphicsDevice, 1, 1);
        whiteTexture.SetData(new[] { Color.White });
        
        fishingMiniGame = new FishingMiniGameLogic(
            position,
            fishingBar,
            fishingFish,
            fishingBGTexture,
            whiteTexture
        );
    }
    
    public override void Update(GameTime gameTime)
    {
        fishingMiniGame?.Update(gameTime);
        
        // Проверяем завершение мини-игры
        if (fishingMiniGame?.IsGameActive == false)
        {
            game.ChangeState(new MainGameState(game));
        }
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        fishingMiniGame?.Draw(spriteBatch);
    }
    
    public override void Exit()
    {
        // Очищаем ресурсы мини-игры
        fishingMiniGame = null;
    }
}