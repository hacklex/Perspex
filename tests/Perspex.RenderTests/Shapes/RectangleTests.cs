// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Perspex.Controls;
using Perspex.Controls.Shapes;
using Perspex.Media;
using Xunit;

#if PERSPEX_CAIRO
namespace Perspex.Cairo.RenderTests.Shapes
#else
namespace Perspex.Direct2D1.RenderTests.Shapes
#endif
{
    public class RectangleTests : TestBase
    {
        public RectangleTests()
            : base(@"Shapes\Rectangle")
        {
        }

        [Fact]
        public void Rectangle_1px_Stroke()
        {
            Decorator target = new Decorator
            {
                Padding = new Thickness(8),
                Width = 200,
                Height = 200,
                Child = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                }
            };

            RenderToFile(target);
            CompareImages();
        }

        [Fact]
        public void Rectangle_2px_Stroke()
        {
            Decorator target = new Decorator
            {
                Padding = new Thickness(8),
                Width = 200,
                Height = 200,
                Child = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                }
            };

            RenderToFile(target);
            CompareImages();
        }

        [Fact]
        public void Rectangle_Stroke_Fill()
        {
            Decorator target = new Decorator
            {
                Padding = new Thickness(8),
                Width = 200,
                Height = 200,
                Child = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Fill = Brushes.Red,
                }
            };

            RenderToFile(target);
            CompareImages();
        }

        [Fact]
        public void Rectangle_Stroke_Fill_ClipToBounds()
        {
            Decorator target = new Decorator
            {
                Padding = new Thickness(8),
                Width = 200,
                Height = 200,
                Child = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Fill = Brushes.Red,
                    ClipToBounds = true,
                }
            };

            RenderToFile(target);
            CompareImages();
        }
    }
}
