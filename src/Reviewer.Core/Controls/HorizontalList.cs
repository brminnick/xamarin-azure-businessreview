using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

/*
 * This comes straight from the Hotel 360 demo app!
 * The repo can be found here: https://github.com/Microsoft/SmartHotel360-mobile-desktop-apps/blob/master/src/SmartHotel.Clients/SmartHotel.Clients/Controls/HorizontalList.cs
 */

namespace Reviewer.Core
{
    class HorizontalList : Grid
    {
        public static readonly BindableProperty SelectedCommandProperty =
            BindableProperty.Create("SelectedCommand", typeof(ICommand), typeof(HorizontalList), null);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(HorizontalList), default(IEnumerable<object>), BindingMode.TwoWay, propertyChanged: ItemsSourceChanged);

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create("SelectedItem", typeof(object), typeof(HorizontalList), null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(HorizontalList), default(DataTemplate));

        readonly AsyncAwaitBestPractices.WeakEventManager weakEventManager = new();

        readonly ScrollView scrollView;
        readonly StackLayout itemsStackLayout;

        ICommand? _innerSelectedCommand;

        public HorizontalList()
        {
            scrollView = new ScrollView();

            itemsStackLayout = new StackLayout
            {
                BackgroundColor = BackgroundColor,
                Padding = Padding,
                Spacing = Spacing,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            scrollView.BackgroundColor = BackgroundColor;
            scrollView.Content = itemsStackLayout;

            Children.Add(scrollView);
        }

        public event EventHandler SelectedItemChanged
        {
            add => weakEventManager.AddEventHandler(value);
            remove => weakEventManager.RemoveEventHandler(value);
        }

        public ICommand? SelectedCommand
        {
            get { return (ICommand?)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        public IEnumerable? ItemsSource
        {
            get { return (IEnumerable?)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object? SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public DataTemplate? ItemTemplate
        {
            get { return (DataTemplate?)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public StackOrientation ListOrientation { get; set; }

        public double Spacing { get; set; }

        public void RaiseSelectedItemChanged() => weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(SelectedItemChanged));

        protected virtual void SetItems()
        {
            itemsStackLayout.Children.Clear();
            itemsStackLayout.Spacing = Spacing;

            _innerSelectedCommand = new Command<View>(view =>
            {
                SelectedItem = view.BindingContext;
                SelectedItem = null; // Allowing item second time selection
            });

            itemsStackLayout.Orientation = ListOrientation;
            scrollView.Orientation = ListOrientation is StackOrientation.Horizontal
                                        ? ScrollOrientation.Horizontal
                                        : ScrollOrientation.Vertical;

            if (ItemsSource == null)
            {
                return;
            }

            foreach (var item in ItemsSource)
            {
                itemsStackLayout.Children.Add(GetItemView(item));
            }

            itemsStackLayout.BackgroundColor = BackgroundColor;
            SelectedItem = null;
        }

        protected virtual View? GetItemView(object item)
        {
            var content = ItemTemplate?.CreateContent();

            if (content is not View view)
            {
                return null;
            }

            view.BindingContext = item;

            var gesture = new TapGestureRecognizer
            {
                Command = _innerSelectedCommand,
                CommandParameter = view
            };

            AddGesture(view, gesture);

            return view;
        }

        void AddGesture(View view, TapGestureRecognizer gesture)
        {
            view.GestureRecognizers.Add(gesture);

            if (view is not Layout<View> layout)
            {
                return;
            }

            foreach (var child in layout.Children)
            {
                AddGesture(child, gesture);
            }
        }

        static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == oldValue && newValue != null)
            {
                return;
            }

            var itemsView = (HorizontalList)bindable;
            itemsView.RaiseSelectedItemChanged();

            if (itemsView.SelectedCommand?.CanExecute(newValue) ?? false)
            {
                itemsView.SelectedCommand?.Execute(newValue);
            }
        }

        static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsLayout = (HorizontalList)bindable;
            itemsLayout.SetItems();
        }
    }
}
