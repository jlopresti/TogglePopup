using System;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace App4
{
    [TemplatePart(Name = TOGGLE_BUTTON_PART, Type = typeof(ToggleButton))]
    [TemplatePart(Name = BUTTON_PART, Type = typeof(Button))]
    [TemplatePart(Name = POPUP_PART, Type = typeof(Popup))]
    [ContentProperty(Name = CONTENT_PART)]
    public sealed class TogglePopup : Control
    {
        #region TEMPLATE PARTS
        private const string CONTENT_PART = "PopupContent";
        private const string TOGGLE_BUTTON_PART = "toggleButton";
        private const string BUTTON_PART = "button";
        private const string POPUP_PART = "popup";
        #endregion

        #region TEMPLATE CONTROLS
        private ToggleButton _toggleButton;
        private Button _button;
        private Popup _popup;
        #endregion

        public TogglePopup()
        {
            this.DefaultStyleKey = typeof(TogglePopup);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate.
        ///  In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence
        ///  the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = (Popup)GetTemplateChild(POPUP_PART);
            _button = (Button)GetTemplateChild(BUTTON_PART);
            _toggleButton = (ToggleButton)GetTemplateChild(TOGGLE_BUTTON_PART);

            if (IsToggle)
            {
                InitToggleBehavior();
            }
            else
            {
                InitButtonBehavior();
            }

            //Remove parent's popup
            var parent = (Panel)_popup.Parent;
            parent.Children.Remove(_popup);

            _popup.Closed += _popup_Closed;
        }        

        #region CONTROLS EVENT

        private void _button_Clicked(object sender, RoutedEventArgs e)
        {
            if (!_popup.IsOpen)
                BuildPopup((FrameworkElement)sender);

            _popup.IsOpen = !_popup.IsOpen;
        }

        private void _toggleButton_Checked(object sender, RoutedEventArgs e)
        {
            BuildPopup((FrameworkElement)sender);
            _popup.IsOpen = true;
        }

        private void _toggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = false;
        }

        private void _popup_Closed(object sender, object e)
        {
            if (_toggleButton != null)
                _toggleButton.IsChecked = false;
        }

        #endregion

        #region PROPERTIES

        public object PopupContent
        {
            get { return (object)GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupContentProperty =
            DependencyProperty.Register("PopupContent", typeof(object), typeof(TogglePopup), new PropertyMetadata(null, OnPopupContentChanged));

        private static void OnPopupContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Wrap non frameworkelement into Textblock to measure right content size
            var target = (TogglePopup)d;
            if (!(e.NewValue is FrameworkElement))
            {
                target.PopupContent = new TextBlock()
                    {
                        Text = e.NewValue.ToString()
                    };
            }
        }


        public Placement PopupPlacement
        {
            get { return (Placement)GetValue(PopupPlacementProperty); }
            set { SetValue(PopupPlacementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupPlacement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register("PopupPlacement", typeof(Placement), typeof(TogglePopup), new PropertyMetadata(Placement.Default));


        public Thickness PopupPadding
        {
            get { return (Thickness)GetValue(PopupPaddingProperty); }
            set { SetValue(PopupPaddingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupPadding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupPaddingProperty =
            DependencyProperty.Register("PopupPadding", typeof(Thickness), typeof(TogglePopup), new PropertyMetadata(new Thickness(10, 10, 10, 10)));


        public Thickness PopupBorderThickness
        {
            get { return (Thickness)GetValue(PopupBorderThicknessProperty); }
            set { SetValue(PopupBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupBorderThicknessProperty =
            DependencyProperty.Register("PopupBorderThickness", typeof(Thickness), typeof(TogglePopup), new PropertyMetadata(new Thickness(2)));


        public Brush PopupBorderBrush
        {
            get { return (Brush)GetValue(PopupBorderBrushProperty); }
            set { SetValue(PopupBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupBorderBrushProperty =
            DependencyProperty.Register("PopupBorderBrush", typeof(Brush), typeof(TogglePopup), new PropertyMetadata(null));


        public bool IsToggle
        {
            get { return (bool)GetValue(IsToggleProperty); }
            set { SetValue(IsToggleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsToggle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsToggleProperty =
            DependencyProperty.Register("IsToggle", typeof (bool), typeof (TogglePopup), new PropertyMetadata(true, OnToggleChanged));

        private static void OnToggleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (TogglePopup)d;
            if ((bool) e.NewValue)
                target.InitToggleBehavior();
            else
                target.InitButtonBehavior();
        }

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(TogglePopup), new PropertyMetadata(null));


        public bool IsLightDismissEnabled
        {
            get { return (bool)GetValue(IsLightDismissEnabledProperty); }
            set { SetValue(IsLightDismissEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLightDismissEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLightDismissEnabledProperty =
            DependencyProperty.Register("IsLightDismissEnabled", typeof(bool), typeof(TogglePopup), new PropertyMetadata(true));



        public bool UseAnimation
        {
            get { return (bool)GetValue(UseAnimationProperty); }
            set { SetValue(UseAnimationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UseAnimation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseAnimationProperty =
            DependencyProperty.Register("UseAnimation", typeof(bool), typeof(TogglePopup), new PropertyMetadata(true));


        #endregion

        private void BuildPopup(FrameworkElement source)
        {
            var windowBounds = Window.Current.Bounds;
            var rootVisual = Window.Current.Content;

            GeneralTransform gt = source.TransformToVisual(rootVisual);

            var absolutePosition = gt.TransformPoint(new Point(0, 0));

            var content = (FrameworkElement)_popup.Child;
            content.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var contentWidth = double.IsNaN(content.Width) ? content.DesiredSize.Width : content.Width;
            var contentHeight = double.IsNaN(content.Height) ? content.DesiredSize.Height : content.Height;
            double vOffset = 0;
            double hOffset = 0;

            double vOffsetAnim = 0;
            double hOffsetAnim = 0;
            switch (PopupPlacement)
            {
                case Placement.Above:
                    vOffset = absolutePosition.Y - contentHeight - 10;
                    hOffset = (absolutePosition.X) + source.ActualWidth / 2 - contentWidth / 2;
                    vOffsetAnim = 20;
                    break;
                case Placement.Below:
                    vOffset = absolutePosition.Y + source.ActualHeight + 10;
                    hOffset = (absolutePosition.X) + source.ActualWidth / 2 - contentWidth / 2;
                    vOffsetAnim = -20;
                    break;
                case Placement.Default:
                case Placement.Left:
                    vOffset = absolutePosition.Y - contentHeight / 2 + source.ActualHeight / 2;
                    hOffset = (absolutePosition.X) - 10 - contentWidth;
                    hOffsetAnim = 20;
                    break;
                case Placement.Right:
                    vOffset = absolutePosition.Y - contentHeight / 2 + source.ActualHeight / 2;
                    hOffset = (absolutePosition.X) + 10 + source.ActualWidth;
                    hOffsetAnim = -20;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Limite position to windows bound
            vOffset = Math.Max(10, Math.Min(vOffset, windowBounds.Height - contentHeight - 10));
            hOffset = Math.Max(10, Math.Min(hOffset, windowBounds.Width - contentWidth - 10));

            _popup.VerticalOffset = vOffset;
            _popup.HorizontalOffset = hOffset;

            if (UseAnimation)
            {
                var transitions = new TransitionCollection();
                transitions.Add(new PopupThemeTransition()
                    {
                        FromHorizontalOffset = hOffsetAnim,
                        FromVerticalOffset = vOffsetAnim
                    });
                _popup.ChildTransitions = transitions;
            }
        }
       
        private void InitButtonBehavior()
        {
            if (_toggleButton == null || _button == null) return;

            _toggleButton.Visibility = Visibility.Collapsed;
            _toggleButton.Unchecked -= _toggleButton_Unchecked;
            _toggleButton.Checked -= _toggleButton_Checked;
            _button.Visibility = Visibility.Visible;
            _button.Click += _button_Clicked;
        }

        private void InitToggleBehavior()
        {
            if (_toggleButton == null || _button == null) return;

            _button.Visibility = Visibility.Collapsed;
            _button.Click -= _button_Clicked;
            _toggleButton.Visibility = Visibility.Visible;
            _toggleButton.Unchecked += _toggleButton_Unchecked;
            _toggleButton.Checked += _toggleButton_Checked;
        }
    }
}
