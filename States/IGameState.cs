using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IGameState
{
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void Enter(); // Вызывается при входе в состояние
    void Exit();  // Вызывается при выходе из состояния
}

public abstract class GameState : IGameState
{
    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(SpriteBatch spriteBatch) { }
    public virtual void Enter() { }
    public virtual void Exit() { }
}