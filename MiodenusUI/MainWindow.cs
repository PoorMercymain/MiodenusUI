using System;
using System.IO;
using Gtk;
using MiodenusUI.MAFStructure;
using UI = Gtk.Builder.ObjectAttribute;

namespace MiodenusUI
{
    class MainWindow : Window
    {
        [UI] private Label _label = null;
        [UI] private ImageMenuItem _createButton = null;
        [UI] private ImageMenuItem _exitButton = null;
        [UI] private Fixed _fixedMainWindow = null;

        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            
            LoaderMaf maf = new LoaderMaf();

            MAFStructure.Animation animation = new Animation();
            animation = maf.Read("test.txt");
            
            Label animationInfoLabel = new Label(animation.AnimationInfo.Type + ", " + animation.AnimationInfo.Version + ", " +
                                                 animation.AnimationInfo.Name + ", " + animation.AnimationInfo.Fps + " fps, " +
                                                 animation.AnimationInfo.FrameHeight + "x" +
                                                 animation.AnimationInfo.FrameWidth + ", " + animation.AnimationInfo.TimeLength + " seconds, " + 
                                                 animation.AnimationInfo.VideoName + ", " + animation.AnimationInfo.VideoType);
            _fixedMainWindow.Put(animationInfoLabel, 800, 700);
            this.ShowAll();

            var mafStr = maf.CreateMafString(animation);

            var writer = File.CreateText("test1.txt");
            writer.Write(mafStr);
            writer.Close();
            
            animation = maf.Read("test1.txt");
            
            DeleteEvent += Window_DeleteEvent;
            _exitButton.Activated += Program_Quit;
            _createButton.Activated += CreateButton_Clicked;
            
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
        
        private void Program_Quit(object sender, EventArgs a)
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

            Label typeLabel = new Label("Type:");
            fixedLayout.Put(typeLabel, 10, 32);
            
            Label versionLabel = new Label("Version:");
            fixedLayout.Put(versionLabel, 10, 56);
            
            Label videoTypeLabel = new Label("Video type:");
            fixedLayout.Put(videoTypeLabel, 10, 80);
            
            Label videoNameLabel = new Label("Video name:");
            fixedLayout.Put(videoNameLabel, 10, 104);
            
            Label timeLengthLabel = new Label("Time length:");
            fixedLayout.Put(timeLengthLabel, 10, 128);
            
            Label fpsLabel = new Label("FPS:");
            fixedLayout.Put(fpsLabel, 10, 156);
            
            Label frameWidthLabel = new Label("Frame width:");
            fixedLayout.Put(frameWidthLabel, 10, 180);
            
            Label frameHeightLabel = new Label("Frame height:");
            fixedLayout.Put(frameHeightLabel, 10, 204);
            
            //Text views
            void SetPlaceholderTextViewSizeAndPosition(TextView placeholderTextView, int positionX, int positionY)
            {
                placeholderTextView.SetSizeRequest(400, 16);            
                fixedLayout.Put(placeholderTextView, positionX, positionY);
            }
            
            TextView nameTextView = new TextView();
            nameTextView.SetSizeRequest(430, 16);            
            fixedLayout.Put(nameTextView, 85, 8);
            
            TextView typeTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(typeTextView, 85, 32);

            TextView versionTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(versionTextView, 85, 56);

            TextView videoTypeTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(videoTypeTextView, 85, 80);

            TextView videoNameTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(videoNameTextView, 85, 104);
            
            TextView timeLengthTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(timeLengthTextView, 85, 128);
            
            TextView fpsTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(fpsTextView, 85, 156);
            
            TextView frameWidthTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(frameWidthTextView, 85, 180);
            
            TextView frameHeightTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(frameHeightTextView, 85, 204);

            //Buttons
            Button okButton = new Button("Ok");
            okButton.SetSizeRequest(45, 30);
            fixedLayout.Put(okButton, 275, 452);

            Button cancelButton = new Button("Cancel");
            cancelButton.SetSizeRequest(45, 30);
            fixedLayout.Put(cancelButton, 155, 452);
            
            init.Add(fixedLayout);
            init.ShowAll();

            okButton.Clicked += OkButton_Clicked;
            cancelButton.Clicked += CancelButton_Clicked;
            
            void OkButton_Clicked(object sender, EventArgs a)
            {
                var animationNew = new Animation();
                animationNew.AnimationInfo.Name = nameTextView.Buffer.Text;
                animationNew.AnimationInfo.Type = typeTextView.Buffer.Text;
                animationNew.AnimationInfo.Version = versionTextView.Buffer.Text;
                animationNew.AnimationInfo.VideoType = videoTypeTextView.Buffer.Text;
                animationNew.AnimationInfo.VideoName = videoNameTextView.Buffer.Text;
                animationNew.AnimationInfo.TimeLength = int.Parse(timeLengthTextView.Buffer.Text);
                animationNew.AnimationInfo.Fps = int.Parse(fpsTextView.Buffer.Text);
                animationNew.AnimationInfo.FrameWidth = int.Parse(frameWidthTextView.Buffer.Text);
                animationNew.AnimationInfo.FrameHeight = int.Parse(frameHeightTextView.Buffer.Text);
                
                LoaderMaf mafLoaderNew = new LoaderMaf();
                var mafStr = mafLoaderNew.CreateMafString(animationNew);

                var writerNew = File.CreateText("test1.txt");
                writerNew.Write(mafStr);
                writerNew.Close();
                
                init.Close();
            }
        
            void CancelButton_Clicked(object sender, EventArgs a)
            {
                init.Close();
            }
        }
    }
}