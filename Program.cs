using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Graph
{
    public class GraphForm : Form
    {
        public GraphForm()
        {
            // Налаштування форми
            this.Text = "Графік функції";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;

            this.Resize += (s, e) => this.Invalidate();
            this.Paint += (s, e) => DrawGraph(e.Graphics);
        }

        private void DrawGraph(Graphics graph)
        {
            graph.Clear(Color.White);

            using (Pen axisPen = new Pen(Color.Black, 1)) // Перо для осей
            using (Pen pen = new Pen(Color.SlateBlue, 2)) // Перо для графіка
            {
                float widthForm = this.ClientSize.Width;
                float heightForm = this.ClientSize.Height;

                float offsetY = heightForm / 2;

                float tMin = 2.3f;
                float tMax = 7.2f;
                float scaleX = widthForm / (tMax - tMin);
                float scaleY = heightForm / 4;

                graph.DrawLine(axisPen, 0, offsetY, widthForm, offsetY); // Вісь X
                graph.DrawLine(axisPen, widthForm / 2, 0, widthForm / 2, heightForm); // Вісь Y

                Font font = new Font("Arial", 10);
                Brush brush = Brushes.Black;

                // Підписи для осі X
                float tStep = 1.0f;
                for (float t = tMin; t <= tMax; t += tStep)
                {
                    int screenX = (int)((t - tMin) * scaleX);
                    graph.DrawString(t.ToString("0.0"), font, brush, screenX, offsetY + 5);
                }

                // Підписи для осі Y
                float yStep = 0.5f;
                for (float y = -2.0f; y <= 2.0f; y += yStep)
                {
                    int screenY = (int)(offsetY - y * scaleY);
                    graph.DrawString(y.ToString("0.0"), font, brush, widthForm / 2 + 5, screenY - 10);
                }

                // Малювання графіка
                float dt = 0.8f;
                int screenX1 = 0, screenY1 = 0;
                bool firstPoint = true;

                for (float t = tMin; t <= tMax; t += dt)
                {
                    float y = (float)(Math.Pow(Math.Cos(t * t), 3) / (1.5 * t + 2));

                    int screenX2 = (int)((t - tMin) * scaleX);
                    int screenY2 = (int)(offsetY - y * scaleY);

                    if (!firstPoint)
                    {
                        graph.DrawLine(pen, screenX1, screenY1, screenX2, screenY2);
                    }
                    else
                    {
                        firstPoint = false;
                    }

                    // Оновлення початкових координат
                    screenX1 = screenX2;
                    screenY1 = screenY2;
                }
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GraphForm());
        }
    }
}
