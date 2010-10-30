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
    enum ControlSchemes { Keyboard, Gamepad }

    /// <summary>
    /// Interface for the control scheme of Gravity Shift
    /// </summary>
    interface IControlScheme
    {
        /// <summary>
        /// Checks to see if the control for left has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isLeftPressed();

        /// <summary>
        /// Checks to see if the control for Right has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isRightPressed();

        /// <summary>
        /// Checks to see if the control for Down has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isDownPressed();

        /// <summary>
        /// Checks to see if the control for Up has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        bool isUpPressed();

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
        private const double BURN_DOWN_TIME = 1000;
        GameTime mGametime = new GameTime(System.TimeSpan.Zero, System.TimeSpan.Zero, System.TimeSpan.Zero, System.TimeSpan.Zero);
        bool upIsPushed = false;
        bool downIsPushed = false;
        bool leftIsPushed = false;
        bool rightIsPushed = false;
        bool upWasPushed = false;
        bool downWasPushed = false;
        bool leftWasPushed = false;
        bool rightWasPushed = false;
        /// <summary>
        /// Checks to see if the given key is pressed
        /// </summary>
        /// <param name="key">Key to be checked</param>
        /// <returns>True if the key is pressed, false otherwise</returns>
        private bool isPressed(Keys key)
        {
            KeyboardState state = Keyboard.GetState();
            return state.IsKeyDown(key);
        }

        /// <summary>
        /// Checks to see if the control for left has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        public bool isLeftPressed()
        {
            if (isPressed(Keys.Left))
            {
                leftIsPushed = true;
            }
            else
            {
                leftIsPushed = false;
            }

            if (leftIsPushed && !leftWasPushed)
            {
                return true;
            }
            leftWasPushed = leftIsPushed;
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Right has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        public bool isRightPressed()
        {
            if (isPressed(Keys.Right))
            {
                rightIsPushed = true;
            }
            else
            {
                rightIsPushed = false;
            }

            if (rightIsPushed && !rightWasPushed)
            {
                return true;
            }
            rightWasPushed = rightIsPushed;
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Down has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        public bool isDownPressed()
        {
            if (isPressed(Keys.Down))
            {
                downIsPushed = true;
            }
            else
            {
                downIsPushed = false;
            }

            if (downIsPushed && !downWasPushed)
            {
                return true;
            }
            downWasPushed = downIsPushed;
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Up has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        public bool isUpPressed()
        {
            if (isPressed(Keys.Up))
            {
                upIsPushed = true;
            }
            else
            {
                upIsPushed = false;
            }
            if (upIsPushed && !upWasPushed)
            {
                return true;
            }
            upWasPushed = upIsPushed;
            return false;
        }

       
        /// <summary>
        /// Gets the type of control scheme this is
        /// </summary>
        /// <returns>Returns the Scheme for these controls. The enum for this is under ControlSchemes</returns>
        public ControlSchemes controlScheme()
        {
            return ControlSchemes.Keyboard;
        }
    }

    class ControllerControl : IControlScheme
    {
        private PlayerIndex mPlayerIndex;

        bool upIsPushed = false;
        bool downIsPushed = false;
        bool leftIsPushed = false;
        bool rightIsPushed = false;
        bool upWasPushed = false;
        bool downWasPushed = false;
        bool leftWasPushed = false;
        bool rightWasPushed = false;

        /// <summary>
        /// Constructor - Constructs a scheme for the controller input type
        /// </summary>
        /// <param name="index">Player index of the controller</param>
        public ControllerControl(PlayerIndex index)
        {
            mPlayerIndex = index;
        }

        /// <summary>
        /// Checks to see if the given button is currently pressed
        /// </summary>
        /// <param name="button">Button we want to test</param>
        /// <returns>True if the given button has been pressed; False otherwise</returns>
        private bool isPressed(Buttons button)
        {
            GamePadState state = GamePad.GetState(mPlayerIndex);
            return state.IsButtonDown(button);
        }

        /// <summary>
        /// Checks to see if the control for left has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        public bool isLeftPressed()
        {
            if (isPressed(Buttons.DPadLeft) || isPressed(Buttons.LeftThumbstickLeft) || isPressed(Buttons.X))
            {
                leftIsPushed = true;
            }
            else
            {
                leftIsPushed = false;
            }
            if (leftIsPushed && !leftWasPushed)
            {
                return true;
            }
            leftWasPushed = leftIsPushed;
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Right has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        public bool isRightPressed()
        {
            if (isPressed(Buttons.DPadRight) || isPressed(Buttons.LeftThumbstickRight) || isPressed(Buttons.B))
            {
                rightIsPushed = true;
            }
            else
            {
                rightIsPushed = false;
            }

            if (rightIsPushed && !rightWasPushed)
            {
                return true;
            }
            rightWasPushed = rightIsPushed;
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Down has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        public bool isDownPressed()
        {
            if (isPressed(Buttons.DPadDown) || isPressed(Buttons.LeftThumbstickDown) || isPressed(Buttons.A))
            {
                downIsPushed = true;
            }
            else
            {
                downIsPushed = false;
            }

            if (downIsPushed && !downWasPushed)
            {
                return true;
            }
            downWasPushed = downIsPushed;
            return false;
        }

        /// <summary>
        /// Checks to see if the control for Up has been interacted with
        /// Only work when the button has just been pushed
        /// </summary>
        /// <returns>True if it has been pressed; false otherwise</returns>
        public bool isUpPressed()
        {
            if (isPressed(Buttons.DPadUp) || isPressed(Buttons.LeftThumbstickUp) || isPressed(Buttons.Y))
            {
                upIsPushed = true;
            }
            else
            {
                upIsPushed = false;
            }
            if (upIsPushed && !upWasPushed)
            {
                return true;
            }
            upWasPushed = upIsPushed;
            return false;
            
        }
        /// <summary>
        /// Checks to see if the left thumbstick is currently left
        /// </summary>
        /// <returns>True if it is; false if it is not</returns>
        private bool LeftThumbStickIsLeft()
        {
            return (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X == -1.0f);
        }
        /// <summary>
        /// Checks to see if the left thumbstick is currently right
        /// </summary>
        /// <returns>True if it is; false if it is not</returns>
        private bool LeftThumbStickIsRight()
        {
            return (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X == 1.0f);
        }
        /// <summary>
        /// Checks to see if the left thumbstick is currently Down
        /// </summary>
        /// <returns>True if it is; false if it is not</returns>
        private bool LeftThumbStickIsDown()
        {
            return (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y == 1.0f);
        }
        /// <summary>
        /// Checks to see if the left thumbstick is currently Up
        /// </summary>
        /// <returns>True if it is; false if it is not</returns>
        private bool LeftThumbStickIsUp()
        {
            return (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y == -1.0f);
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
