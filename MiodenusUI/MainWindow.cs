using System;
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
        [UI] private Label _label = null;

        private string currentFilePath = "test.txt";
        private Label animationInfoLabel = new Label();
        
        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            Gdk.Color mainColor = new Color(90, 103, 131);
            Gdk.Color almostWhite = new Color(245, 244, 242);
            Gdk.Color almostBlack = new Color(35, 42, 62);
            
            MenuBar bar = new MenuBar();
            
            Menu filemenu = new Menu();
            MenuItem file = new MenuItem("File");
            file.Submenu = filemenu;
       
            MenuItem newMenuItem = new MenuItem("New");
            MenuItem openMenuItem = new MenuItem("Open");
            MenuItem exitMenuItem = new MenuItem("Exit");
            
            filemenu.Append(newMenuItem);
            filemenu.Append(openMenuItem);
            filemenu.Append(exitMenuItem);
            
            bar.Append(file);
            
            VBox vbox = new VBox(false, 2);
            vbox.PackStart(bar, false, false, 0);
            
            this.Add(vbox);
            
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

            HBox animationInfoOuterHBox = new HBox();
            HBox animationInfoHBox = new HBox();
            HBox testHBox = new HBox();
            VBox actionVBox1 = new VBox();
            VBox actionVBox2 = new VBox();
            VBox actionVBox3 = new VBox();

            actionVBox1.BorderWidth = 2;
            actionVBox1.ModifyBg(Gtk.StateType.Normal, almostBlack);
            actionVBox2.BorderWidth = 2;
            actionVBox2.ModifyBg(Gtk.StateType.Normal, almostBlack);
            actionVBox3.BorderWidth = 2;
            actionVBox3.ModifyBg(Gtk.StateType.Normal, almostBlack);
            
            //actionVBox1.StyleContext.SetProperty("border-color", new Value("#232A3E"));

            testHBox.ModifyBg(Gtk.StateType.Normal, mainColor);
            
            testHBox.PackStart(actionVBox1, false, false, 0);
            testHBox.PackEnd(actionVBox3, false, false, 0);

            actionVBox1.SetSizeRequest(400, 35);
            actionVBox3.SetSizeRequest(400, 35);

            Box createFirstColumnBox()
            {
                Box firstColumnBox = new Box(Gtk.Orientation.Vertical, 0);
                firstColumnBox.SetSizeRequest(5, 35);
                firstColumnBox.ModifyBg(Gtk.StateType.Normal, mainColor);
                firstColumnBox.Margin = 5;

                return firstColumnBox;
            }

            Box firstColumnBox1 = createFirstColumnBox();
            //Box firstColumnInnerBox1 = createFirstColumnBox();
            //firstColumnInnerBox1.ModifyBg(Gtk.StateType.Normal, almostBlack);
            //firstColumnBox1.Add(firstColumnInnerBox1);
            
            Box firstColumnBox2 = createFirstColumnBox();
            //Box firstColumnInnerBox2 = createFirstColumnBox();
            //firstColumnInnerBox2.ModifyBg(Gtk.StateType.Normal, almostBlack);
            //firstColumnBox2.Add(firstColumnInnerBox2);
            
            //firstColumnBox2.ModifyBg(Gtk.StateType.Normal, almostBlack);
            
            Box firstColumnBox3 = createFirstColumnBox();
            Box firstColumnBox4 = createFirstColumnBox();
            
            Box firstColumnBox5 = createFirstColumnBox();
            Box firstColumnBox6 = createFirstColumnBox();
            
            Box firstColumnBox7 = createFirstColumnBox();
            Box firstColumnBox8 = createFirstColumnBox();
            
            Box firstColumnBox9 = createFirstColumnBox();
            Box firstColumnBox10 = createFirstColumnBox();
            
            Box firstColumnBox11 = createFirstColumnBox();
            Box firstColumnBox12 = createFirstColumnBox();
            
            Box firstColumnBox13 = createFirstColumnBox();
            Box firstColumnBox14 = createFirstColumnBox();
            
            Box firstColumnBox15 = createFirstColumnBox();
            Box firstColumnBox16 = createFirstColumnBox();
            
            Box firstColumnBox17 = createFirstColumnBox();
            Box firstColumnBox18 = createFirstColumnBox();
            
            actionVBox1.Add(firstColumnBox1);
            actionVBox1.Add(firstColumnBox2);
            actionVBox1.Add(firstColumnBox3);
            actionVBox1.Add(firstColumnBox4);
            actionVBox1.Add(firstColumnBox5);
            actionVBox1.Add(firstColumnBox6);
            actionVBox1.Add(firstColumnBox7);
            actionVBox1.Add(firstColumnBox8);
            actionVBox1.Add(firstColumnBox9);
            actionVBox1.Add(firstColumnBox10);
            actionVBox1.Add(firstColumnBox11);
            actionVBox1.Add(firstColumnBox12);
            actionVBox1.Add(firstColumnBox13);
            actionVBox1.Add(firstColumnBox14);
            actionVBox1.Add(firstColumnBox15);
            actionVBox1.Add(firstColumnBox16);
            actionVBox1.Add(firstColumnBox17);
            actionVBox1.Add(firstColumnBox18);

            testHBox.Add(actionVBox1);
            testHBox.Add(actionVBox2);
            testHBox.Add(actionVBox3);

            vbox.Add(testHBox);
            
            animationInfoHBox.Add(animationInfoLabel);

            animationInfoHBox.Margin = 10;
            
            animationInfoOuterHBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            
            vbox.PackStart(animationInfoOuterHBox, false, false, 0);
            
            animationInfoHBox.Halign = Align.End;
            animationInfoHBox.Valign = Align.End;
            
            animationInfoOuterHBox.Add(animationInfoHBox);
            
            vbox.Add(animationInfoOuterHBox);

            //HBox labelHbox = new HBox();
            //labelHbox.SetSizeRequest(100, 100);
            //Gdk.Color color = new Gdk.Color(255, 0, 0);
            //Label test = new Label("test");
            //labelHbox.Add(test);
            //labelHbox.ModifyBg(Gtk.StateType.Normal, color);
            
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