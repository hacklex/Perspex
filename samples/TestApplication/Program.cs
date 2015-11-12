﻿// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Reactive.Linq;
using Perspex;
using Perspex.Animation;
using Perspex.Collections;
using Perspex.Controls;
using Perspex.Controls.Html;
using Perspex.Controls.Primitives;
using Perspex.Controls.Shapes;
using Perspex.Controls.Templates;
using Perspex.Diagnostics;
using Perspex.Layout;
using Perspex.Media;
using Perspex.Media.Imaging;
using Perspex.Threading;
#if PERSPEX_GTK
using Perspex.Gtk;
#endif
using ReactiveUI;

namespace TestApplication
{
    internal static class Program
    {

        public static double WrapAngle(this double angle, double cap)
        {
            while (angle < 0) angle += cap;
            while (angle > cap) angle -= cap;
            return angle;
        }

        private static readonly PerspexList<Node> s_treeData = new PerspexList<Node>
        {
            new Node
            {
                Name = "Root 1",
                Children = new PerspexList<Node>
                {
                    new Node
                    {
                        Name = "Child 1",
                    },
                    new Node
                    {
                        Name = "Child 2",
                        Children = new PerspexList<Node>
                        {
                            new Node
                            {
                                Name = "Grandchild 1",
                            },
                            new Node
                            {
                                Name = "Grandmaster Flash",
                            },
                        }
                    },
                    new Node
                    {
                        Name = "Child 3",
                    },
                }
            },
            new Node
            {
                Name = "Root 2",
            },
        };

        private static readonly PerspexList<Item> s_listBoxData = new PerspexList<Item>
        {
            new Item { Name = "Item 1", Value = "Item 1 Value" },
            new Item { Name = "Item 2", Value = "Item 2 Value" },
            new Item { Name = "Item 3", Value = "Item 3 Value" },
            new Item { Name = "Item 4", Value = "Item 4 Value" },
            new Item { Name = "Item 5", Value = "Item 5 Value" },
            new Item { Name = "Item 6", Value = "Item 6 Value" },
            new Item { Name = "Item 7", Value = "Item 7 Value" },
            new Item { Name = "Item 8", Value = "Item 8 Value" },
        };


        class ArcWindow : Window
        {
            private double _lambda1;
            private double _lambda2;
            private bool _swapSegments;
            private double _ellipseAngle = Math.PI / 2;
            private double _ellipseHorizontalRadius = 100;
            private double _ellipseVerticalRadius = 50;

            private Stopwatch _st = Stopwatch.StartNew();
            private TimeSpan _lastFps;
            private int _frames;

            public ArcWindow()
            {
                Title = "Bezier-Approximated Arcs Demo";
                var dt = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(10),
                    IsEnabled = true,
                };
                Background = Brushes.Transparent;
                dt.Tick += delegate
                {
                    _lambda1 = (_lambda1 + 0.05).WrapAngle(Math.PI * 2);
                    _lambda2 = (_lambda2 + 0.03).WrapAngle(Math.PI * 2);
                    if (Math.Abs(_lambda1 - _lambda2) < 0.013)
                    {
                        _swapSegments = !_swapSegments;
                        _lambda2 = _lambda1;
                        _lambda1 += 0.05;
                    }
                    _ellipseAngle += 1;

                    var lambda1 = _swapSegments ? _lambda2 : _lambda1;
                    var lambda2 = _swapSegments ? _lambda1 : _lambda2;
;
                    InvalidateVisual();
                };
                dt.Start();

                Show();
            }

            public override void Render(DrawingContext drawingContext)
            {
                var now = _st.Elapsed;
                var diff = now - _lastFps;
                if (diff.Seconds >= 1)
                {
                    _lastFps = now;
                    Title = "FPS: " + _frames;
                    _frames = 0;
                }

                _frames++;

                drawingContext.FillRectangle(Brushes.LightGray, new Rect(0, 0, ClientSize.Width, ClientSize.Height));

                //_arcHelper = new EllipticalArc(0, 0, _ellipseHorizontalRadius, _ellipseVerticalRadius, _ellipseAngle, lambda1, lambda2, false);

                System.Collections.Generic.List<StreamGeometry> toDraw = new System.Collections.Generic.List<StreamGeometry>();

                var sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(180, 150), false);
                    sgc.ArcTo(new Point(240, 150), new Size(30, 30), 0.0, false, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);

                sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(360, 150), false);
                    sgc.ArcTo(new Point(300, 150), new Size(30, 30), 0.0, false, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);

                sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(210, 210), false);
                    sgc.ArcTo(new Point(210, 270), new Size(30, 30), 0.0, false, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);

                sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(330, 270), false);
                    sgc.ArcTo(new Point(330, 210), new Size(30, 30), 0.0, false, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);

                sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(480, 270), false);
                    sgc.ArcTo(new Point(585, 360), new Size(105, 90), 0.0, true, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);

                sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(180, 330), false);
                    sgc.ArcTo(new Point(210, 300), new Size(30, 30), 0.0, false, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);

                sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(240, 300), false);
                    sgc.ArcTo(new Point(270, 330), new Size(30, 30), 0.0, false, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);

                sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(210, 390), false);
                    sgc.ArcTo(new Point(180, 360), new Size(30, 30), 0.0, false, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);

                sg = new StreamGeometry();
                using (var sgc = sg.Open())
                {
                    sgc.BeginFigure(new Point(270, 360), false);
                    sgc.ArcTo(new Point(240, 390), new Size(30, 30), 0.0, false, SweepDirection.Clockwise);
                    sgc.EndFigure(false);
                }
                toDraw.Add(sg);
                // using (drawingContext.PushPostTransform(Matrix.CreateTranslation(ClientSize.Width / 2, ClientSize.Height / 2)))
                foreach(var x in toDraw)
                    drawingContext.DrawGeometry(Brushes.Wheat, new Pen(Brushes.Black), x);
            }
        }

        private static void Main(string[] args)
        {
            // The version of ReactiveUI currently included is for WPF and so expects a WPF
            // dispatcher. This makes sure it's initialized.
            System.Windows.Threading.Dispatcher foo = System.Windows.Threading.Dispatcher.CurrentDispatcher;

            new App
            {
                DataTemplates = new DataTemplates
                {
                    new FuncTreeDataTemplate<Node>(
                        x => new TextBlock { Text = x.Name },
                        x => x.Children,
                        x => true),
                },
            };

            TabControl container;


            Application.Current.Run(new ArcWindow());
            return;

            Window window = new Window
            {
                Title = "Perspex Test Application",
                Width = 900,
                Height = 480,
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions
                    {
                        new ColumnDefinition(1, GridUnitType.Star),
                        new ColumnDefinition(1, GridUnitType.Star),
                    },
                    RowDefinitions = new RowDefinitions
                    {
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(1, GridUnitType.Star),
                        new RowDefinition(GridLength.Auto),
                    },
                    Children = new Controls
                    {
                        (container = new TabControl
                        {
                            Padding = new Thickness(5),
                            Items = new[]
                            {
                                ButtonsTab(),
                                TextTab(),
                                HtmlTab(),
								ImagesTab(),
                                ListsTab(),
                                LayoutTab(),
                                AnimationsTab(),
                            },
                            Transition = new CrossFade(TimeSpan.FromSeconds(0.25)),
                            [Grid.RowProperty] = 1,
                            [Grid.ColumnSpanProperty] = 2,
                        })
                    }
                },
            };

            container.Classes.Add(":container");

            DevTools.Attach(window);
            window.Show();
            Application.Current.Run(window);
        }

        private static TabItem ButtonsTab()
        {
            var result = new TabItem
            {
                Header = "Button",
				Content = new ScrollViewer()
				{
					CanScrollHorizontally = false,
					Content = new StackPanel
					{
						Margin = new Thickness(10),
						Orientation = Orientation.Vertical,
						Gap = 4,
						Children = new Controls
						{
							new TextBlock
							{
								Text = "Button",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A button control",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new Button
							{
								Width = 150,
								Content = "Button"
							},
							new Button
							{
								Width   = 150,
								Content = "Disabled",
								IsEnabled = false,
							},
							new TextBlock
							{
								Margin = new Thickness(0, 40, 0, 0),
								Text = "ToggleButton",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A toggle button control",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new ToggleButton 
							{ 
								Width = 150,
								IsChecked = true,
								Content = "On" 
							},
							new ToggleButton 
							{ 
								Width = 150,
								IsChecked = false, 
								Content = "Off" 
							},
						}
					}
                },
            };
            

            return result;
        }

        private static TabItem HtmlTab()
        {
            return new TabItem
            {
                Header = "Text",
				Content = new ScrollViewer() 
				{
					CanScrollHorizontally = false,
					Content = new StackPanel()
					{                   
						Margin = new Thickness(10),
						Orientation = Orientation.Vertical,
						Gap = 4,
						Children = new Controls 
						{
							new TextBlock
							{
								Text = "TextBlock",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A control for displaying text.",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new TextBlock
							{
								Text = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
								FontSize = 11
							},
							new TextBlock
							{
								Text = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
								FontSize = 11,
								FontWeight = FontWeight.Medium
							},
							new TextBlock
							{
								Text = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
								FontSize = 11,
								FontWeight = FontWeight.Bold
							},
							new TextBlock
							{
								Text = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
								FontSize = 11,
								FontStyle = FontStyle.Italic,
							},
							new TextBlock
							{
								Margin = new Thickness(0, 40, 0, 0),
								Text = "HtmlLabel",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A label capable of displaying HTML content",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new HtmlLabel 
							{ 
								Background = SolidColorBrush.Parse("#CCCCCC"),
								Padding = new Thickness(5),
								Text = @"<p><strong>Pellentesque habitant morbi tristique</strong> senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam, feugiat vitae, ultricies eget, tempor sit amet, ante. Donec eu libero sit amet quam egestas semper. <em>Aenean ultricies mi vitae est.</em> Mauris placerat eleifend leo. Quisque sit amet est et sapien ullamcorper pharetra. Vestibulum erat wisi, condimentum sed, <code>commodo vitae</code>, ornare sit amet, wisi. Aenean fermentum, elit eget tincidunt condimentum, eros ipsum rutrum orci, sagittis tempus lacus enim ac dui. <a href=""#"">Donec non enim</a> in turpis pulvinar facilisis. Ut felis.</p>
										<h2>Header Level 2</h2>
											       
										<ol>
										   <li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li>
										   <li>Aliquam tincidunt mauris eu risus.</li>
										</ol>

										<blockquote><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus magna. Cras in mi at felis aliquet congue. Ut a est eget ligula molestie gravida. Curabitur massa. Donec eleifend, libero at sagittis mollis, tellus est malesuada tellus, at luctus turpis elit sit amet quam. Vivamus pretium ornare est.</p></blockquote>

										<h3>Header Level 3</h3>

										<ul>
										   <li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li>
										   <li>Aliquam tincidunt mauris eu risus.</li>
										</ul>" 				
							}
						}
              		}
				}
            };
        }

        private static TabItem TextTab()
        {
            return new TabItem
            {
                Header = "Input",
				Content = new ScrollViewer()
				{
					Content = new StackPanel
					{
						Margin = new Thickness(10),
						Orientation = Orientation.Vertical,
						Gap = 4,
						Children = new Controls
						{
							new TextBlock
							{
								Text = "TextBox",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A text box control",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},

							new TextBox { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", Width = 200},
                            new TextBox { Width = 200, Watermark="Watermark"},
                            new TextBox { Width = 200, Watermark="Floating Watermark", UseFloatingWatermark = true },
                            new TextBox { AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, Width = 200, Height = 150, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus magna. Cras in mi at felis aliquet congue. Ut a est eget ligula molestie gravida. Curabitur massa. Donec eleifend, libero at sagittis mollis, tellus est malesuada tellus, at luctus turpis elit sit amet quam. Vivamus pretium ornare est." },
							new TextBlock
							{
								Margin = new Thickness(0, 40, 0, 0),
								Text = "CheckBox",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A check box control",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new CheckBox { IsChecked = true, Margin = new Thickness(0, 0, 0, 5), Content = "Checked" },
							new CheckBox { IsChecked = false, Content = "Unchecked" },
							new TextBlock
							{
								Margin = new Thickness(0, 40, 0, 0),
								Text = "RadioButton",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A radio button control",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new RadioButton { IsChecked = true, Content = "Option 1" },
							new RadioButton { IsChecked = false, Content = "Option 2" },
							new RadioButton { IsChecked = false, Content = "Option 3" },
						}
					}
				}
            };
        }

        
        private static TabItem ListsTab()
        {
            return new TabItem
            {
                Header = "Lists",
				Content = new ScrollViewer()
				{
					CanScrollHorizontally = false,
					Content = new StackPanel
					{
						HorizontalAlignment = HorizontalAlignment.Left,
						Orientation = Orientation.Vertical,
						VerticalAlignment = VerticalAlignment.Top,
						Gap = 4,
						Margin = new Thickness(10),
						DataTemplates = new DataTemplates
						{
							new FuncDataTemplate<Item>(x =>
								new StackPanel
								{
									Gap = 4,
									Orientation = Orientation.Horizontal,
									Children = new Controls
									{
										new Image { Width = 50, Height = 50, Source = new Bitmap("github_icon.png") },
										new TextBlock { Text = x.Name, FontSize = 18 }
									}
								})
						},
						Children = new Controls
						{
							new TextBlock
							{
								Text = "ListBox",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A list box control.",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new ListBox
							{
								BorderThickness = 2,
								Items = s_listBoxData,						
								Height = 300,
								Width =	 300,
							},
							new TextBlock
							{
								Margin = new Thickness(0, 40, 0, 0),
								Text = "TreeView",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A tree view control.",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new TreeView
							{
								Name = "treeView",
								Items = s_treeData,							
								Height = 300,
								BorderThickness = 2,
								Width =	 300,
							}
						}
					},
				}
            };
        }

		private static TabItem ImagesTab() 
		{
			var imageCarousel = new Carousel 
			{
				Width = 400,
				Height = 400,
				Transition = new PageSlide(TimeSpan.FromSeconds(0.25)),
				Items = new[] 
				{
					new Image { Source = new Bitmap("github_icon.png"),  Width = 400, Height = 400 },
					new Image { Source = new Bitmap("pattern.jpg"), Width = 400, Height = 400 },
				}
			};

			var next = new Button 
			{
				VerticalAlignment = VerticalAlignment.Center,
				Padding = new Thickness(20),
				Content = new Perspex.Controls.Shapes.Path 
				{
					Data = StreamGeometry.Parse("M4,11V13H16L10.5,18.5L11.92,19.92L19.84,12L11.92,4.08L10.5,5.5L16,11H4Z"),
					Fill = Brushes.Black
				}
			};

			var prev = new Button 
			{
				VerticalAlignment = VerticalAlignment.Center,
				Padding = new Thickness(20),
				Content = new Perspex.Controls.Shapes.Path 
				{
					Data = StreamGeometry.Parse("M20,11V13H8L13.5,18.5L12.08,19.92L4.16,12L12.08,4.08L13.5,5.5L8,11H20Z"),
					Fill = Brushes.Black
				}
			};

			prev.Click += (s, e) => 
			{
				if (imageCarousel.SelectedIndex == 0)
					imageCarousel.SelectedIndex = 1;
				else
					imageCarousel.SelectedIndex--;
			};

			next.Click += (s, e) => 
			{
				if (imageCarousel.SelectedIndex == 1)
					imageCarousel.SelectedIndex = 0;
				else
					imageCarousel.SelectedIndex++;
			};

			return new TabItem
			{
				Header = "Images",
				Content = new ScrollViewer 
				{
					Content = new StackPanel 
					{
						HorizontalAlignment = HorizontalAlignment.Left,
						Orientation = Orientation.Vertical,
						VerticalAlignment = VerticalAlignment.Top,
						Gap = 4,
						Margin = new Thickness(10),
						Children = new Controls 
						{
							new TextBlock
							{
								Text = "Carousel",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "An items control that displays its items as pages that fill the controls.",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new StackPanel 
							{
								Name = "carouselVisual",
								Orientation = Orientation.Horizontal,
								Gap = 4,
								Children = new Controls
								{								
									prev,
									imageCarousel,
									next
								}
							}
						}
					}
				}
			};
		}

        private static TabItem LayoutTab()
        {
            return new TabItem
            {
                Header = "Layout",
				Content = new ScrollViewer 
				{
					Content = new StackPanel 
					{
						HorizontalAlignment = HorizontalAlignment.Left,
						Orientation = Orientation.Vertical,
						VerticalAlignment = VerticalAlignment.Top,
						Gap = 4,
						Margin = new Thickness(10),
						Children = new Controls 
						{
							new TextBlock
							{
								Text = "Grid",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "Lays out child controls according to a grid.",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new Grid
							{
								Width = 600,
								ColumnDefinitions = new ColumnDefinitions
								{
									new ColumnDefinition(1, GridUnitType.Star),
									new ColumnDefinition(1, GridUnitType.Star),
								},

								RowDefinitions = new RowDefinitions
								{
									new RowDefinition(1, GridUnitType.Auto),
									new RowDefinition(1, GridUnitType.Auto)
								},
								Children = new Controls
								{

									new Rectangle
									{
										Fill = SolidColorBrush.Parse("#FF5722"),
										[Grid.ColumnSpanProperty] = 2,
										Height = 200,
										Margin = new Thickness(2.5)
									},
									new Rectangle
									{
										Fill = SolidColorBrush.Parse("#FF5722"),
										[Grid.RowProperty] = 1,
										Height = 100,
										Margin = new Thickness(2.5)
									},
									new Rectangle
									{
										Fill = SolidColorBrush.Parse("#FF5722"),
										[Grid.RowProperty] = 1,
										[Grid.ColumnProperty] = 1,
										Height = 100,
										Margin = new Thickness(2.5)
									},
								},
							},
							new TextBlock
							{
								Margin = new Thickness(0, 40, 0, 0),
								Text = "StackPanel",
								FontWeight = FontWeight.Medium,
								FontSize = 20,
								Foreground = SolidColorBrush.Parse("#212121"),
							},
							new TextBlock
							{
								Text = "A panel which lays out its children horizontally or vertically.",
								FontSize = 13,
								Foreground = SolidColorBrush.Parse("#727272"),
								Margin = new Thickness(0, 0, 0, 10)
							},
							new StackPanel 
							{
								Orientation = Orientation.Vertical,
								Gap = 4,
								Width = 300,
								Children = new Controls
								{
									new Rectangle
									{
										Fill = SolidColorBrush.Parse("#FFC107"),
										Height = 50,
									},
									new Rectangle
									{
										Fill = SolidColorBrush.Parse("#FFC107"),
										Height = 50,
									},
									new Rectangle
									{
										Fill = SolidColorBrush.Parse("#FFC107"),
										Height = 50,
									},
								}
							},
                            new TextBlock
                            {
                                Margin = new Thickness(0, 40, 0, 0),
                                Text = "Canvas",
                                FontWeight = FontWeight.Medium,
                                FontSize = 20,
                                Foreground = SolidColorBrush.Parse("#212121"),
                            },
                            new TextBlock
                            {
                                Text = "A panel which lays out its children by explicit coordinates.",
                                FontSize = 13,
                                Foreground = SolidColorBrush.Parse("#727272"),
                                Margin = new Thickness(0, 0, 0, 10)
                            },
                            new Canvas
                            {
                                Background = Brushes.Yellow,
                                Width = 300,
                                Height = 400,
                                Children = new Controls
                                {
                                    new Rectangle
                                    {
                                        Fill = Brushes.Blue,
                                        Width = 63,
                                        Height = 41,
                                        [Canvas.LeftProperty] = 40,
                                        [Canvas.TopProperty] = 31,
                                    },
                                    new Ellipse
                                    {
                                        Fill = Brushes.Green,
                                        Width = 58,
                                        Height = 58,
                                        [Canvas.LeftProperty] = 130,
                                        [Canvas.TopProperty] = 79,
                                    },
                                }
                            },
                        }
					}
				}
            };
        }

        private static TabItem AnimationsTab()
        {
            Border border1;
            Border border2;
            RotateTransform rotate;
            Button button1;

            var result = new TabItem
            {
                Header = "Animations",
                Content = new StackPanel
                {
					Orientation = Orientation.Vertical,
					Gap = 4,
					Margin = new Thickness(10),
                    Children = new Controls
                    {
						new TextBlock
						{
							Text = "Animations",
							FontWeight = FontWeight.Medium,
							FontSize = 20,
							Foreground = SolidColorBrush.Parse("#212121"),
						},
						new TextBlock
						{
							Text = "A few animations showcased below",
							FontSize = 13,
							Foreground = SolidColorBrush.Parse("#727272"),
							Margin = new Thickness(0, 0, 0, 10)
						},
						(button1 = new Button
						{
							Content = "Animate",
							Width = 120,
							[Grid.ColumnProperty] = 1,
							[Grid.RowProperty] = 1,
						}),
						new Canvas 
						{
                            ClipToBounds = false,
							Children = new Controls 
							{
								(border1 = new Border
								{
									Width = 100,
									Height = 100,
									HorizontalAlignment = HorizontalAlignment.Center,
									VerticalAlignment = VerticalAlignment.Center,
									Background = Brushes.Crimson,
									RenderTransform = new RotateTransform(),
									Child = new TextBox
									{
										Background = Brushes.White,
										Text = "Hello!",
										HorizontalAlignment = HorizontalAlignment.Center,
										VerticalAlignment = VerticalAlignment.Center,
									},
									[Canvas.LeftProperty] = 100,
									[Canvas.TopProperty] = 100,
								}),
								(border2 = new Border
								{
									Width = 100,
									Height = 100,
									HorizontalAlignment = HorizontalAlignment.Center,
									VerticalAlignment = VerticalAlignment.Center,
									Background = Brushes.Coral,
									Child = new Image
									{
										Source = new Bitmap("github_icon.png"),
										HorizontalAlignment = HorizontalAlignment.Center,
										VerticalAlignment = VerticalAlignment.Center,
									},
									RenderTransform = (rotate = new RotateTransform
									{
										PropertyTransitions = new PropertyTransitions
										{
											RotateTransform.AngleProperty.Transition(500),
										}
									}),
									PropertyTransitions = new PropertyTransitions
									{
										Layoutable.WidthProperty.Transition(300),
										Layoutable.HeightProperty.Transition(1000),
									},
									[Canvas.LeftProperty] = 400,
									[Canvas.TopProperty] = 100,
								}),
							}
						}
                    },
                },
            };

            button1.Click += (s, e) =>
            {
                if (border2.Width == 100)
                {
                    border2.Width = border2.Height = 400;
                    rotate.Angle = 180;
                }
                else
                {
                    border2.Width = border2.Height = 100;
                    rotate.Angle = 0;
                }
            };

            var start = Animate.Stopwatch.Elapsed;
            var degrees = Animate.Timer
                .Select(x =>
                {
                    var elapsed = (x - start).TotalSeconds;
                    var cycles = elapsed / 4;
                    var progress = cycles % 1;
                    return 360.0 * progress;
                });

            border1.RenderTransform.Bind(
                RotateTransform.AngleProperty,
                degrees,
                BindingPriority.Animation);

            return result;
        }
    }
}
