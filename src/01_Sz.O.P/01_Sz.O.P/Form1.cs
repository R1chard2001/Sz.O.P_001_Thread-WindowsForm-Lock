using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace _01_Sz.O.P
{
    public partial class Form1 : Form
    {
        Graphics g; // osztályban lévő változó, külön metódusokban elérhessünk,
                    // majd ez segítségével rajzolunk

        Random rnd = new Random(); // csak az DrawRandomLine érdekesebbé tétele miatt
        public Form1()
        {
            InitializeComponent();
            g = Canvas.CreateGraphics(); // megmondjuk, hogy ennek használjuk
        }

        public void DrawRandomLine()
        {
            int x1 = rnd.Next(Canvas.Width);
            int y1 = rnd.Next(Canvas.Height);
            int x2 = rnd.Next(Canvas.Width);
            int y2 = rnd.Next(Canvas.Height);

            g.DrawLine(Pens.Black, x1,y1,x2,y2);
        }

        // kattintáskor véletlen helyre rak egy vonalat
        //private void Canvas_MouseDown(object sender, MouseEventArgs e)
        //{ 
        //    Thread t = new Thread(DrawRandomLine); 
        //    t.Start();
        //}

        // helytelen lock-olás, a metódus teljes futása alatt ellehetetlenítjük
        // a megfelelő erőforráshoz a hozzáférést
        //private void DrawLine(int x, int y)
        //{
        //    lock (g)
        //    {
        //        // A Canvas közepétől rajzolja a vonalat
        //        g.DrawLine(Pens.Black, Canvas.Width / 2, Canvas.Height / 2, x, y);

        //        // a megadott ideig (ms) vár
        //        Thread.Sleep(2500);

        //        // "Törli" a vonalat
        //        g.DrawLine(Pens.White, Canvas.Width / 2, Canvas.Height / 2, x, y);
        //    }

        //}

        // Helyes lock-olás, csak a kritikus szakaszban foglaljuk le
        // a kívánt erőforrást
        private void DrawLine(int x, int y)
        {
            lock (g) // A Canvas közepétől rajzolja a vonalat
            {
                g.DrawLine(Pens.Black, Canvas.Width / 2, Canvas.Height / 2, x, y);
            }
            // a megadott ideig (ms) vár
            Thread.Sleep(2500);
            lock (g) // "Törli" a vonalat
            {
                g.DrawLine(Pens.White, Canvas.Width / 2, Canvas.Height / 2, x, y);
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            Thread t = new Thread(() => DrawLine(e.X, e.Y));
            t.Start();
        }
    }
}
