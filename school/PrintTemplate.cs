using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace school
{
    public static class PrintConfig
    {
        private static ConfigData _config;

        public static float TitleX => _config?.Title.X ?? 40f;
        public static float TitleY => _config?.Title.Y ?? 80f;
        public static float TitlePageWidth => _config?.Title.PageWidth ?? 750f;
        public static float TitleOffset => _config?.Title.Offset ?? 200f;
        public static int TitleFontSize => _config?.Title.FontSize ?? 16;

        public static Brush HeaderBgBrush => GetBrush(_config?.Header.BgColor ?? "LightBlue");
        public static Pen HeaderBorderPen => GetPen(_config?.Header.BorderColor ?? "DarkBlue");
        public static int HeaderFontSize => _config?.Header.FontSize ?? 10;
        public static float HeaderHeight => _config?.Header.Height ?? 28f;
        public static float HeaderPaddingX => _config?.Header.PaddingX ?? 5f;
        public static float HeaderPaddingY => _config?.Header.PaddingY ?? 5f;
        public static float HeaderOffsetY => _config?.Header.OffsetY ?? 32f;

        public static Brush RowBgBrush => GetBrush(_config?.Row.BgColor ?? "White");
        public static Pen RowBorderPen => GetPen(_config?.Row.BorderColor ?? "Gray");
        public static int RowFontSize => _config?.Row.FontSize ?? 8;
        public static float RowCellPaddingX => _config?.Row.CellPaddingX ?? 4f;
        public static float RowCellPaddingY => _config?.Row.CellPaddingY ?? 4f;
        public static float RowCellPaddingTotal => _config?.Row.CellPaddingTotal ?? 8f;
        public static float MinRowHeight => _config?.Row.MinHeight ?? 24f;
        public static float MaxPageY => _config?.Page.MaxY ?? 1050f;

        public static sbyte MinColWidth { get; internal set; }

        static PrintConfig()
        {
            LoadConfig();
        }

        public static void LoadConfig()
        {
            try
            {
                string json = File.ReadAllText("print-config.json");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _config = JsonSerializer.Deserialize<ConfigData>(json, options);
                Console.WriteLine("✅ PrintConfig загружен из JSON");
            }
            catch
            {
                _config = new ConfigData(); // дефолтные значения
                Console.WriteLine("⚠️ PrintConfig не найден, используются дефолтные");
            }
        }

        private static Brush GetBrush(string colorName)
        {
            switch (colorName?.ToLower())
            {
                case "lightblue": return Brushes.LightBlue;
                case "lightgreen": return Brushes.LightGreen;
                case "white": return Brushes.White;
                case "lightgray": return Brushes.LightGray;
                default: return Brushes.White;
            }
        }

        private static Pen GetPen(string colorName)
        {
            switch (colorName?.ToLower())
            {
                case "darkblue": return Pens.DarkBlue;
                case "gray": return Pens.Gray;
                case "black": return Pens.Black;
                default: return Pens.Gray;
            }
        }
    }

    class ConfigData
    {
        public TitleData Title { get; set; } = new TitleData();
        public HeaderData Header { get; set; } = new HeaderData();
        public RowData Row { get; set; } = new RowData();
        public PageData Page { get; set; } = new PageData();
    }

    class TitleData { public float X { get; set; } = 40; public float Y { get; set; } = 80; public float PageWidth { get; set; } = 750; public float Offset { get; set; } = 200; public int FontSize { get; set; } = 16; }
    class HeaderData { public string BgColor { get; set; } = "LightBlue"; public string BorderColor { get; set; } = "DarkBlue"; public int FontSize { get; set; } = 10; public float Height { get; set; } = 28; public float PaddingX { get; set; } = 5; public float PaddingY { get; set; } = 5; public float OffsetY { get; set; } = 32; }
    class RowData { public string BgColor { get; set; } = "White"; public string BorderColor { get; set; } = "Gray"; public int FontSize { get; set; } = 8; public float CellPaddingX { get; set; } = 4; public float CellPaddingY { get; set; } = 4; public float CellPaddingTotal { get; set; } = 8; public float MinHeight { get; set; } = 24; }
    class PageData { public float MaxY { get; set; } = 1050; }
}