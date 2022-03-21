using System;
using System.Collections.Generic;
using System.IO;
using Gdk;
using GLib;
using Gtk;
using MiodenusUI.MAFStructure;
using Application = Gtk.Application;
using Menu = Gtk.Menu;
using MenuItem = Gtk.MenuItem;
using UI = Gtk.Builder.ObjectAttribute;
using Window = Gtk.Window;

namespace MiodenusUI
{
    class MainWindow : Window
    {
        private string currentFilePath = "test.txt";
        private Label animationInfoLabel = new Label();
        
        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MenuBar CreateMenuBar(MenuItem newMenuItem, MenuItem openMenuItem, MenuItem exitMenuItem)
        {
            MenuBar bar = new MenuBar();
            
            Menu filemenu = new Menu();
            MenuItem file = new MenuItem("File");
            file.Submenu = filemenu;
            
            filemenu.Append(newMenuItem);
            filemenu.Append(openMenuItem);
            filemenu.Append(exitMenuItem);
            
            bar.Append(file);

            return bar;
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            Box allWindowElements = new Box(Gtk.Orientation.Vertical, 0);
            
            int firstColumnBoxesAmount = 100;
            
            Gdk.Color mainColor = new Color(90, 103, 131);
            Gdk.Color almostWhite = new Color(245, 244, 242);
            Gdk.Color almostBlack = new Color(35, 42, 62);
            
            MenuItem newMenuItem = new MenuItem("New");
            MenuItem openMenuItem = new MenuItem("Open");
            MenuItem exitMenuItem = new MenuItem("Exit");

            MenuBar bar = CreateMenuBar(newMenuItem, openMenuItem, exitMenuItem);
            
            allWindowElements.PackStart(bar, false, false, 0);

            builder.Autoconnect(this);
            
            LoaderMaf maf = new LoaderMaf();

            MAFStructure.Animation animation = new Animation();
            animation = maf.Read(currentFilePath);
            
            animationInfoLabel.Text = animation.AnimationInfo.Type + ", " + animation.AnimationInfo.Version + ", " +
                                                 animation.AnimationInfo.Name + ", " + animation.AnimationInfo.Fps + " fps, " +
                                                 animation.AnimationInfo.FrameHeight + "x" +
                                                 animation.AnimationInfo.FrameWidth + ", " + animation.AnimationInfo.TimeLength + " seconds, " + 
                                                 animation.AnimationInfo.VideoName + ", " + animation.AnimationInfo.VideoType;

            animationInfoLabel.ModifyFg(Gtk.StateType.Normal, almostWhite);

            Box animationInfoOuterHBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box animationInfoHBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box backgroundHBox = new Box(Gtk.Orientation.Horizontal, 0);
            
            ScrolledWindow scrolledTimelinesNames = new ScrolledWindow();

            Box timelinesNamesVBox = new Box(Gtk.Orientation.Vertical, 0);
            Box timelinesVBox = new Box(Gtk.Orientation.Vertical, 0);
            Box propertiesVBox = new Box(Gtk.Orientation.Vertical, 0);

            timelinesNamesVBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            timelinesVBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            propertiesVBox.ModifyBg(Gtk.StateType.Normal, almostBlack);

            timelinesNamesVBox.Margin = 5;
            timelinesVBox.Margin = 5;
            propertiesVBox.Margin = 5;
            
            backgroundHBox.ModifyBg(Gtk.StateType.Normal, mainColor);

            timelinesNamesVBox.SetSizeRequest(400, 35);
            propertiesVBox.SetSizeRequest(400, 35);

            Button createFirstColumnBox()
            {
                Button firstColumnButton = new Button("test");
                
                firstColumnButton.Margin = 5;

                return firstColumnButton;
            }

            List<Button> firstColumnButtons = new List<Button>();
            Box scrolledTimelinesElementsBox = new Box(Gtk.Orientation.Vertical, 0);
            for(var i = 0;i<100;i++)
            {
                Button firstColumnBox = createFirstColumnBox();
                firstColumnButtons.Add(firstColumnBox);
                scrolledTimelinesElementsBox.Add(firstColumnButtons[^1]);
            }

            scrolledTimelinesElementsBox.Vexpand = true;
            
            scrolledTimelinesNames.Add(scrolledTimelinesElementsBox);
            
            timelinesNamesVBox.Add(scrolledTimelinesNames);

            Box timelinesBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box divBox = new Box(Gtk.Orientation.Vertical, 0);
            Box divHBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box macBox = new Box(Gtk.Orientation.Horizontal, 0);

            macBox.SetSizeRequest(300,600);
            macBox.Margin = 5;
            
            macBox.ModifyBg(Gtk.StateType.Normal, almostWhite);
            
            divBox.Add(macBox);
            
            divHBox.PackStart(timelinesNamesVBox, false, false, 0);
            timelinesVBox.Expand = true;
            divHBox.Add(timelinesVBox);
            
            divBox.Add(divHBox);
            
            timelinesBox.Add(divBox);

            //backgroundHBox.PackStart(timelinesNamesVBox, false, false, 0);//testHBox.Add(scroll);//PackStart(scroll, true, true, 0);
            //timelinesVBox.Expand = true;
            //backgroundHBox.Add(timelinesVBox);
            backgroundHBox.Add(timelinesBox);
            backgroundHBox.PackEnd(propertiesVBox,false,false,0);//backgroundHBox.Add(propertiesVBox);

            allWindowElements.Add(backgroundHBox);
            
            animationInfoHBox.Add(animationInfoLabel);

            animationInfoHBox.Margin = 10;
            
            animationInfoHBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            animationInfoOuterHBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            
            //animationInfoOuterHBox.Add(animationInfoHBox);
            
            //animationInfoOuterHBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            
            allWindowElements.PackEnd(animationInfoOuterHBox, false, false, 0);
            
            animationInfoHBox.Halign = Align.End;
            //animationInfoHBox.Valign = Align.End;
            
            //animationInfoHBox.ModifyBg(Gtk.StateType.Normal, mainColor);
            
            animationInfoOuterHBox.PackEnd(animationInfoHBox, false, false, 0);
            
            //allWindowElements.Add(animationInfoHBox);

            //HBox labelHbox = new HBox();
            //labelHbox.SetSizeRequest(100, 100);
            //Gdk.Color color = new Gdk.Color(255, 0, 0);
            //Label test = new Label("test");
            //labelHbox.Add(test);
            //labelHbox.ModifyBg(Gtk.StateType.Normal, color);
            Add(allWindowElements);
            
            this.ShowAll();

            var mafStr = maf.CreateMafString(animation);

            var writer = File.CreateText("test1.txt");
            writer.Write(mafStr);
            writer.Close();
            
            animation = maf.Read("test1.txt");
            
            DeleteEvent += Window_DeleteEvent;
            exitMenuItem.Activated += Program_Quit;
            newMenuItem.Activated += CreateButton_Clicked;
            openMenuItem.Activated += OpenButton_Clicked;

        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
        
        private void Program_Quit(object sender, EventArgs a)
        {
            Application.Quit();
        }
        
        private void OpenButton_Clicked(object sender, EventArgs a)
        {
            var openDialog = new FileChooserDialog("Open", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
            openDialog.ShowAll();

            if (openDialog.Run() == (int)ResponseType.Accept) 
            {

                var mafOpen = new LoaderMaf();
                var openedAnimation = new Animation();
                openedAnimation = mafOpen.Read(openDialog.Filename);
                
                animationInfoLabel.Text = openedAnimation.AnimationInfo.Type + ", " + openedAnimation.AnimationInfo.Version + ", " +
                                          openedAnimation.AnimationInfo.Name + ", " + openedAnimation.AnimationInfo.Fps + " fps, " +
                                          openedAnimation.AnimationInfo.FrameHeight + "x" +
                                          openedAnimation.AnimationInfo.FrameWidth + ", " + openedAnimation.AnimationInfo.TimeLength + " seconds, " + 
                                          openedAnimation.AnimationInfo.VideoName + ", " + openedAnimation.AnimationInfo.VideoType;
                
            }

            openDialog.Destroy();
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
            
            Label animationPathLabel = new Label("Animation path:");
            fixedLayout.Put(animationPathLabel, 10, 228);
            
            //Text views
            void SetPlaceholderTextViewSizeAndPosition(TextView placeholderTextView, int positionX, int positionY)
            {
                placeholderTextView.SetSizeRequest(400, 16);            
                fixedLayout.Put(placeholderTextView, positionX, positionY);
            }
            
            TextView nameTextView = new TextView();
            nameTextView.SetSizeRequest(400, 16);            
            fixedLayout.Put(nameTextView, 95, 8);
            
            TextView typeTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(typeTextView, 95, 32);

            TextView versionTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(versionTextView, 95, 56);

            TextView videoTypeTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(videoTypeTextView, 95, 80);

            TextView videoNameTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(videoNameTextView, 95, 104);
            
            TextView timeLengthTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(timeLengthTextView, 95, 128);
            
            TextView fpsTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(fpsTextView, 95, 156);
            
            TextView frameWidthTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(frameWidthTextView, 95, 180);
            
            TextView frameHeightTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(frameHeightTextView, 95, 204);
            
            TextView animationPathTextView = new TextView();
            SetPlaceholderTextViewSizeAndPosition(animationPathTextView, 95, 228);

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

                var writerNew = File.CreateText(animationPathTextView.Buffer.Text);
                writerNew.Write(mafStr);
                writerNew.Close();

                currentFilePath = animationPathTextView.Buffer.Text;
                animationInfoLabel.Text = animationNew.AnimationInfo.Type + ", " + animationNew.AnimationInfo.Version + ", " +
                                     animationNew.AnimationInfo.Name + ", " + animationNew.AnimationInfo.Fps + " fps, " +
                                     animationNew.AnimationInfo.FrameHeight + "x" +
                                     animationNew.AnimationInfo.FrameWidth + ", " + animationNew.AnimationInfo.TimeLength + " seconds, " + 
                                     animationNew.AnimationInfo.VideoName + ", " + animationNew.AnimationInfo.VideoType;
                init.Close();
            }
        
            void CancelButton_Clicked(object sender, EventArgs a)
            {
                init.Close();
            }
        }
    }
}