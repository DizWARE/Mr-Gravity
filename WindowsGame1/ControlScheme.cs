using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GravityShift
{
    /// <summary>
    /// Options for the type of controls this scheme could be based on
    /// </summary>
    public enum ControlSchemes { Keyboard, Gamepad }

    /// <summary>
    /// Interface for the control scheme of Gravity Shift
    /// </summary>
    public interface IControlScheme
    {
        /// <summary>
        /// Checks to see if the control for left has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isLeftPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Right has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isRightPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Down has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isDownPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Up has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isUpPressed(bool held);

        /// <summary>
        /// Checks to see if the control for A has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isAPressed(bool held);

        /// <summary>
        /// Checks to see if the control for B has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isBPressed(bool held);

        /// <summary>
        /// Checks to see if the control for X has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isXPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Y has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isYPressed(bool held);

        /// <summary>
        /// Checks to see if the control for RT has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isRightTriggerPressed(bool held);

        /// <summary>
        /// Checks to see if the control for LT has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isLeftTriggerPressed(bool held);

        /// <summary>
        /// Checks to see if the control for RS has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isRightShoulderPressed(bool held);

        /// <summary>
        /// Checks to see if the control for LS has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isLeftShoulderPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Start has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isStartPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Select has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isBackPressed(bool held);

        /// <summary>
        /// Checks to see if the control for HOME has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isHomePressed(bool held);

        /// <summary>
        /// Gets the type of control scheme this is
        /// </summary>
        /// <returns>Returns the Scheme for these controls. The enum for this is under ControlSchemes</returns>
        ControlSchemes controlScheme();
    }

    /// <summary>
    /// Keyboard Scheme; Used to handle keyboard controls
    /// </summary>
    class KeyboardControl : IControlScheme
    {
        List<Keys> pressedKeys = new List<Keys>();

        /// <summary>
        /// Checks to see if the given key is pressed
        /// </summary>
        /// <param name="key">Key to be checked</param>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if the key is pressed, false otherwise
        /// </returns>
        private bool isPressed(Keys key, bool held)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(key))
            {
                if (pressedKeys.Contains(key) && !held) return false;
                if (!pressedKeys.Contains(key)) 
                    pressedKeys.Add(key);
                return true;
            }
            else
            {
                if (pressedKeys.Contains(key))  pressedKeys.Remove(key);
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the control for left has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isLeftPressed(bool held)
        {
            return isPressed(Keys.Left, held);
        }

        /// <summary>
        /// Checks to see if the control for Right has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isRightPressed(bool held)
        {
            return isPressed(Keys.Right, held);
        }

        /// <summary>
        /// Checks to see if the control for Down has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isDownPressed(bool held)
        {
            return isPressed(Keys.Down, held);
        }

        /// <summary>
        /// Checks to see if the control for Up has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isUpPressed(bool held)
        {
            return isPressed(Keys.Up, held);
        }

        /// <summary>
        /// Checks to see if the control for A has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isAPressed(bool held)
        {
            return isPressed(Keys.A, held);
        }

        /// <summary>
        /// Checks to see if the control for B has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isBPressed(bool held)
        {
            return isPressed(Keys.B, held);
        }

        /// <summary>
        /// Checks to see if the control for X has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isXPressed(bool held)
        {
            return isPressed(Keys.X, held);
        }

        /// <summary>
        /// Checks to see if the control for Y has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isYPressed(bool held)
        {
            return isPressed(Keys.Y, held);
        }

        /// <summary>
        /// Checks to see if the control for RT has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isRightTriggerPressed(bool held)
        {
            return isPressed(Keys.PageDown, held);
        }

        /// <summary>
        /// Checks to see if the control for LT has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isLeftTriggerPressed(bool held)
        {
            return isPressed(Keys.PageUp, held);
        }

        /// <summary>
        /// Checks to see if the control for RS has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isRightShoulderPressed(bool held)
        {
            return isPressed(Keys.OemPlus, held);
        }

        /// <summary>
        /// Checks to see if the control for LS has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isLeftShoulderPressed(bool held)
        {
            return isPressed(Keys.OemMinus, held);
        }

        /// <summary>
        /// Checks to see if the control for Start has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isStartPressed(bool held)
        {
            return isPressed(Keys.Enter, held);
        }

        /// <summary>
        /// Checks to see if the control for back has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isBackPressed(bool held)
        {
            return isPressed(Keys.Escape, held);
        }

        /// <summary>
        /// Checks to see if the control for home has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isHomePressed(bool held)
        {
            return isPressed(Keys.Home, held);
        }

        /// <summary>
        /// Gets the type of control scheme this is
        /// </summary>
        /// <returns>
        /// Returns the Scheme for these controls. The enum for this is under ControlSchemes
        /// </returns>
        public ControlSchemes controlScheme()
        {
            return ControlSchemes.Keyboard;
        }
    }


    /// <summary>
    /// Control interface for a controller
    /// </summary>
    class ControllerControl : IControlScheme
    {
        List<Buttons> pressedButtons = new List<Buttons>();

        Vector2 joystickDirection;

        /// <summary>
        /// Finds the first player index that is connected
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        ///   <c>true</c> if the specified button is pressed; otherwise, <c>false</c>.
        /// </returns>
        private bool isPressed(Buttons button, bool held)
        {
            bool pressed = false;
            if (GamePad.GetState(PlayerIndex.One).IsConnected) pressed = pressed || isPressed(button, held, PlayerIndex.One);
            if (GamePad.GetState(PlayerIndex.Two).IsConnected) pressed = pressed || isPressed(button, held, PlayerIndex.Two);
            if (GamePad.GetState(PlayerIndex.Three).IsConnected) pressed = pressed || isPressed(button, held, PlayerIndex.Three);
            if (GamePad.GetState(PlayerIndex.Four).IsConnected) pressed = pressed || isPressed(button, held, PlayerIndex.Four);
            return pressed;
        }

        /// <summary>
        /// Checks to see if the given button is currently pressed
        /// </summary>
        /// <param name="button">Button we want to test</param>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns>
        /// True if the given button has been pressed; False otherwise
        /// </returns>
        private bool isPressed(Buttons button, bool held, PlayerIndex playerIndex)
        {
            GamePadState state = GamePad.GetState(playerIndex);
            
            if (state.IsButtonDown(button))
            {
                if (pressedButtons.Contains(button) && !held) return false;
                if (!pressedButtons.Contains(button))
                    pressedButtons.Add(button);
                return true;
            }
            else
            {
                if (pressedButtons.Contains(button)) pressedButtons.Remove(button);
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the control for left has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isLeftPressed(bool held)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                bool isLeftPressed = GamePad.GetState(current).DPad.Left == ButtonState.Pressed;
                bool isUpPressed = pressedButtons.Contains(Buttons.DPadUp);
                bool isDownPressed = pressedButtons.Contains(Buttons.DPadDown);

                if (!isLeftPressed && pressedButtons.Contains(Buttons.DPadLeft)) pressedButtons.Remove(Buttons.DPadLeft);
                else if (!isUpPressed && !isDownPressed && isLeftPressed) return ((isPressed(Buttons.DPadLeft, held, current)));
                else if (!isUpPressed && !isDownPressed) return (LeftThumbStickIsLeft(held));
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Right has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isRightPressed(bool held)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                bool isRightPressed = GamePad.GetState(current).DPad.Right == ButtonState.Pressed;
                bool isUpPressed = pressedButtons.Contains(Buttons.DPadUp);
                bool isDownPressed = pressedButtons.Contains(Buttons.DPadDown);

                if (!isRightPressed && pressedButtons.Contains(Buttons.DPadRight)) pressedButtons.Remove(Buttons.DPadRight);
                else if (!isUpPressed && !isDownPressed && isRightPressed) return ((isPressed(Buttons.DPadRight, held, current)));
                else if (!isUpPressed && !isDownPressed) return (LeftThumbStickIsRight(held));
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Down has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held"></param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isDownPressed(bool held)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                bool isDownPressed = GamePad.GetState(current).DPad.Down == ButtonState.Pressed;
                bool isLeftPressed = pressedButtons.Contains(Buttons.DPadLeft);
                bool isRightPressed = pressedButtons.Contains(Buttons.DPadRight);

                if (!isDownPressed && pressedButtons.Contains(Buttons.DPadDown)) pressedButtons.Remove(Buttons.DPadDown);
                else if (!isLeftPressed && !isRightPressed && isDownPressed) return ((isPressed(Buttons.DPadDown, held, current)));
                else if (!isLeftPressed && !isRightPressed) return (LeftThumbStickIsDown(held));
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Up has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isUpPressed(bool held)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                bool isUpPressed = GamePad.GetState(current).DPad.Up == ButtonState.Pressed;
                bool isLeftPressed = pressedButtons.Contains(Buttons.DPadLeft);
                bool isRightPressed = pressedButtons.Contains(Buttons.DPadRight);

                if (!isUpPressed && pressedButtons.Contains(Buttons.DPadUp)) pressedButtons.Remove(Buttons.DPadUp);
                else if (!isLeftPressed && !isRightPressed && isUpPressed) return ((isPressed(Buttons.DPadUp, held, current)));
                else if (!isLeftPressed && !isRightPressed) return (LeftThumbStickIsUp(held));
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the left thumbstick is currently left
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it is; false if it is not
        /// </returns>
        private bool LeftThumbStickIsLeft(bool held)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                GamePadState state = GamePad.GetState(current);

                Vector2 direction = new Vector2(-1, 0);
                if (state.ThumbSticks.Left.X < -.8f)//Check to see if this is slipping in from a diagnal
                {
                    if (Math.Abs(state.ThumbSticks.Left.Y) > .8 && Math.Abs(joystickDirection.Y) == 1) return false;
                    if (!held && direction == joystickDirection) return false;
                    if (held && direction == joystickDirection) return true;
                    if (!held && direction != joystickDirection) { joystickDirection = direction; return true; }
                }
                else if (state.ThumbSticks.Left.X >= 0 && joystickDirection == direction) { joystickDirection = new Vector2(); return false; }
            }
            return false;
        }
        /// <summary>
        /// Checks to see if the left thumbstick is currently right
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it is; false if it is not
        /// </returns>
        private bool LeftThumbStickIsRight(bool held)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                GamePadState state = GamePad.GetState(current);

                Vector2 direction = new Vector2(1, 0);
                if (state.ThumbSticks.Left.X > .8f)
                {
                    if (Math.Abs(state.ThumbSticks.Left.Y) > .8 && Math.Abs(joystickDirection.Y) == 1) return false;
                    if (!held && direction == joystickDirection) return false;
                    if (held && direction == joystickDirection) return true;
                    if (!held && direction != joystickDirection) { joystickDirection = direction; return true; }
                }
                else if (state.ThumbSticks.Left.X <= 0 && joystickDirection == direction) { joystickDirection = new Vector2(); return false; }
            }
            return false;
        }
        /// <summary>
        /// Checks to see if the left thumbstick is currently Down
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it is; false if it is not
        /// </returns>
        private bool LeftThumbStickIsDown(bool held)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                GamePadState state = GamePad.GetState(current);
                
                Vector2 direction = new Vector2(0, -1);
                if (state.ThumbSticks.Left.Y < -.8f)
                {
                    if (!held && direction == joystickDirection) return false;
                    if (held && direction == joystickDirection) return true;
                    if (!held && direction != joystickDirection) { joystickDirection = direction; return true; }
                }
                else if (state.ThumbSticks.Left.Y >= 0 && joystickDirection == direction) { joystickDirection = new Vector2(); return false; }
            }
            return false;
        }
        /// <summary>
        /// Checks to see if the left thumbstick is currently Up
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it is; false if it is not
        /// </returns>
        private bool LeftThumbStickIsUp(bool held)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                GamePadState state = GamePad.GetState(current);

                Vector2 direction = new Vector2(0, 1);
                if (state.ThumbSticks.Left.Y > .8f)
                {
                    if (!held && direction == joystickDirection) return false;
                    if (held && direction == joystickDirection) return true;
                    if (!held && direction != joystickDirection) { joystickDirection = direction; return true; }
                }
                else if (state.ThumbSticks.Left.Y <= 0 && joystickDirection == direction) joystickDirection = new Vector2();
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the control for A has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isAPressed(bool held)
        {
            return isPressed(Buttons.A, held);
        }

        /// <summary>
        /// Checks to see if the control for B has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held"></param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isBPressed(bool held)
        {
            return isPressed(Buttons.B, held);
        }

        /// <summary>
        /// Checks to see if the control for X has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isXPressed(bool held)
        {
            return isPressed(Buttons.X, held);
        }

        /// <summary>
        /// Checks to see if the control for Y has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isYPressed(bool held)
        {
            return isPressed(Buttons.Y, held);
        }

        /// <summary>
        /// Checks to see if the control for RT has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isRightTriggerPressed(bool held)
        {
            return isPressed(Buttons.RightTrigger, held);
        }

        /// <summary>
        /// Checks to see if the control for LT has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isLeftTriggerPressed(bool held)
        {
            return isPressed(Buttons.LeftTrigger, held);
        }

        /// <summary>
        /// Checks to see if the control for RS has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isRightShoulderPressed(bool held)
        {
            return isPressed(Buttons.RightShoulder, held);
        }

        /// <summary>
        /// Checks to see if the control for LS has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isLeftShoulderPressed(bool held)
        {
            return isPressed(Buttons.LeftShoulder, held);
        }


        /// <summary>
        /// Checks to see if the control for Start has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held"></param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isStartPressed(bool held)
        {
            return isPressed(Buttons.Start, held);
        }

        /// <summary>
        /// Checks to see if the control for Select has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isBackPressed(bool held)
        {
            return isPressed(Buttons.Back, held);
        }

        /// <summary>
        /// Checks to see if the control for HOME has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if it has been pressed; false otherwise
        /// </returns>
        public bool isHomePressed(bool held)
        {
            return isPressed(Buttons.BigButton, held);
        }

        /// <summary>
        /// Gets the type of control scheme this is
        /// </summary>
        /// <returns>Returns the Scheme for these controls. The enum for this is under ControlSchemes</returns>
        public ControlSchemes controlScheme()
        {
            return ControlSchemes.Gamepad;
        }
    }
}
