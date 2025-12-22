using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace school
{
    public class DataGridViewPrinter
    {
        public static void PrintDataGridView(DataGridView gridView, string title, PrintPageEventArgs e)
        {
            float x = PrintConfig.TitleX;
            float y = PrintConfig.TitleY;
            float pageWidth = PrintConfig.TitlePageWidth;

            // Заголовок
            e.Graphics.DrawString(title, new Font("Arial", PrintConfig.TitleFontSize, FontStyle.Bold),
                Brushes.Black, x + (pageWidth - PrintConfig.TitleOffset) / 2, y);
            y += 50;

            float colWidth = pageWidth / gridView.ColumnCount;

            for (int col = 0; col < gridView.ColumnCount; col++)
            {
                float colX = x + col * colWidth;
                e.Graphics.FillRectangle(PrintConfig.HeaderBgBrush, colX, y, colWidth, PrintConfig.HeaderHeight);
                e.Graphics.DrawRectangle(PrintConfig.HeaderBorderPen, colX, y, colWidth, PrintConfig.HeaderHeight);
                e.Graphics.DrawString(gridView.Columns[col].HeaderText, new Font("Arial", PrintConfig.HeaderFontSize, FontStyle.Bold),
                    Brushes.White, colX + PrintConfig.HeaderPaddingX, y + PrintConfig.HeaderPaddingY);
            }
            y += PrintConfig.HeaderOffsetY;

            for (int row = 0; row < gridView.RowCount && y < PrintConfig.MaxPageY; row++)
            {
                float neededHeight = GetRowHeight(e.Graphics, gridView, row, colWidth, x);

                for (int col = 0; col < gridView.ColumnCount; col++)
                {
                    float colX = x + col * colWidth;
                    e.Graphics.FillRectangle(PrintConfig.RowBgBrush, colX, y, colWidth, neededHeight);
                }

                for (int col = 0; col < gridView.ColumnCount; col++)
                {
                    float colX = x + col * colWidth;
                    e.Graphics.DrawRectangle(PrintConfig.RowBorderPen, colX, y, colWidth, neededHeight);
                }

                // Текст ячеек
                for (int col = 0; col < gridView.ColumnCount; col++)
                {
                    float colX = x + col * colWidth;
                    string cellText = gridView.Rows[row].Cells[col].Value?.ToString() ?? "";
                    DrawTextWithWrap(e.Graphics, cellText, colX + PrintConfig.RowCellPaddingX,
                        y + PrintConfig.RowCellPaddingY, colWidth - PrintConfig.RowCellPaddingTotal,
                        new Font("Arial", PrintConfig.RowFontSize));
                }
                y += neededHeight;
            }
        }

        private static float GetRowHeight(Graphics g, DataGridView gridView, int rowIndex, float colWidth, float startX)
        {
            float maxHeight = PrintConfig.MinRowHeight;
            Font font = new Font("Arial", PrintConfig.RowFontSize);
            float lineHeight = g.MeasureString("А", font).Height + 3;

            for (int col = 0; col < gridView.ColumnCount; col++)
            {
                string text = gridView.Rows[rowIndex].Cells[col].Value?.ToString() ?? "";
                float colHeight = CalculateTextHeight(g, text, colWidth - PrintConfig.RowCellPaddingTotal, font);
                if (colHeight > maxHeight) maxHeight = colHeight;
            }
            return Math.Max(PrintConfig.MinRowHeight, maxHeight);
        }

        private static void DrawTextWithWrap(Graphics g, string text, float x, float y, float maxWidth, Font font)
        {
            if (string.IsNullOrEmpty(text)) return;

            string[] words = text.Split(' ');
            string currentLine = "";
            float lineHeight = g.MeasureString("А", font).Height + 3;

            foreach (string word in words)
            {
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                if (g.MeasureString(testLine, font).Width > maxWidth)
                {
                    g.DrawString(currentLine, font, Brushes.Black, x, y);
                    y += lineHeight;
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }
            g.DrawString(currentLine, font, Brushes.Black, x, y);
        }

        private static float CalculateTextHeight(Graphics g, string text, float maxWidth, Font font)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            string[] words = text.Split(' ');
            string currentLine = "";
            float lineHeight = g.MeasureString("А", font).Height + 3;
            int lines = 1;

            foreach (string word in words)
            {
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                if (g.MeasureString(testLine, font).Width > maxWidth)
                {
                    lines++;
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }

            return lines * lineHeight;
        }
    }
}
