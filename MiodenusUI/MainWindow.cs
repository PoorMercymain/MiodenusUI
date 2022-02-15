using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace MiodenusUI
{
    class MainWindow : Window
    {
        [UI] private Label _label = null;
        [UI] private ImageMenuItem _createButton = null;

        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _createButton.Activated += CreateButton_Clicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void CreateButton_Clicked(object sender, EventArgs a)
        {
            
            var init = new Window("Set up your animation");

            init.SetDefaultSize(500, 500);
            init.Resizable = false;
            
            Fixed fixedLayout = new Fixed();
            fixedLayout.SetSizeRequest(500,500);
            
            //Text labels
            Label nameLabel = new Label("Name:");
            fixedLayout.Put(nameLabel, 10, 8);

            Label placeholderLabel1 = new Label("Placeholder:");
            fixedLayout.Put(placeholderLabel1, 10, 32);
            
            Label placeholderLabel2 = new Label("Placeholder:");
            fixedLayout.Put(placeholderLabel2, 10, 56);
            
            Label placeholderLabel3 = new Label("Placeholder:");
            fixedLayout.Put(placeholderLabel3, 10, 80);
            
            Label placeholderLabel4 = new Label("Placeholder:");
            fixedLayout.Put(placeholderLabel4, 10, 104);
            
            //Text views
            void SetPlaceholderTextViewSizeAndPosition(TextView placeholderTextView, int positionX, int positionY)
            {
                placeholderTextView.SetSizeRequest(400, 16);            
                fixedLayout.Put(placeholderTextView, positionX, positionY);
            }
            
            TextView nameTextView = new TextView();
            nameTextView.SetSizeRequest(430, 16);            
            fixedLayout.Put(nameTextView, 50, 8);
            
            TextView placeholderTextView1 = new TextView();
            SetPlaceholderTextViewSizeAndPosition(placeholderTextView1, 80, 32);

            TextView placeholderTextView2 = new TextView();
            SetPlaceholderTextViewSizeAndPosition(placeholderTextView2, 80, 56);

            TextView placeholderTextView3 = new TextView();
            SetPlaceholderTextViewSizeAndPosition(placeholderTextView3, 80, 80);

            TextView placeholderTextView4 = new TextView();
            SetPlaceholderTextViewSizeAndPosition(placeholderTextView4, 80, 104);

            init.Add(fixedLayout);
            init.ShowAll();
        }
    }
}