﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TimeSpan = System.TimeSpan;

namespace PongClient
{
    public partial class MainWindow : Window
    {
        //Player movement directions
        private bool Player1Up = false;
        private bool Player1Down = false;
        private bool Player2Up = false;
        private bool Player2Down = false;

        //Timer
        private DispatcherTimer PlayerMovementTimer = new DispatcherTimer();
        private DispatcherTimer AnimationTimer = new DispatcherTimer();

        //BallSpeed
        private double BeginnSpeedX = 220; //140
        private double BeginnSpeedY = 220; //220
        private double SpeedX = 220;//140
        private double SpeedY = 220;

        //Player Counts
        private int PlayerLeftCount = 0;
        private int PlayerRightCount = 0;

        //Ball Starting Position
        private double BeginnBallX = 200;
        private double BeginnBallY = 200; //150

        //Cooldown to prevent recollision with paddle
        private bool LeftCooldown = false;
        private bool RightCooldown = false;

        public MainWindow()
        {
            InitializeComponent();
            //Player Movement Timer Initialization
            PlayerMovementTimer.Interval = TimeSpan.FromMilliseconds(30);
            PlayerMovementTimer.Tick += MovePlayer!;
            PlayerMovementTimer.Start();

            //AnimationTimer Initialization
            AnimationTimer.Interval = TimeSpan.FromSeconds(0.05);
            AnimationTimer.Tick += Animate!;
            AnimationTimer.Start();
        }

        private void Animate(object sender, EventArgs e)
        {
            var x = Canvas.GetLeft(Ball);
            var y = Canvas.GetTop(Ball);

            //Ball collision with left paddle
            if (x <= LeftPaddle.Width
                && y >= Canvas.GetTop(LeftPaddle)
                && y + Ball.Height <= Canvas.GetTop(LeftPaddle) + LeftPaddle.Height
                && !LeftCooldown)
            {
                SpeedX = -SpeedX;
                LeftCooldown = true;
                RightCooldown = false;
            }
            //Ball collision with right paddle
            if (x + Ball.Width >= Gamefield.ActualWidth - (Canvas.GetRight(RightPaddle) + RightPaddle.Width)
                && y + RightPaddle.Width >= Canvas.GetTop(RightPaddle)
                && y + Ball.Height <= Canvas.GetTop(RightPaddle) + RightPaddle.Height
                && !RightCooldown)
            {
                SpeedX = -SpeedX;
                RightCooldown = true;
                LeftCooldown = false;
            }

            //Ball collision with walls
            if (y <= 0.0 || y >= Gamefield.ActualHeight - Ball.Height)
            {
                SpeedY = -SpeedY;
            }

            //Points for right Player
            if (x <= -10 - Ball.Width)
            {
                SpeedX = BeginnSpeedX;
                SpeedY = BeginnSpeedY;
                PlayerRightCount++;
                LeftCooldown = false;
                RightCooldown = false;
                Canvas.SetLeft(Ball, BeginnBallX);
                Canvas.SetTop(Ball, BeginnSpeedY);
                RightCount.Content = PlayerRightCount.ToString();
                return;
            }

            //Points for left Player
            if (x >= 10 + Gamefield.ActualWidth)
            {
                SpeedX = BeginnSpeedX;
                SpeedY = BeginnSpeedY;
                PlayerLeftCount++;
                LeftCooldown = false;
                RightCooldown = false;
                Canvas.SetLeft(Ball, BeginnBallX);
                Canvas.SetTop(Ball, BeginnSpeedY);
                LeftCount.Content = PlayerLeftCount.ToString();
                return;
            }

            //Speed up Ball
            SpeedX *= 1.001; //1.001
            SpeedY *= 1.001; //1.001

            //Ball Movement
            x += SpeedX * AnimationTimer.Interval.TotalSeconds;
            y += SpeedY * AnimationTimer.Interval.TotalSeconds;
            Canvas.SetLeft(Ball, x);
            Canvas.SetTop(Ball, y);
        }

        private void MovePlayer(object sender, EventArgs e)
        {
            if (Player1Down)
            {
                if (Canvas.GetTop(LeftPaddle) + LeftPaddle.Height < Gamefield.ActualHeight)
                {
                    Canvas.SetTop(LeftPaddle, Canvas.GetTop(LeftPaddle) + 10);
                }
            }
            else if (Player1Up)
            {
                if (Canvas.GetTop(LeftPaddle) > 0.0)
                {
                    Canvas.SetTop(LeftPaddle, Canvas.GetTop(LeftPaddle) - 10);
                }

            }
            if (Player2Down)
            {
                if (Canvas.GetTop(RightPaddle) + 100 < Gamefield.ActualHeight)
                {
                    Canvas.SetTop(RightPaddle, Canvas.GetTop(RightPaddle) + 10);
                }
            }
            else if (Player2Up)
            {
                if (Canvas.GetTop(RightPaddle) > 0.0)
                {
                    Canvas.SetTop(RightPaddle, Canvas.GetTop(RightPaddle) - 10);
                }
            }
        }

        #region Player Movement Events

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    {
                        Player2Up = true;
                        Player2Down = false;
                        break;
                    }

                case Key.Down:
                    {
                        Player2Up = false;
                        Player2Down = true;
                        break;
                    }
            }
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    {
                        Player2Up = false;
                        Player2Down = false;
                        break;
                    }

                case Key.Down:
                    {
                        Player2Up = false;
                        Player2Down = false;
                        break;
                    }
            }
        }


        private void Window_KeyDown_LeftPlayer(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    {
                        Player1Up = true;
                        Player1Down = false;
                        break;
                    }

                case Key.S:
                    {
                        Player1Up = false;
                        Player1Down = true;
                        break;
                    }
            }
        }

        private void Window_KeyUp_LeftPlayer(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    {
                        Player1Up = false;
                        Player1Down = false;
                        break;
                    }

                case Key.S:
                    {
                        Player1Up = false;
                        Player1Down = false;
                        break;
                    }
            }
        }

        #endregion
    }
}
