using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace RectangleByClick
{
    internal class Figure
    {
        //private Rectangle _rectangle;
        public Rectangle rectangle { get; set; }
        //private Brush _brush;
        public Brush brush { get; } 
        public Figure(Rectangle rectangle, Brush brush)
        {
            this.rectangle = rectangle;
            this.brush = brush;
        }
    }
}
