// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Windows;
using System.Windows.Media;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using Bodoconsult.App.Wpf.Helpers;
using PropertyChanged;
using FontFamily = System.Windows.Media.FontFamily;

namespace Bodoconsult.App.Wpf.Documents.Services;

/// <summary>
/// Set the typography settings for <see cref="FlowDocumentService"/> used in Typography.xaml
/// </summary>
[AddINotifyPropertyChangedInterface]
public class TypographySettingsService: BasePageSettingsService
{
    private double _tableBorderWidth;
    private Thickness _tableBorderThickness;

    /// <summary>
    /// Default ctor
    /// </summary>
    public TypographySettingsService()
    {
        Typography = new CompactTypographyPageHeader("Calibri", "Calibri", "Calibri");
        LoadTypography();
    }

    /// <summary>
    /// Ctor to load settings from a typography
    /// </summary>
    /// <param name="typography">Current typography instance to load</param>
    public TypographySettingsService(ITypography typography)
    {
        Typography = typography;
        LoadTypography();
    }

    /// <summary>
    /// Current typography
    /// </summary>
    public ITypography Typography { get; private set; }


    #region Formatting properties

    /// <summary>
    /// Maximum height for images in the document
    /// </summary>
    public double MaxImageHeight { get; set; }

    /// <summary>
    /// Maximum width for images in the document
    /// </summary>
    public double MaxImageWidth { get; set; }



    ///// <summary>
    ///// The name used to register the current used language resources. See <see cref="LanguageResourceService"/>.
    ///// Needs a <see cref="CurrentLanguage"/> to be defined too to work properly
    ///// </summary>
    //public string CurrentLanguageModule { get; set; }

    /// <summary>
    /// Prefix used for the ongoing numbering of figures in the report
    /// </summary>
    public string FigureCounterPrefix { get; set; }

    /// <summary>
    /// Name of the default style
    /// </summary>
    public string DefaultStyleName { get; set; }

    /// <summary>
    /// Set an individual XAML resource file for formatting the document.
    /// Resource file must follow the rules of Typography.XAML in Bodoconsult.Wpf.Documents
    /// </summary>
    public string UserDefinedTypographyFile { get; set; }

    /// <summary>
    /// Show an ongoing numbering in the legend of each figure
    /// </summary>
    public bool ShowFigureCounter { get; set; }

    /// <summary>
    /// Automatically count the headlines per level and add numbers to headline printed
    /// </summary>
    public bool AutoNumbering { get; set; }

    /// <summary>
    /// Template used to resolve images in textblocks
    /// </summary>
    public string ImageTemplate { get; set; } = "<Figure CanDelayPlacement=\"false\" HorizontalAnchor=\"ColumnCenter\"><BlockUIContainer><Image Source=\"{0}\" MaxHeight=\"{2}\" MaxWidth=\"{3}\"/></BlockUIContainer>{1}</Figure>";

    #endregion



    #region Text formatting settings





    ///// <summary>
    ///// Name of primary font
    ///// </summary>
    //public string PrimaryFontName { get; set; }

    /// <summary>
    /// Font family of the primary font. FontName in <see cref="Typography"/> must be set
    /// </summary>
    public FontFamily PrimaryFontFamily
    {
        get
        {
            FontFamily f = null;

            Application.Current.Dispatcher.Invoke(() => f = new FontFamily(Typography.FontName));

            return f;
        }
    }

    /// <summary>
    /// Font family of the primary font. HeadingFontName in <see cref="Typography"/> must be set
    /// </summary>
    public FontFamily SecondaryFontFamily
    {
        get
        {
            FontFamily f = null;
            Application.Current.Dispatcher.Invoke(() => f = new FontFamily(Typography.HeadingFontName));
            return f;
        }
    }

    ///// <summary>
    ///// Name of third font. FontName in <see cref="Typography"/> must be set
    ///// </summary>
    //public string ThirdFontName { get; set; }

    /// <summary>
    /// Font family of the third font. TitleFontName in <see cref="Typography"/> must be set
    /// </summary>
    public FontFamily ThirdFontFamily
    {
        get
        {
            FontFamily f = null;
            Application.Current.Dispatcher.Invoke(() => f = new FontFamily(Typography.TitleFontName));
            return f;
        }
    }




    /// <summary>
    /// Regular font size in DIU
    /// </summary>
    public double RegularFontSize { get; private set; }

    /// <summary>
    /// Line height of regular text in DIU
    /// </summary>
    public double RegularLineHeight { get; private set; }


    /// <summary>
    /// Font size for small text in DIU
    /// </summary>
    public double SmallFontSize { get; private set; }


    /// <summary>
    /// Font size for tiny text in DIU
    /// </summary>
    public double ExtraSmallFontSize { get; private set; }


    /// <summary>
    /// Font size for heading level 1
    /// </summary>
    public double Heading1FontSize { get; private set; }

    /// <summary>
    /// Font size for heading level 2
    /// </summary>
    public double Heading2FontSize { get; private set; }

    /// <summary>
    /// Font size for heading level 3
    /// </summary>
    public double Heading3FontSize { get; private set; }

    /// <summary>
    /// Font size for heading level 4
    /// </summary>
    public double Heading4FontSize { get; private set; }

    /// <summary>
    /// Font size for heading level 5
    /// </summary>
    public double Heading5FontSize { get; private set; }

    /// <summary>
    /// Font size for title level 1
    /// </summary>
    public double TitleFontSize { get; private set; }

    /// <summary>
    /// Font size for title level 2
    /// </summary>
    public double Title2FontSize { get; private set; }



    /// <summary>
    /// Regular margins for text
    /// </summary>
    public Thickness RegularThickness { get; set; }


    /// <summary>
    /// Regular margins for table content
    /// </summary>
    public Thickness TableContentThickness { get; set; }

    /// <summary>
    /// Small margins for table content
    /// </summary>
    public Thickness TableContentSmallThickness { get; set; }

    /// <summary>
    /// Tiny margins for table content
    /// </summary>
    public Thickness TableContentExtraSmallThickness { get; set; }

    /// <summary>
    /// Regular margins for table content
    /// </summary>
    public Thickness TableContentUnborderedThickness { get; set; }

    /// <summary>
    /// Small margins for table content
    /// </summary>
    public Thickness TableContentSmallUnborderedThickness { get; set; }

    /// <summary>
    /// Tiny margins for table content
    /// </summary>
    public Thickness TableContentExtraSmallUnborderedThickness { get; set; }


    /// <summary>
    /// Margins for heading level 5
    /// </summary>
    public Thickness Heading5Thickness { get; set; }
    /// <summary>
    /// Margins for heading level 4
    /// </summary>
    public Thickness Heading4Thickness { get; set; }
    /// <summary>
    /// Margins for heading level 3
    /// </summary>
    public Thickness Heading3Thickness { get; set; }
    /// <summary>
    /// Margins for heading level 2
    /// </summary>
    public Thickness Heading2Thickness { get; set; }
    /// <summary>
    /// Margins for heading level 1
    /// </summary>
    public Thickness Heading1Thickness { get; set; }

    /// <summary>
    /// Margins for title level 1
    /// </summary>
    public Thickness TitleThickness { get; set; }


    /// <summary>
    /// Margins for title level 2
    /// </summary>
    public Thickness Title2Thickness { get; set; }

    /// <summary>
    /// Margin around figures
    /// </summary>
    public Thickness FigureThickness { get; set; }

    /// <summary>
    /// Color for table header background
    /// </summary>
    public Brush TableHeaderBackground { get; set; }


    /// <summary>
    /// Color for table body background
    /// </summary>
    public Brush TableBodyBackground { get; set; }

    /// <summary>
    /// Color for table body border
    /// </summary>
    public Brush TableBorder { get; set; }

    /// <summary>
    /// Color for table header background
    /// </summary>
    public Brush TableHeaderUnborderedBackground { get; set; }

    /// <summary>
    /// Color for table body background
    /// </summary>
    public Brush TableBodyUnborderedBackground { get; set; }


    /// <summary>
    /// Corner radius of a table
    /// </summary>
    public double TableCornerRadius { get; set; }

    /// <summary>
    /// Margins around a table
    /// </summary>
    public Thickness TableThickness { get; set; }

    /// <summary>
    /// Border around a table
    /// </summary>
    public Thickness TableBorderThickness => _tableBorderThickness;

    /// <summary>
    /// Border width of tables
    /// </summary>
    public double TableBorderWidth
    {
        get => _tableBorderWidth;
        set
        {
            _tableBorderWidth = value;
            _tableBorderThickness = new(_tableBorderWidth, 0, _tableBorderWidth, 0);
        }
    }

    /// <summary>
    /// Set default margins based on regular font size
    /// </summary>
    public void SetDefaultMargins()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var quarterSize = Math.Ceiling(RegularFontSize / 4.0);

            RegularThickness = new Thickness(0, quarterSize, 0, 0);
            TableContentThickness = new Thickness(quarterSize, 0, quarterSize, 0);
            TableContentSmallThickness = new Thickness(quarterSize, 0, quarterSize, 0);
            TableContentExtraSmallThickness = new Thickness(quarterSize - 1, 0, quarterSize - 1, 0);
            TableThickness = new Thickness(0, 4 * quarterSize, 0, 2 * quarterSize);

            TableContentUnborderedThickness = new Thickness(0, 0, quarterSize, 0);
            TableContentSmallUnborderedThickness = new Thickness(0, 0, quarterSize, 0);
            TableContentExtraSmallUnborderedThickness = new Thickness(0, 0, quarterSize - 1, 0);

            Heading1Thickness = new Thickness(0, 4 * quarterSize, 0, 2 * quarterSize);
            Heading2Thickness = new Thickness(0, 4 * quarterSize, 0, 2 * quarterSize);
            Heading3Thickness = new Thickness(0, 4 * quarterSize, 0, 2 * quarterSize);
            Heading4Thickness = new Thickness(0, quarterSize, 0, 0);
            Heading5Thickness = new Thickness(0, quarterSize, 0, 0);
            TitleThickness = new Thickness(0, 48 * quarterSize, 0, 16 * quarterSize);
            Title2Thickness = new Thickness(0, 16 * quarterSize, 0, 8 * quarterSize);
            RegularLineHeight = RegularFontSize * 1.2;
            Heading1LineSeparatorThickness = new Thickness(0, 0, 0, 2 * quarterSize);
            FigureThickness = new Thickness(0, 2 * quarterSize, 0, 2 * quarterSize);
        });
    }

    /// <summary>
    /// Margin between heading1 tetxt and line below heading 1. Default 0.5*RegularFontSize
    /// </summary>
    public Thickness Heading1LineSeparatorThickness { get; set; }

    #endregion

    #region Private methods

    private void LoadTypography()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Typography.SetMargins();

            PageSize = new Size(MeasurementHelper.GetDiuFromCm(Typography.TypoPaperFormat.Size.Width),
                MeasurementHelper.GetDiuFromCm(Typography.TypoPaperFormat.Size.Height));

            Margins = new Thickness(MeasurementHelper.GetDiuFromCm(Typography.TypoMargins.Left),
                MeasurementHelper.GetDiuFromCm(Typography.TypoMargins.Top - Typography.PageHeaderHeight - Typography.PageHeaderMargin),
                MeasurementHelper.GetDiuFromCm(Typography.TypoMargins.Right),
                MeasurementHelper.GetDiuFromCm(Typography.TypoMargins.Bottom - Typography.PageFooterHeight - Typography.PageFooterMargin));

            HeaderFontName = Typography.FontName;
            FooterFontName = Typography.FontName;

            RegularFontSize = MeasurementHelper.GetDiuFromPoint(Typography.FontSize);
            SmallFontSize = MeasurementHelper.GetDiuFromPoint(Typography.SmallFontSize);
            ExtraSmallFontSize = MeasurementHelper.GetDiuFromPoint(Typography.ExtraSmallFontSize);
            Heading1FontSize = MeasurementHelper.GetDiuFromPoint(Typography.HeadingFontSize1);
            Heading2FontSize = MeasurementHelper.GetDiuFromPoint(Typography.HeadingFontSize2);
            Heading3FontSize = MeasurementHelper.GetDiuFromPoint(Typography.HeadingFontSize3);
            Heading4FontSize = MeasurementHelper.GetDiuFromPoint(Typography.HeadingFontSize4);
            Heading5FontSize = MeasurementHelper.GetDiuFromPoint(Typography.HeadingFontSize5);
            TitleFontSize = MeasurementHelper.GetDiuFromPoint(Typography.TitleFontSize);
            Title2FontSize = MeasurementHelper.GetDiuFromPoint(Typography.SubTitleFontSize);

            TableBodyBackground = new SolidColorBrush(WpfHelper.GetColor(Typography.TableBodyBackground));
            TableHeaderBackground = new SolidColorBrush(WpfHelper.GetColor(Typography.TableHeaderBackground));
            TableBodyUnborderedBackground = new SolidColorBrush(WpfHelper.GetColor(Typography.TableBodyUnborderedBackground));
            TableHeaderUnborderedBackground = new SolidColorBrush(WpfHelper.GetColor(Typography.TableHeaderUnborderedBackground));

            TableBorder = new SolidColorBrush(WpfHelper.GetColor(Typography.TableBorderColor));
            TableCornerRadius = MeasurementHelper.GetDiuFromCm(Typography.TableCornerRadius);
            TableBorderWidth = MeasurementHelper.GetDiuFromCm(Typography.TableBorderWidth);

            HeaderHeight = MeasurementHelper.GetDiuFromCm(Typography.PageHeaderHeight);
            HeaderMarginBottom = MeasurementHelper.GetDiuFromCm(Typography.PageHeaderMargin);
            FooterMarginTop = MeasurementHelper.GetDiuFromCm(Typography.PageFooterMargin);
            FooterHeight = MeasurementHelper.GetDiuFromCm(Typography.PageFooterHeight);
            FooterFontName = Typography.FontName;
            FooterFontSize = MeasurementHelper.GetDiuFromPoint(Typography.SmallFontSize);

            HeaderHeight += HeaderMarginBottom;
            FooterHeight += FooterMarginTop;

            SetDefaultMargins();
        });
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Load default typography
    /// </summary>
    public void LoadDefaults()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Typography = new ElegantTypographyPageHeader("Calibri", "Calibri", "Calibri");
            LoadTypography();
        });

        SetDefaultMargins();
    }

    /// <summary>
    /// Load default typography for a compact styling
    /// </summary>
    public void LoadCompactDefaults()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Typography = new CompactTypographyPageHeader("Calibri", "Calibri", "Calibri");
            LoadTypography();
        });

        SetDefaultMargins();
    }


    /// <summary>
    /// Load settings from <see cref="ITypography"/> Data
    /// </summary>
    /// <param name="typography"></param>
    public void LoadTypography(ITypography typography)
    {
        Typography = typography;
        LoadTypography();
    }

    #endregion
}