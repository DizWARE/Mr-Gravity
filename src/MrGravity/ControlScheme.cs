using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MrGravity
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
        bool IsLeftPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Right has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsRightPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Down has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsDownPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Up has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsUpPressed(bool held);

        /// <summary>
        /// Checks to see if the control for A has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsAPressed(bool held);

        /// <summary>
        /// Checks to see if the control for B has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsBPressed(bool held);

        /// <summary>
        /// Checks to see if the control for X has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsXPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Y has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsYPressed(bool held);

        /// <summary>
        /// Checks to see if the control for RT has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsRightTriggerPressed(bool held);

        /// <summary>
        /// Checks to see if the control for LT has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsLeftTriggerPressed(bool held);

        /// <summary>
        /// Checks to see if the control for RS has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsRightShoulderPressed(bool held);

        /// <summary>
        /// Checks to see if the control for LS has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsLeftShoulderPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Start has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsStartPressed(bool held);

        /// <summary>
        /// Checks to see if the control for Select has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsBackPressed(bool held);

        /// <summary>
        /// Checks to see if the control for HOME has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool IsHomePressed(bool held);

        /// <summary>
        /// Gets the type of control scheme this is
        /// </summary>
        /// <returns>Returns the Scheme for these controls. The enum for this is under ControlSchemes</returns>
        ControlSchemes ControlScheme();
    }

    /// <summary>
    /// Keyboard Scheme; Used to handle keyboard controls
    /// </summary>
    internal class KeyboardControl : IControlScheme
    {
        private readonly List<Keys> _pressedKeys = new List<Keys>();

        /// <summary>
        /// Checks to see if the given key is pressed
        /// </summary>
        /// <param name="key">Key to be checked</param>
        /// <param name="held">Set true if this button is allowed to be held down and still respond to interaction</param>
        /// <returns>
        /// True if the key is pressed, false otherwise
        /// </returns>
        private bool IsPressed(Keys key, bool held)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(key))
            {
                if (_pressedKeys.Contains(key) && !held) return false;
                if (!_pressedKeys.Contains(key)) 
                    _pressedKeys.Add(key);
                return true;
            }
            if (_pressedKeys.Contains(key))  _pressedKeys.Remove(key);
            return false;
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
            return IsPressed(Keys.Left, held);
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
            return IsPressed(Keys.Right, held);
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
            return IsPressed(Keys.Down, held);
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
            return IsPressed(Keys.Up, held);
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
            return IsPressed(Keys.A, held);
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
            return IsPressed(Keys.B, held);
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
            return IsPressed(Keys.X, held);
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
            return IsPressed(Keys.Y, held);
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
            return IsPressed(Keys.PageDown, held);
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
            return IsPressed(Keys.PageUp, held);
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
            return IsPressed(Keys.OemPlus, held);
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
            return IsPressed(Keys.OemMinus, held);
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
            return IsPressed(Keys.Enter, held);
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
            return IsPressed(Keys.Escape, held);
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
            return IsPressed(Keys.Home, held);
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
    internal class ControllerControl : IControlScheme
    {
        private readonly List<Buttons> _pressedButtons = new List<Buttons>();

        private Vector2 _joystickDirection;

        private bool _mControllerIndexSet;
        public PlayerIndex ControllerIndex { get; private set; }

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
            var pressed = false;
            if (!_mControllerIndexSet)
            {
                if (GamePad.GetState(PlayerIndex.One).IsConnected) pressed = pressed || isPressed(button, held, PlayerIndex.One);
                if (GamePad.GetState(PlayerIndex.Two).IsConnected) pressed = pressed || isPressed(button, held, PlayerIndex.Two);
                if (GamePad.GetState(PlayerIndex.Three).IsConnected) pressed = pressed || isPressed(button, held, PlayerIndex.Three);
                if (GamePad.GetState(PlayerIndex.Four).IsConnected) pressed = pressed || isPressed(button, held, PlayerIndex.Four);
                if(pressed) _mControllerIndexSet = true;
            }
            else pressed = pressed || isPressed(button, held, ControllerIndex);
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
                if (!_mControllerIndexSet) 
                { ControllerIndex = playerIndex; }
                if (_pressedButtons.Contains(button) && !held) return false;
                if (!_pressedButtons.Contains(button))
                    _pressedButtons.Add(button);
                return true;
            }
            if (_pressedButtons.Contains(button)) _pressedButtons.Remove(button);
            return false;
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
            var isLeftPressed = GamePad.GetState(ControllerIndex).DPad.Left == ButtonState.Pressed;
            var isUpPressed = _pressedButtons.Contains(Buttons.DPadUp);
            var isDownPressed = _pressedButtons.Contains(Buttons.DPadDown);

            if (!isLeftPressed && _pressedButtons.Contains(Buttons.DPadLeft)) _pressedButtons.Remove(Buttons.DPadLeft);
            else if (!isUpPressed && !isDownPressed && isLeftPressed) return ((isPressed(Buttons.DPadLeft, held, ControllerIndex)));
            else if (!isUpPressed && !isDownPressed) return (LeftThumbStickIsLeft(held));
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
            var isRightPressed = GamePad.GetState(ControllerIndex).DPad.Right == ButtonState.Pressed;
            var isUpPressed = _pressedButtons.Contains(Buttons.DPadUp);
            var isDownPressed = _pressedButtons.Contains(Buttons.DPadDown);

            if (!isRightPressed && _pressedButtons.Contains(Buttons.DPadRight)) _pressedButtons.Remove(Buttons.DPadRight);
            else if (!isUpPressed && !isDownPressed && isRightPressed) return ((isPressed(Buttons.DPadRight, held, ControllerIndex)));
            else if (!isUpPressed && !isDownPressed) return (LeftThumbStickIsRight(held));

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
            var isDownPressed = GamePad.GetState(ControllerIndex).DPad.Down == ButtonState.Pressed;
            var isLeftPressed = _pressedButtons.Contains(Buttons.DPadLeft);
            var isRightPressed = _pressedButtons.Contains(Buttons.DPadRight);

            if (!isDownPressed && _pressedButtons.Contains(Buttons.DPadDown)) _pressedButtons.Remove(Buttons.DPadDown);
            else if (!isLeftPressed && !isRightPressed && isDownPressed) return ((isPressed(Buttons.DPadDown, held, ControllerIndex)));
            else if (!isLeftPressed && !isRightPressed) return (LeftThumbStickIsDown(held));
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
            var isUpPressed = GamePad.GetState(ControllerIndex).DPad.Up == ButtonState.Pressed;
            var isLeftPressed = _pressedButtons.Contains(Buttons.DPadLeft);
            var isRightPressed = _pressedButtons.Contains(Buttons.DPadRight);

            if (!isUpPressed && _pressedButtons.Contains(Buttons.DPadUp)) _pressedButtons.Remove(Buttons.DPadUp);
            else if (!isLeftPressed && !isRightPressed && isUpPressed) return ((isPressed(Buttons.DPadUp, held, ControllerIndex)));
            else if (!isLeftPressed && !isRightPressed) return (LeftThumbStickIsUp(held));
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
            GamePadState state = GamePad.GetState(ControllerIndex);

            var direction = new Vector2(-1, 0);
            if (state.ThumbSticks.Left.X < -.8f)//Check to see if this is slipping in from a diagnal
            {
                if (Math.Abs(state.ThumbSticks.Left.Y) > .8 && Math.Abs(_joystickDirection.Y) == 1) return false;
                if (!held && direction == _joystickDirection) return false;
                if (held && direction == _joystickDirection) return true;
                if (!held && direction != _joystickDirection) { _joystickDirection = direction; return true; }
            }
            else if (state.ThumbSticks.Left.X >= 0 && _joystickDirection == direction) { _joystickDirection = new Vector2(); return false; }

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
            GamePadState state = GamePad.GetState(ControllerIndex);

            var direction = new Vector2(1, 0);
            if (state.ThumbSticks.Left.X > .8f)
            {
                if (Math.Abs(state.ThumbSticks.Left.Y) > .8 && Math.Abs(_joystickDirection.Y) == 1) return false;
                if (!held && direction == _joystickDirection) return false;
                if (held && direction == _joystickDirection) return true;
                if (!held && direction != _joystickDirection) { _joystickDirection = direction; return true; }
            }
            else if (state.ThumbSticks.Left.X <= 0 && _joystickDirection == direction) { _joystickDirection = new Vector2(); return false; }
            
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
            GamePadState state = GamePad.GetState(ControllerIndex);
                
            var direction = new Vector2(0, -1);
            if (state.ThumbSticks.Left.Y < -.8f)
            {
                if (!held && direction == _joystickDirection) return false;
                if (held && direction == _joystickDirection) return true;
                if (!held && direction != _joystickDirection) { _joystickDirection = direction; return true; }
            }
            else if (state.ThumbSticks.Left.Y >= 0 && _joystickDirection == direction) { _joystickDirection = new Vector2(); return false; }
            
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
            GamePadState state = GamePad.GetState(ControllerIndex);

            var direction = new Vector2(0, 1);
            if (state.ThumbSticks.Left.Y > .8f)
            {
                if (!held && direction == _joystickDirection) return false;
                if (held && direction == _joystickDirection) return true;
                if (!held && direction != _joystickDirection) { _joystickDirection = direction; return true; }
            }
            else if (state.ThumbSticks.Left.Y <= 0 && _joystickDirection == direction) _joystickDirection = new Vector2();
            
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
