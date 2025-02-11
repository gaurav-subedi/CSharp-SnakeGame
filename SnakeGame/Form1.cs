using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Point> snake;
        private Point food;
        private int direction; // 0 = Up, 1 = Down, 2 = Left, 3 = Right
        private int gridSize = 20;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            snake = new List<Point> { new Point(5 * gridSize, 5 * gridSize) }; // Initial Snake Position
            direction = 3; // Start moving right
            GenerateFood();
            timer1.Interval = 100; // Snake speed
            timer1.Start();
        }

        private void GenerateFood()
        {
            int maxX = pictureBox1.Width / gridSize;
            int maxY = pictureBox1.Height / gridSize;
            food = new Point(random.Next(0, maxX) * gridSize, random.Next(0, maxY) * gridSize);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (snake.Count == 0) return; // Prevent IndexOutOfRangeException

            // Move the snake
            Point newHead = snake[0];

            switch (direction)
            {
                case 0: newHead.Y -= gridSize; break; // Up
                case 1: newHead.Y += gridSize; break; // Down
                case 2: newHead.X -= gridSize; break; // Left
                case 3: newHead.X += gridSize; break; // Right
            }

            // Check collision with wall or itself
            if (newHead.X < 0 || newHead.Y < 0 ||
                newHead.X >= pictureBox1.Width || newHead.Y >= pictureBox1.Height ||
                snake.Contains(newHead))
            {
                timer1.Stop();
                MessageBox.Show("Game Over!", "Snake Game");
                InitializeGame();
                return;
            }

            // Check if snake eats food
            if (newHead == food)
            {
                snake.Insert(0, newHead); // Grow the snake
                GenerateFood(); // Generate new food
            }
            else
            {
                snake.Insert(0, newHead);
                snake.RemoveAt(snake.Count - 1); // Remove tail if not eating
            }

            pictureBox1.Invalidate(); // Redraw
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W: if (direction != 1) direction = 0; break;
                case Keys.S: if (direction != 0) direction = 1; break;
                case Keys.A: if (direction != 3) direction = 2; break;
                case Keys.D: if (direction != 2) direction = 3; break;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush snakeBrush = Brushes.Green;
            Brush foodBrush = Brushes.Red;

            // Draw the snake as squares
            foreach (Point p in snake)
            {
                g.FillRectangle(snakeBrush, p.X, p.Y, gridSize, gridSize);
            }

            // Draw the food as a circle
            g.FillEllipse(foodBrush, food.X, food.Y, gridSize, gridSize);
        }
    }
}
