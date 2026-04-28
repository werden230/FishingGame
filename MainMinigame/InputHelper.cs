using Microsoft.Xna.Framework.Input;

namespace FishingMiniGame.Utilities
{
    public class InputHelper
    {
        private MouseState previousMouseState;
        private float holdTime;
        
        public bool IsLeftButtonPressed { get; private set; }
        public bool IsLeftButtonHeld { get; private set; }
        public float HoldTime => holdTime;
        
        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState();
            bool isPressed = currentMouseState.LeftButton == ButtonState.Pressed;
            bool wasPressed = previousMouseState.LeftButton == ButtonState.Pressed;
            
            IsLeftButtonPressed = isPressed && !wasPressed;
            IsLeftButtonHeld = isPressed && wasPressed;
            
            if (IsLeftButtonHeld)
            {
                holdTime += 0.016f; // Примерно 60 FPS
            }
            else
            {
                holdTime = 0;
            }
            
            previousMouseState = currentMouseState;
        }
        
        public bool IsExitRequested()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape);
        }
    }
}