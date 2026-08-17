//using System.Windows;
//using Bodoconsult.App.Wpf.Delegates;
//using Bodoconsult.App.Wpf.Services;

//namespace Bodoconsult.App.Wpf.Documents.Paginators
//{
//    /// <summary>
//    /// Print definitions regarding paper size, margins, headers and footers
//    /// </summary>
//    public class PrintDefinition
//    {
//        /// <summary>
//        /// Default constructor
//        /// </summary>
//        public PrintDefinition()
//        {
//            RepeatTableHeaders = true;
//            Margins = new Thickness(92.7, 55, 55, 55);
//            PageSize = new Size(793.5987, 1122.3987);
//            MaxImageHeight = 300;
//            HeaderHeight = 45;
//            FooterHeight = 25;
//            HeaderMarginBottom = 20;
//            DefaultStyleName = "Standard";
//            FigureCounterPrefix = "Figure";
//            AutoNumbering = true;
//            ShowFigureCounter = true;

//            ImageTemplate = "<Figure CanDelayPlacement=\"false\" HorizontalAnchor=\"ColumnCenter\"><BlockUIContainer><Image Source=\"{0}\" MaxHeight=\"{2}\" MaxWidth=\"{3}\"/></BlockUIContainer>{1}</Figure>";
//            CurrentLanguage = "de";

//            HeaderFontName = "Aptos";
//            HeaderFontSize = 10.0;
//            FooterFontName = "Aptos";
//            FooterFontSize = 10.0;
//            LogoWidth = 113;
//        }

//        #region Page settings

//        /// <summary>
//        /// Page size in DIUs
//        /// </summary>
//        public Size PageSize { get; set; }

//        /// <summary>
//        /// Page margins
//        /// </summary>
//        public Thickness Margins { get; set; }

//        /// <summary>
//        /// Space reserved for the header in DIUs
//        /// </summary>
//        public double HeaderHeight { get; set; }

//        /// <summary>
//        /// Bottom margin of the header in DIUs
//        /// </summary>
//        public double HeaderMarginBottom { get; set; }

//        /// <summary>
//        /// Space reserved for the footer in DIUs
//        /// </summary>
//        public double FooterHeight { get; set; }

//        /// <summary>
//        /// Margin in footer above the footer text and below the main text
//        /// </summary>
//        public double FooterMarginTop { get; set; }

//        #endregion

//        #region Formatting properties

//        /// <summary>
//        /// Text to be printed in the page header
//        /// </summary>
//        public string HeaderText { get; set; }

//        /// <summary>
//        /// Font name to use for header
//        /// </summary>
//        public string HeaderFontName { get; set; }

//        /// <summary>
//        /// Header font size
//        /// </summary>
//        public double HeaderFontSize { get; set; }

//        /// <summary>
//        /// Text to be printed in the page footer
//        /// </summary>
//        public string FooterText { get; set; }

//        /// <summary>
//        /// Font name to use for footer
//        /// </summary>
//        public string FooterFontName { get; set; }

//        /// <summary>
//        /// Footer font size
//        /// </summary>
//        public double FooterFontSize { get; set; }

//        /// <summary>
//        /// Absolute or relative path to the logo to print in the page header
//        /// </summary>
//        public string LogoPath { get; set; }

//        /// <summary>
//        /// Width of the logo to print in the page header
//        /// </summary>
//        public double LogoWidth { get; set; }

//        /// <summary>
//        /// Maximum height for images in the document
//        /// </summary>
//        public double MaxImageHeight { get; set; }

//        /// <summary>
//        /// Maximum width for images in the document
//        /// </summary>
//        public double MaxImageWidth { get; set; }

//        /// <summary>
//        /// Language code like en or de (only first 2 letters needed). Default: de
//        /// Needs a <see cref="CurrentLanguageModule"/> to be defined too to work properly
//        /// </summary>
//        public string CurrentLanguage { get; set; }

//        /// <summary>
//        /// The name used to register the current used language resources. See <see cref="LanguageResourceService"/>.
//        /// Needs a <see cref="CurrentLanguage"/> to be defined too to work properly
//        /// </summary>
//        public string CurrentLanguageModule { get; set; }

//        /// <summary>
//        /// Prefix used for the ongoing numbering of figures in the report
//        /// </summary>
//        public string FigureCounterPrefix { get; set; }

//        /// <summary>
//        /// Name of the default style
//        /// </summary>
//        public string DefaultStyleName { get; set; }

//        /// <summary>
//        /// Set an individual XAML resource file for formatting the document.
//        /// Resource file must follow the rules of Typography.XAML in Bodoconsult.Wpf.Documents
//        /// </summary>
//        public string UserDefinedTypographyFile { get; set; }

//        /// <summary>
//        /// Show an ongoing numbering in the legend of each figure
//        /// </summary>
//        public bool ShowFigureCounter { get; set; }

//        /// <summary>
//        /// Automatically count the headlines per level and add numbers to headline printed
//        /// </summary>
//        public bool AutoNumbering { get; set; }

//        /// <summary>
//        /// Template used to resolve images in textblocks
//        /// </summary>
//        public string ImageTemplate { get; set; }

//        ///<summary>
//        /// Repeat table headers? Default: false
//        ///</summary>
//        public bool RepeatTableHeaders { get; set; }

//        #endregion

//        #region Delegates for drawing main page sections

//        /// <summary>
//        /// Delegate to print a header to the document page
//        /// </summary>
//        public DrawSectionDelegate DrawHeaderDelegate;

//        /// <summary>
//        /// Delegate to print a footer to the document page
//        /// </summary>
//        public DrawSectionDelegate DrawFooterDelegate;   

//        #endregion
        

//        #region Important measures for page sections calculate from page settings

//        /// <summary>
//        /// Current content size
//        /// </summary>
//        public Size ContentSize
//        {
//            get
//            {
//                var size = new Size(PageSize.Width - Margins.Left - Margins.Right,
//                    PageSize.Height - (Margins.Top + Margins.Bottom + HeaderHeight + FooterHeight));

//                return size;
//            }
//        }

//        /// <summary>
//        /// The origin of the content
//        /// </summary>
//        public Point ContentOrigin =>
//            new(
//                Margins.Left,
//                Margins.Top + HeaderRect.Height
//            );

//        /// <summary>
//        /// The defined header area
//        /// </summary>
//        public Rect HeaderRect =>
//            new(
//                Margins.Left, Margins.Top,
//                ContentSize.Width, HeaderHeight
//            );

//        /// <summary>
//        /// The defined footer area
//        /// </summary>
//        public Rect FooterRect =>
//            new(
//                Margins.Left, ContentOrigin.Y + ContentSize.Height,
//                ContentSize.Width, FooterHeight
//            );

//        #endregion

//    }
//}