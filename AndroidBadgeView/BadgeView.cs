using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using System;
using static Android.Views.ViewGroup;

namespace AndroidAdapter
{
    public class BadgeView : TextView
    {
        #region Propery - View

        public View TargetView { get { return _targetView; } }
        private View _targetView { get; set; }
        private FrameLayout _containerView { get; set; }
        private ViewGroup _parentView { get; set; }

        #endregion


        #region Propery - BadgeView Parameters

        private static Animation _fadeIn { get; set; }
        private static Animation _fadeOut { get; set; }
        private Context _context { get; set; }
        private ShapeDrawable _badgeBg { get; set; }
        private int _targetTabIndex { get; set; }
        private ShapeDrawable _defaultBackground
        {
            get
            {
                int r = DipToPixels(BadgeViewDefaultValue.DEFAULT_CORNER_RADIUS_DIP);
                float[] outerR = new float[] { r, r, r, r, r, r, r, r };

                RoundRectShape rr = new RoundRectShape(outerR, null, null);
                ShapeDrawable drawable = new ShapeDrawable(rr);
                drawable.Paint.Color = _badgeColor;

                return drawable;
            }
        }

        public BadgeViewPosition Position { get; set; }

        public int HorizontalMargin { get; set; }
        public int VerticalMargin { get; set; }

        public void SetBadgeMargin(int badgeMargin)
        {
            HorizontalMargin = badgeMargin;
            VerticalMargin = badgeMargin;
        }
        public void SetBadgeMargin(int horizontal, int vertical)
        {
            HorizontalMargin = horizontal;
            VerticalMargin = vertical;
        }

        private Color _badgeColor { get; set; }
        public Color BackgroundColor
        {
            get
            {
                return _badgeColor;
            }
            set
            {
                _badgeColor = value;
                _badgeBg = _defaultBackground;
            }
        }

        #endregion


        #region Construction

        public BadgeView(Context context) : this(context, (IAttributeSet)null, Android.Resource.Attribute.TextViewStyle)
        {
        }
        public BadgeView(Context context, IAttributeSet attrs) : this(context, attrs, Android.Resource.Attribute.TextViewStyle)
        {
        }
        public BadgeView(Context context, View target) : this(context, null, Android.Resource.Attribute.TextViewStyle, target, 0)
        {
        }
        public BadgeView(Context context, TabWidget target, int index) : this(context, null, Android.Resource.Attribute.TextViewStyle, target, index)
        {
        }
        public BadgeView(Context context, IAttributeSet attrs, int defStyle) : this(context, attrs, defStyle, null, 0)
        {
        }
        public BadgeView(Context context, IAttributeSet attrs, int defStyle, View target, int tabIndex) : base(context, attrs, defStyle)
        {
            Initial(context, target, tabIndex);
        }

        #endregion


        #region Initial

        private void Initial(Context context, View target, int tabIndex)
        {
            _context = context;
            _targetView = target;
            _targetTabIndex = tabIndex;

            Position = BadgeViewDefaultValue.DEFAULT_POSITION;
            HorizontalMargin = DipToPixels(BadgeViewDefaultValue.DEFAULT_MARGIN_DIP);
            VerticalMargin = HorizontalMargin;
            _badgeColor = BadgeViewDefaultValue.DEFAULT_BADGE_COLOR;

            SetTypeface(Typeface, TypefaceStyle.Bold);
            int paddingPixels = DipToPixels(BadgeViewDefaultValue.DEFAULT_LR_PADDING_DIP);
            SetPadding(paddingPixels, 0, paddingPixels, 0);
            SetTextColor(BadgeViewDefaultValue.DEFAULT_TEXT_COLOR);

            _fadeIn = new AlphaAnimation(0, 1);
            _fadeIn.Interpolator = new DecelerateInterpolator();
            _fadeIn.Duration = 200;

            _fadeOut = new AlphaAnimation(1, 0);
            _fadeOut.Interpolator = new AccelerateInterpolator();
            _fadeOut.Duration = 200;

            if (_targetView != null)
            {
                ApplyTo(_targetView);
            }
            else
            {
                Show();
            }
        }

        private void ApplyTo(View target)
        {
            if (target.GetType() == typeof(TabWidget))
            {
                SetBadge(target as TabWidget, _targetTabIndex);
            }
            else if (target.GetType() == typeof(FrameLayout))
            {
                SetBadge(target as FrameLayout);
            }
            else
            {
                SetBadge(target);
            }
        }

        #endregion


        #region Set/Remove Badge

        /// <summary>
        /// Set Badge View (Any View)
        /// </summary>
        /// <param name="target"></param>
        private void SetBadge(View target)
        {
            if (_containerView == null)
            {
                // Get Target View & Target Parent View
                LayoutParams layoutParams = target.LayoutParameters;
                _targetView = target;
                _parentView = (_parentView == null) ? (target.Parent as ViewGroup) : (_parentView as ViewGroup);

                // Create Container
                _containerView = new FrameLayout(_context);

                // Get Target View Index of Parent View
                int index = _parentView.IndexOfChild(target);

                // Add Container And Move Target View From Parent View To Container
                _parentView.RemoveView(_targetView);
                _parentView.AddView(_containerView, index, layoutParams);
                _containerView.AddView(_targetView);

                // Add Badge TextView
                Visibility = ViewStates.Gone;
                _containerView.AddView(this);

                // Invalidate
                _parentView.Invalidate();
            }
        }

        /// <summary>
        /// Set Badge View (TabWidget)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="tabIndex"></param>
        private void SetBadge(TabWidget target, int tabIndex)
        {
            ViewGroup tabView = target.GetChildTabViewAt(tabIndex) as ViewGroup;
            AddBadge(tabView);
        }

        /// <summary>
        /// Set Badge View (FrameLayout)
        /// </summary>
        /// <param name="target"></param>
        private void SetBadge(FrameLayout target)
        {
            AddBadge(target);
        }

        /// <summary>
        /// Add Badge View
        /// </summary>
        /// <param name="target"></param>
        private void AddBadge(ViewGroup target)
        {
            if (_containerView == null)
            {
                // Create Container
                _containerView = new FrameLayout(_context);

                // Set Target View & Target Parent View
                _parentView = target;

                // Add Container
                _parentView.AddView(_containerView, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));

                // Add Badge TextView
                Visibility = ViewStates.Gone;
                _containerView.AddView(this);
            }
        }

        /// <summary>
        /// Remove Badge View
        /// </summary>
        public void RemoveBadge()
        {
            if (_parentView != null && _containerView != null)
            {
                bool isSpecialType = (_targetView.GetType() == typeof(TabWidget)
                                   || _targetView.GetType() == typeof(FrameLayout));

                if (isSpecialType)
                {
                    // Remove Container
                    _containerView.RemoveAllViews();
                    _parentView.RemoveView(_containerView);
                }
                else
                {
                    // Get Container Index of Parent View
                    int index = _parentView.IndexOfChild(_containerView);

                    // Remove Container And Move Target View From Container To Parent View
                    _containerView.RemoveAllViews();
                    _parentView.RemoveView(_containerView);
                    _parentView.AddView(_targetView, index);

                    // Invalidate
                    _parentView.Invalidate();
                }

                _parentView = null;
                _containerView = null;
            }
        }

        #endregion


        #region Show/Hide

        public void SetVisibility(bool isVisibility)
        {
            if (isVisibility)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        public void SetVisibility(bool isVisibility, bool animate)
        {
            if (isVisibility)
            {
                Show(animate);
            }
            else
            {
                Hide(animate);
            }
        }
        public void SetVisibility(bool isVisibility, Animation anim)
        {
            if (isVisibility)
            {
                Show(anim);
            }
            else
            {
                Hide(anim);
            }
        }
        public void SetVisibility(bool isVisibility, bool animate, Animation anim)
        {
            if (isVisibility)
            {
                Show(animate);
            }
            else
            {
                Hide(anim);
            }
        }

        public void Show()
        {
            Show(false, null);
        }
        public void Show(bool animate)
        {
            Show(animate, _fadeIn);
        }
        public void Show(Animation anim)
        {
            Show(true, anim);
        }
        private void Show(bool animate, Animation anim)
        {
            if (Background == null)
            {
                Background = (_badgeBg != null) ? _badgeBg : _defaultBackground;
            }
            ApplyLayoutParams();

            if (animate)
            {
                StartAnimation(anim);
            }
            Visibility = ViewStates.Visible;
        }

        public void Hide()
        {
            Hide(false, null);
        }
        public void Hide(bool animate)
        {
            Hide(animate, _fadeOut);
        }
        public void Hide(Animation anim)
        {
            Hide(true, anim);
        }
        private void Hide(bool animate, Animation anim)
        {
            //(Parent as ViewGroup).GetChildAt(1).Visibility = ViewStates.Gone;
            //(Parent as ViewGroup).GetChildAt(1).Invalidate();
            (Parent as ViewGroup).Invalidate();
            Visibility = ViewStates.Gone;
            Invalidate();
            if (animate)
            {
                StartAnimation(anim);
            }
        }

        public void Toggle()
        {
            Toggle(false, null, null);
        }
        public void Toggle(bool animate)
        {
            Toggle(animate, _fadeIn, _fadeOut);
        }
        public void Toggle(Animation animIn, Animation animOut)
        {
            Toggle(true, animIn, animOut);
        }
        private void Toggle(bool animate, Animation animIn, Animation animOut)
        {
            if (IsShown)
            {
                Hide(animate && (animOut != null), animOut);
            }
            else
            {
                Show(animate && (animIn != null), animIn);
            }
        }

        #endregion


        #region Update

        public int Increment(int offset = 1)
        {
            int i = 0;
            if (Text != null)
            {
                try
                {
                    i = int.Parse(Text.ToString());
                }
                catch (Exception)
                {
                }
            }

            i = i + offset;

            Text = Convert.ToString(i);
            return i;
        }
        public int Decrement(int offset = 1)
        {
            return Increment(-offset);
        }
        public void SetZero()
        {
            SetBadgeText(0);
        }

        public string SetBadgeText(object number)
        {
            Text = Convert.ToString(number);
            return Text.ToString();
        }

        #endregion


        #region Private Method

        private void ApplyLayoutParams()
        {
            FrameLayout.LayoutParams lp = new FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            switch (Position)
            {
                case BadgeViewPosition.TopLeft:
                    lp.Gravity = GravityFlags.Left | GravityFlags.Top;
                    lp.SetMargins(HorizontalMargin, VerticalMargin, 0, 0);
                    break;
                case BadgeViewPosition.TopRight:
                    lp.Gravity = GravityFlags.Right | GravityFlags.Top;
                    lp.SetMargins(0, VerticalMargin, HorizontalMargin, 0);
                    break;
                case BadgeViewPosition.BottomLeft:
                    lp.Gravity = GravityFlags.Left | GravityFlags.Bottom;
                    lp.SetMargins(HorizontalMargin, 0, 0, VerticalMargin);
                    break;
                case BadgeViewPosition.BottomRight:
                    lp.Gravity = GravityFlags.Right | GravityFlags.Bottom;
                    lp.SetMargins(0, 0, HorizontalMargin, VerticalMargin);
                    break;
                case BadgeViewPosition.Center:
                    lp.Gravity = GravityFlags.Center;
                    lp.SetMargins(0, 0, 0, 0);
                    break;
                case BadgeViewPosition.Top:
                    lp.Gravity = GravityFlags.Top | GravityFlags.CenterHorizontal;
                    lp.SetMargins(0, VerticalMargin, 0, 0);
                    break;
                case BadgeViewPosition.Bottom:
                    lp.Gravity = GravityFlags.Bottom | GravityFlags.CenterHorizontal;
                    lp.SetMargins(0, 0, 0, VerticalMargin);
                    break;
                case BadgeViewPosition.Left:
                    lp.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;
                    lp.SetMargins(HorizontalMargin, 0, 0, 0);
                    break;
                case BadgeViewPosition.Right:
                    lp.Gravity = GravityFlags.Right | GravityFlags.CenterVertical;
                    lp.SetMargins(0, 0, HorizontalMargin, 0);
                    break;
                default:
                    break;
            }

            LayoutParameters = lp;
        }

        private int DipToPixels(int dip)
        {
            float px = TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, Resources.DisplayMetrics);
            return (int)px;
        }

        #endregion
    }
}
