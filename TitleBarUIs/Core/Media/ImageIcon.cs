using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TitleBarUIs
{
    /// <inheritdoc />
    /// <summary>
    /// Image with vectors sources
    /// </summary>
    public class ImageIcon : Image
    {
        private static readonly Type OwnerType = typeof(ImageIcon);

        //may be .png, ,jpeg ..  or .svg file type
        #region Dependency property Icon
        public static DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Uri), OwnerType, new PropertyMetadata(null, IconPropertyChanged));

        public Uri Icon
        {
            get => (Uri)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private static void IconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ImageIcon ic)) return;

            if (ic.Source != null) return; //may be binding connect in xaml

            if (!(e.NewValue is Uri uri)) return;

            ImageSource im;
            var path = uri.IsAbsoluteUri ? uri.AbsolutePath : uri.OriginalString;
            var isSvg = Path.GetExtension(path) == ".svg";
            if (!isSvg)
            {
                im = new BitmapImage(uri);
            }
            else
            {
                var fullPath = path;

                var scheme = "pack";
                try
                {
                    scheme = uri.Scheme;
                }
                catch (Exception)
                {
                    // ignored
                }

                if (scheme == "pack")
                {
                    var componentPath = PathInfo.GetComponentPath(path);

                    var assemblePath =
                        Path.GetFullPath(Path.Combine(Path.GetFullPath(Path.GetRandomFileName()), @"..\..\..\")); //without bin/Debug

                    fullPath = Path.Combine(assemblePath, componentPath);
                }
                else
                if (scheme == "file")
                {
                    fullPath = PathInfo.GetComponentPath(path);
                }

                var group = SvgConvertor.SvgFileToWpfObject(fullPath);
                im = new DrawingImage() { Drawing = group };
            }

            ic.Source = im;

        }
        #endregion

        #region Dependency property Pen
        public static DependencyProperty PenProperty =
            DependencyProperty.Register("Pen", typeof(Pen), OwnerType, new PropertyMetadata(null, PenPropertyChanged));

        public Pen Pen
        {
            get => (Pen)GetValue(PenProperty);
            set => SetValue(PenProperty, value);
        }

        private static void PenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ImageIcon ic)) return;

            if (e.OldValue == null)
            {
                //refresh for activate PropertyChangedCallback
                var ig = ic.IconGeometry;
                ic.IconGeometry = null;
                ic.IconGeometry = ig;

                var ip = ic.IconPath;
                ic.IconPath = null;
                ic.IconPath = ip;
            }
        }
        #endregion

        #region Dependency property Fill
        public static DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), OwnerType, new PropertyMetadata(null, FillPropertyChanged));

        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        private static void FillPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ImageIcon ic)) return;

            if (e.OldValue == null)
            {
                //refresh for activate PropertyChangedCallback
                var ig = ic.IconGeometry;
                ic.IconGeometry = null;
                ic.IconGeometry = ig;

                var ip = ic.IconPath;
                ic.IconPath = null;
                ic.IconPath = ip;
            }
        }

        #endregion

        #region Dependency property IconGeometry
        public static DependencyProperty IconGeometryProperty =
            DependencyProperty.Register("IconGeometry", typeof(Geometry), OwnerType, new PropertyMetadata(null, IconGeometryPropertyChanged));

        public Geometry IconGeometry
        {
            get => (Geometry)GetValue(IconGeometryProperty);
            set => SetValue(IconGeometryProperty, value);
        }

        private static void IconGeometryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ImageIcon ic)) return;

            var path = (Geometry)e.NewValue;
            var dr = new GeometryDrawing()
            {
                Geometry = path,
                Brush = ic.Fill,
                Pen =  ic.Pen
            };
            var im = new DrawingImage() { Drawing = dr };
            ic.Source = im;
        }
        #endregion

        #region Dependency property IconPath
        public static DependencyProperty IconPathProperty =
            DependencyProperty.Register("IconPath", typeof(System.Windows.Shapes.Path), OwnerType, new PropertyMetadata(null, IconPathPropertyChanged));

        public System.Windows.Shapes.Path IconPath
        {
            get => (System.Windows.Shapes.Path)GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }

        private static void IconPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ImageIcon ic)) return;

            var path = (System.Windows.Shapes.Path)e.NewValue;
            var dr = new GeometryDrawing()
            {
                Geometry = path.Data,
                Brush = path.Fill,
                Pen = ic.Pen
            };

            var im = new DrawingImage() { Drawing = dr };
            ic.Source = im;
        }
        #endregion

        #region Dependency property AllowOutRender
        public static DependencyProperty AllowOutRenderProperty =
            DependencyProperty.Register("AllowOutRender", typeof(bool), OwnerType);

        public bool AllowOutRender
        {
            get => (bool)GetValue(AllowOutRenderProperty);
            set => SetValue(AllowOutRenderProperty, value);
        }
        #endregion

        public ImageIcon()
        {
            //default size
            Height = 16;
            Width = 16;
        }

  
    }
}
