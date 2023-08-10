using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


//Доработать рассматриваемый на занятии (RectangleByClick) проектследующим образом:
//001 ячейки(клеточки) при создании чередуются: нечётные челёные, чётные красные. - DONE
//002 запретить наложение областей. клеточка не создаётся, если перекрывает существующую. - DONE
//003* добавить возможность перетаскивания клетки по форме. - DONE, но не до конца корректно
//004* добавить возможность удаления выбранной клетки. - DONE
//005** добавить возможность сохранения текущего состояния. -DONE

namespace RectangleByClick
{
    public partial class Form1 : Form
    {
        Graphics g;
        List<Figure> rectangles_lst = new List<Figure>();
        int counterCell = 0; // счетчик для подсчета ячеек ( для выбора цвета)
        private bool isDragging = false;         // флаг
        private Point coordinatesMove;
        private Figure selectedFigure = null;

        public Form1()
        {
            InitializeComponent();
            initCanvas();
            MessageBox.Show("*создать ячейку - двойной клик.\n**удалить ячейку - клик правой кнопкой мыши");
            saveBtn.Location = new Point(this.ClientSize.Width - saveBtn.Width - 10, this.ClientSize.Height - saveBtn.Height - 10); //кнопка в правом нижнем углу
            

        }
        private void initCanvas()
        {
            this.Size = new Size(900, 900);
        }
        private void addCell(Point point, Brush brush)
        {
            Rectangle alarm = new Rectangle(0, 0, 135, 135);
            if (alarm.Contains(point))
            {
                return;
            }
            g = CreateGraphics();
            int height = 90;
            int width = 90;
            Rectangle rectCell = new Rectangle(
                point.X - (width / 2), point.Y - (height / 2)
                , width, height);
            foreach (var item in rectangles_lst)  // заперт на наложение областей
            {
                if (item.rectangle.IntersectsWith(rectCell))
                {
                    return;
                }
            }
            Figure figure = new Figure(rectCell, brush);
            rectangles_lst.Add(figure);
            g.Clear(BackColor);
            foreach (var item in rectangles_lst)
            {
                g.FillRectangle(item.brush, item.rectangle);
            }
        }
        private void deleteCell()
        {
            if (selectedFigure != null)
            {
                rectangles_lst.Remove(selectedFigure);
                selectedFigure = null;
                Refresh();
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            counterCell++;
            if (counterCell % 2 == 0)
            {
                addCell(e.Location, Brushes.Red);
            }
            else
            {
                addCell(e.Location, Brushes.Green);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            string mess = string.Empty;
            int counter = 0;
            foreach (var item in rectangles_lst)
            {
                mess += $"№{++counter} {item.rectangle.X} : " +
                    $"{item.rectangle.Y} " + "\n";
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var item in rectangles_lst)
            {
                if (item.rectangle.Contains(e.Location))
                {
                    isDragging = true;
                    coordinatesMove = new Point(e.X - item.rectangle.X, e.Y - item.rectangle.Y);
                    selectedFigure = item;
                    break;
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedFigure != null)
            {
                selectedFigure.rectangle = new Rectangle(e.X - coordinatesMove.X, e.Y - coordinatesMove.Y, selectedFigure.rectangle.Width, selectedFigure.rectangle.Height);

                Refresh();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            selectedFigure = null;

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                deleteCell();
            }
        }
        private void SaveFile() // метод сохранения в PNG
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PNG Image|*.png";
                saveDialog.Title = "Save Image";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap bitmap = new Bitmap(this.Width, this.Height))
                    {
                        this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
                        bitmap.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            SaveFile();
        }
    }
}
