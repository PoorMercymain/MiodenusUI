using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gdk;
using GLib;
using Gtk;
using MiodenusUI.MafStructure;
using Application = Gtk.Application;
using Menu = Gtk.Menu;
using MenuItem = Gtk.MenuItem;
using UI = Gtk.Builder.ObjectAttribute;
using Window = Gtk.Window;
using WrapMode = Pango.WrapMode;

namespace MiodenusUI
{
    class MainWindow : Window
    {
        private string currentFilePath = "test.txt";
        private Label animationInfoLabel = new Label();
        MenuItem openMenuItem = new MenuItem("Open");
        Button includeButton = new Button("Choose files");
        private Label choosenIncludes = new Label("");
        private Label animationPath = new Label("");
        private Button animationPathButton = new Button("Choose file");
        
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
            CssProvider css = new CssProvider();
            try
            {
                css.LoadFromPath("style.css");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Application.Quit();
            }
            
            Box allWindowElements = new Box(Gtk.Orientation.Vertical, 0);
            
            allWindowElements.StyleContext.AddProvider(css, 0);
            allWindowElements.StyleContext.AddClass("GtkBox");
            
            int firstColumnBoxesAmount = 100;
            
            Gdk.Color mainColor = new Color(90, 103, 131);
            Gdk.Color almostWhite = new Color(245, 244, 242);
            Gdk.Color almostBlack = new Color(35, 42, 62);
            
            MenuItem newMenuItem = new MenuItem("New");
            //MenuItem openMenuItem = new MenuItem("Open");
            MenuItem exitMenuItem = new MenuItem("Exit");

            MenuBar bar = CreateMenuBar(newMenuItem, openMenuItem, exitMenuItem);
            
            allWindowElements.PackStart(bar, false, false, 0);

            builder.Autoconnect(this);
            
            LoaderMaf maf = new LoaderMaf();

            MiodenusUI.MafStructure.Animation animation = new MiodenusUI.MafStructure.Animation();
            animation = maf.Read(currentFilePath);
            
            animationInfoLabel.Text = animation.AnimationInfo.Type + ", " + animation.AnimationInfo.Version + ", " +
                                                 animation.AnimationInfo.Name + ", " + animation.AnimationInfo.Fps + " fps, " +
                                                 animation.AnimationInfo.FrameHeight + "x" +
                                                 animation.AnimationInfo.FrameWidth + ", " + animation.AnimationInfo.TimeLength + " milliseconds, " + 
                                                 animation.AnimationInfo.VideoName + ", " + animation.AnimationInfo.VideoFormat;

            animationInfoLabel.ModifyFg(Gtk.StateType.Normal, almostWhite);

            Box animationInfoOuterHBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box animationInfoHBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box backgroundHBox = new Box(Gtk.Orientation.Horizontal, 0);
            
            ScrolledWindow scrolledTimelinesNames = new ScrolledWindow();

            Box timelinesNamesVBox = new Box(Gtk.Orientation.Vertical, 0);
            Box timelinesVBox = new Box(Gtk.Orientation.Vertical, 0);
            Box propertiesVBox = new Box(Gtk.Orientation.Vertical, 0);
            
            propertiesVBox.StyleContext.AddProvider(css, 1);
            propertiesVBox.StyleContext.AddClass(".box");

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
                
                firstColumnButton.ModifyBg(Gtk.StateType.Normal, mainColor);

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
            
            //scrolledTimelinesNames.Add(scrolledTimelinesElementsBox);
            
            timelinesNamesVBox.Add(scrolledTimelinesElementsBox/*scrolledTimelinesNames*/);
            //timelinesNamesVBox.Fill = false;

            Box timelinesBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box divBox = new Box(Gtk.Orientation.Vertical, 0);
            Box divHBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box macBox = new Box(Gtk.Orientation.Horizontal, 0);

            
            
            macBox.Vexpand = true;
            
            macBox.SetSizeRequest(300,600);
            macBox.Margin = 5;
            
            macBox.ModifyBg(Gtk.StateType.Normal, almostWhite);
            
            //macBox.Vexpand = true;
            
            divBox.Add(macBox);
            
            divHBox.PackStart(timelinesNamesVBox, false, false, 0);
            timelinesVBox.Hexpand = true;
            //timelinesVBox.Vexpand = false;
            divHBox.Add(timelinesVBox);
            
            divBox.Add(divHBox);
            
            timelinesBox.Add(divBox);
            
            scrolledTimelinesNames.Add(timelinesBox);

            //backgroundHBox.PackStart(timelinesNamesVBox, false, false, 0);//testHBox.Add(scroll);//PackStart(scroll, true, true, 0);
            //timelinesVBox.Expand = true;
            //backgroundHBox.Add(timelinesVBox);
            backgroundHBox.Add(scrolledTimelinesNames/*timelinesBox*/);
            backgroundHBox.PackEnd(propertiesVBox,false,false,0);//backgroundHBox.Add(propertiesVBox);

            allWindowElements.Add(backgroundHBox);
            
            animationInfoHBox.Add(animationInfoLabel);

            animationInfoHBox.Margin = 10;
            
            animationInfoHBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            //animationInfoHBox.
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

        private void ChooseFolderButton_Clicked(object sender, EventArgs a)
        {
            var chooseFolderDialog = new FileChooserDialog("Choose folder", this, FileChooserAction.Save, "Cancel",
                ResponseType.Cancel, "Choose", ResponseType.Accept);
            chooseFolderDialog.ShowAll();

            if (chooseFolderDialog.Run()==(int)ResponseType.Accept)
            {
                animationPath.Text = chooseFolderDialog.Filename;
            }
            
            chooseFolderDialog.Destroy();
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

                if (sender == openMenuItem)
                {
                    animationInfoLabel.Text = openedAnimation.AnimationInfo.Type + ", " + openedAnimation.AnimationInfo.Version + ", " +
                                                              openedAnimation.AnimationInfo.Name + ", " + openedAnimation.AnimationInfo.Fps + " fps, " +
                                                              openedAnimation.AnimationInfo.FrameHeight + "x" +
                                                              openedAnimation.AnimationInfo.FrameWidth + ", " + openedAnimation.AnimationInfo.TimeLength + " seconds, " + 
                                                              openedAnimation.AnimationInfo.VideoName + ", " + openedAnimation.AnimationInfo.VideoFormat;
                }
                else if(sender == includeButton)
                {
                    if (choosenIncludes.Text.Length > 0)
                    {
                        choosenIncludes.Text += "\n";
                    }
                    
                    choosenIncludes.Text += openDialog.Filename;
                    
                }
                //else if(sender == animationPathButton)
                //{
                    //animationPath.Text = openDialog.Filename;
                //}
            }

            openDialog.Destroy();
        }

        
        private void CreateButton_Clicked(object sender, EventArgs a)
        {
            int fpsLength = 0;
            
            float[] rgbBackground = new float[3];
            
            var init = new Window("Set up your animation");

            init.SetDefaultSize(500, 500);
            init.Resizable = false;

            Box newFileCreationBox = new Box(Gtk.Orientation.Horizontal, 0);

            Box labelsBox = new Box(Gtk.Orientation.Vertical, 0);

            labelsBox.Margin = 5;

            //Fixed fixedLayout = new Fixed();
            //fixedLayout.SetSizeRequest(500,500);
            
            //Text labels
            Label animationNameLabel = new Label("Name:");
            animationNameLabel.MarginBottom = 6;
            labelsBox.Add(animationNameLabel);
            //fixedLayout.Put(nameLabel, 10, 8);
            
            Label videoNameLabel = new Label("Video name:");
            videoNameLabel.MarginBottom = 6;
            labelsBox.Add(videoNameLabel);
            //fixedLayout.Put(videoNameLabel, 10, 104);

            Label typeLabel = new Label("Type:");
            typeLabel.MarginBottom = 6;
            labelsBox.Add(typeLabel);
            //fixedLayout.Put(typeLabel, 10, 32);
            
            Label versionLabel = new Label("Version:");
            versionLabel.MarginBottom = 6;
            labelsBox.Add(versionLabel);
            //fixedLayout.Put(versionLabel, 10, 56);
            
            Label videoFormatLabel = new Label("Video format:");
            videoFormatLabel.MarginBottom = 7;
            labelsBox.Add(videoFormatLabel);
            //fixedLayout.Put(videoTypeLabel, 10, 80);
            
            Label videoCodecLabel = new Label("Video codec:");
            videoCodecLabel.MarginBottom = 7;
            labelsBox.Add(videoCodecLabel);
            
            Label videoBitrateLabel = new Label("Video bitrate:");
            videoBitrateLabel.MarginBottom = 6;
            labelsBox.Add(videoBitrateLabel);
            
            Label timeLengthLabel = new Label("Video length (milliseconds):");
            timeLengthLabel.MarginBottom = 9;
            labelsBox.Add(timeLengthLabel);
            //fixedLayout.Put(timeLengthLabel, 10, 128);
            
            Label fpsLabel = new Label("FPS:");
            fpsLabel.MarginBottom = 6;
            labelsBox.Add(fpsLabel);
            //fixedLayout.Put(fpsLabel, 10, 156);
            
            Label multisamplingLabel = new Label("Enable multisampling:");
            multisamplingLabel.MarginBottom = 8;
            labelsBox.Add(multisamplingLabel);
            
            Label frameWidthLabel = new Label("Frame width:");
            frameWidthLabel.MarginBottom = 6;
            labelsBox.Add(frameWidthLabel);
            //fixedLayout.Put(frameWidthLabel, 10, 180);

            Label frameHeightLabel = new Label("Frame height:");
            frameHeightLabel.MarginBottom = 20;
            labelsBox.Add(frameHeightLabel);
            //fixedLayout.Put(frameHeightLabel, 10, 204);
            
            Label backgroundColorLabel = new Label("Background color:");
            backgroundColorLabel.MarginBottom = 35;
            labelsBox.Add(backgroundColorLabel);
            
            Label includeLabel = new Label("Include:");
            includeLabel.MarginBottom = 30;
            labelsBox.Add(includeLabel);
            
            Label animationPathLabel = new Label("Animation path:");
            animationPathLabel.MarginBottom = 19;
            labelsBox.Add(animationPathLabel);
            //fixedLayout.Put(animationPathLabel, 10, 228);
            
            newFileCreationBox.Add(labelsBox);

            Box choicesBox = new Box(Gtk.Orientation.Vertical, 0);

            choicesBox.Margin = 5;
            
            //Text views
            /*void SetPlaceholderTextViewSizeAndPosition(TextView placeholderTextView, int positionX, int positionY)
            {
                placeholderTextView.SetSizeRequest(400, 16);            
                fixedLayout.Put(placeholderTextView, positionX, positionY);
            }*/
            
            TextView animationNameTextView = new TextView();
            animationNameTextView.SetSizeRequest(400,16);
            animationNameTextView.MarginBottom = 5;
            choicesBox.Add(animationNameTextView);
            //nameTextView.SetSizeRequest(400, 16);            
            //fixedLayout.Put(nameTextView, 95, 8);
            
            TextView videoNameTextView = new TextView();
            videoNameTextView.SetSizeRequest(400, 16);
            videoNameTextView.MarginBottom = 5;
            choicesBox.Add(videoNameTextView);
            //SetPlaceholderTextViewSizeAndPosition(videoNameTextView, 95, 104);

            TextView typeTextView = new TextView();
            typeTextView.SetSizeRequest(400, 16);
            typeTextView.Buffer.Text = ".maf";
            typeTextView.Editable = false;
            typeTextView.ModifyBg(StateType.Normal, new Color(240,240,240));
            typeTextView.MarginBottom = 5;
            choicesBox.Add(typeTextView);
            //SetPlaceholderTextViewSizeAndPosition(typeTextView, 95, 32);

            TextView versionTextView = new TextView();
            versionTextView.SetSizeRequest(400, 16);
            versionTextView.Buffer.Text = "1.0";
            versionTextView.Editable = false;
            versionTextView.ModifyBg(StateType.Normal, new Color(240,240,240));
            versionTextView.MarginBottom = 5;
            choicesBox.Add(versionTextView);
            //SetPlaceholderTextViewSizeAndPosition(versionTextView, 95, 56);

            TextView videoFormatTextView = new TextView();
            videoFormatTextView.SetSizeRequest(400, 16);
            videoFormatTextView.MarginBottom = 5;
            choicesBox.Add(videoFormatTextView);
            //SetPlaceholderTextViewSizeAndPosition(videoTypeTextView, 95, 80);

            Box codecChoiceBox = new Box(Gtk.Orientation.Horizontal, 0);
            
            RadioButton mpeg4 = new RadioButton("MPEG4");
            RadioButton H264 = new RadioButton(mpeg4, "H.264");
            
            codecChoiceBox.Add(mpeg4);
            codecChoiceBox.Add(H264);
            
            codecChoiceBox.MarginBottom = 5;
            choicesBox.Add(codecChoiceBox);
            
            TextView videoBitrateTextView = new TextView();
            videoBitrateTextView.SetSizeRequest(400, 16);
            videoBitrateTextView.MarginBottom = 5;
            choicesBox.Add(videoBitrateTextView);

            TextView timeLengthTextView = new TextView();
            timeLengthTextView.SetSizeRequest(400, 16);
            timeLengthTextView.MarginBottom = 5;
            choicesBox.Add(timeLengthTextView);
            //SetPlaceholderTextViewSizeAndPosition(timeLengthTextView, 95, 128);
            
            TextView fpsTextView = new TextView();
            fpsTextView.SetSizeRequest(400, 16);
            fpsTextView.MarginBottom = 5;
            choicesBox.Add(fpsTextView);
            //SetPlaceholderTextViewSizeAndPosition(fpsTextView, 95, 156);
            
            Box multisamplingBox = new Box(Gtk.Orientation.Horizontal,0);
            multisamplingBox.SetSizeRequest(400, 16);
            multisamplingBox.MarginBottom = 5;
            RadioButton multisamplingOn = new RadioButton("On");
            RadioButton multisamplingOff = new RadioButton(multisamplingOn, "Off");
            multisamplingBox.Add(multisamplingOn);
            multisamplingBox.Add(multisamplingOff);
            choicesBox.Add(multisamplingBox);
            
            TextView frameWidthTextView = new TextView();
            frameWidthTextView.SetSizeRequest(400, 16);
            frameWidthTextView.MarginBottom = 5;
            choicesBox.Add(frameWidthTextView);
            //SetPlaceholderTextViewSizeAndPosition(frameWidthTextView, 95, 180);
            
            TextView frameHeightTextView = new TextView();
            frameHeightTextView.SetSizeRequest(400, 16);
            frameHeightTextView.MarginBottom = 5;
            choicesBox.Add(frameHeightTextView);
            //SetPlaceholderTextViewSizeAndPosition(frameHeightTextView, 95, 204);
            
            TextView backgroundColorTextViewR = new TextView();
            TextView backgroundColorTextViewG = new TextView();
            TextView backgroundColorTextViewB = new TextView();
            Box backgroundColorComponents = new Box(Gtk.Orientation.Horizontal, 0);
            Box backgroundColorComponentsVBox = new Box(Gtk.Orientation.Vertical, 0);
            backgroundColorComponents.SetSizeRequest(400, 16);
            backgroundColorComponents.Hexpand = true;
            backgroundColorComponents.MarginBottom = 5;
            Button chooseColorButton = new Button("Choose color");
            Label choosenColorRedComponent = new Label("");
            Label choosenColorGreenComponent = new Label("");
            Label choosenColorBlueComponent = new Label("");
            Box choosenColorBox = new Box(Gtk.Orientation.Horizontal,0);
            choosenColorBox.ModifyBg(StateType.Normal, new Color(0,0,0));
            
            rgbBackground[0] = 0;
            rgbBackground[1] = 0;
            rgbBackground[2] = 0;
            
            choosenColorRedComponent.Text = "Red = 0";
            choosenColorGreenComponent.Text = "Green = 0";
            choosenColorBlueComponent.Text = "Blue = 0";
            
            choosenColorRedComponent.MarginEnd = 5;
            choosenColorGreenComponent.MarginEnd = 5;
            choosenColorBlueComponent.MarginEnd = 5;
            
            backgroundColorComponentsVBox.Add(choosenColorRedComponent);
            backgroundColorComponentsVBox.Add(choosenColorGreenComponent);
            backgroundColorComponentsVBox.Add(choosenColorBlueComponent);
            
            choosenColorBox.MarginEnd = 5;
            choosenColorBox.SetSizeRequest(40,10);
            choosenColorBox.Expand = false;
            backgroundColorComponents.Add(backgroundColorComponentsVBox);
            backgroundColorComponents.Add(choosenColorBox);
            backgroundColorComponents.Add(chooseColorButton);
            
            /*backgroundColorTextViewR.SetSizeRequest((400-10)/3, 16);
            backgroundColorTextViewR.MarginEnd = 5;
            backgroundColorTextViewG.SetSizeRequest((400-10)/3, 16);
            backgroundColorTextViewG.MarginEnd = 5;
            backgroundColorTextViewB.SetSizeRequest((400-10)/3, 16);
            backgroundColorComponents.Add(backgroundColorTextViewR);
            backgroundColorComponents.Add(backgroundColorTextViewG);
            backgroundColorComponents.Add(backgroundColorTextViewB);*/
            choicesBox.Add(backgroundColorComponents);
            
            //Button includeButton = new Button("Choose files");
            choosenIncludes.Text = "";
            Box includesBox = new Box(Gtk.Orientation.Horizontal, 0);
            includesBox.SetSizeRequest(400, 16);
            includeButton.SetSizeRequest(50, 16);
            ScrolledWindow scrolledChoosenIncludes = new ScrolledWindow();
            choosenIncludes.SetSizeRequest(300,16);
            scrolledChoosenIncludes.SetSizeRequest(300,16);
            scrolledChoosenIncludes.Add(choosenIncludes);
            includeButton.MarginBottom = 5;
            includesBox.Add(scrolledChoosenIncludes);
            includesBox.Add(includeButton);
            choicesBox.Add(includesBox);
            
            //TextView animationPathTextView = new TextView();

            animationPath.Text = "";
            Box animationPathBox = new Box(Gtk.Orientation.Horizontal, 0);
            animationPathBox.SetSizeRequest(400, 16);
            animationPathButton.SetSizeRequest(50, 16);
            ScrolledWindow scrolledAnimationPath = new ScrolledWindow();
            animationPath.SetSizeRequest(300,16);
            scrolledAnimationPath.SetSizeRequest(300,16);
            scrolledAnimationPath.Add(animationPath);
            animationPathButton.MarginBottom = 5;
            animationPathBox.Add(scrolledAnimationPath);
            animationPathBox.Add(animationPathButton);
            choicesBox.Add(animationPathBox);
            
            //animationPathTextView.SetSizeRequest(400, 16);
            //animationPathTextView.MarginBottom = 5;
            //choicesBox.Add(animationPathTextView);
            //SetPlaceholderTextViewSizeAndPosition(animationPathTextView, 95, 228);

            newFileCreationBox.Add(choicesBox);
            
            //Buttons
            Button okButton = new Button("Ok");
            
            labelsBox.Add(okButton);
            
            //okButton.SetSizeRequest(45, 30);
            //fixedLayout.Put(okButton, 275, 452);

            Button cancelButton = new Button("Cancel");
            
            choicesBox.Add(cancelButton);
            
            //cancelButton.SetSizeRequest(45, 30);
            //fixedLayout.Put(cancelButton, 155, 452);
            
            init.Add(newFileCreationBox);
            init.ShowAll();

            /*if (multisamplingOn.Active)
            {
                Console.WriteLine("multisampling changed");
            }*/

            //multisamplingOn.Act += Multisampling_Changed;
            animationPathButton.Clicked += ChooseFolderButton_Clicked;
            includeButton.Clicked += OpenButton_Clicked;
            okButton.Clicked += OkButton_Clicked;
            cancelButton.Clicked += CancelButton_Clicked;
            chooseColorButton.Clicked += ChooseColorButton_Clicked;
            fpsTextView.Buffer.Changed += CheckInt;
            videoBitrateTextView.Buffer.Changed += CheckInt;
            timeLengthTextView.Buffer.Changed += CheckInt;
            frameWidthTextView.Buffer.Changed += CheckInt;
            frameHeightTextView.Buffer.Changed += CheckInt;

            void Multisampling_Changed(object sender, EventArgs a)
            {
                Console.WriteLine("multisampling changed");
            }

            void ChooseColorButton_Clicked(object sender, EventArgs a)
            {
                ColorChooserDialog chooseColorWindow = new ColorChooserDialog("Choose color", this);
                chooseColorWindow.ShowAll();

                if (chooseColorWindow.Run() == (int)ResponseType.Ok)
                {
                    choosenColorRedComponent.Text = $"Red = {(((float)chooseColorWindow.Rgba.Red)*255).ToString()}";
                    choosenColorGreenComponent.Text = $"Green = {(((float)chooseColorWindow.Rgba.Green)*255).ToString()}";
                    choosenColorBlueComponent.Text = $"Blue = {(((float)chooseColorWindow.Rgba.Blue)*255).ToString()}";

                    choosenColorBox.ModifyBg(StateType.Normal, new Color((byte)(chooseColorWindow.Rgba.Red*255), (byte)(chooseColorWindow.Rgba.Green*255), (byte)(chooseColorWindow.Rgba.Blue*255)));

                    rgbBackground[0] = (float) chooseColorWindow.Rgba.Red;
                    rgbBackground[1] = (float) chooseColorWindow.Rgba.Green;
                    rgbBackground[2] = (float) chooseColorWindow.Rgba.Blue;
                }
                chooseColorWindow.Destroy();
            }

            void OkButton_Clicked(object sender, EventArgs a)
            {
                var animationNew = new Animation();
                animationNew.AnimationInfo.Name = animationNameTextView.Buffer.Text;
                animationNew.AnimationInfo.Type = typeTextView.Buffer.Text;
                animationNew.AnimationInfo.Version = versionTextView.Buffer.Text;
                animationNew.AnimationInfo.VideoFormat = videoFormatTextView.Buffer.Text;

                if (mpeg4.Active)
                {
                    animationNew.AnimationInfo.VideoCodec = "MPEG4";
                }
                else
                {
                    animationNew.AnimationInfo.VideoCodec = "H.264";
                }
                
                //animationNew.AnimationInfo.VideoCodec = videoCodecTextView.Buffer.Text;
                animationNew.AnimationInfo.VideoBitrate = int.Parse(videoBitrateTextView.Buffer.Text);
                animationNew.AnimationInfo.VideoName = videoNameTextView.Buffer.Text;
                animationNew.AnimationInfo.TimeLength = int.Parse(timeLengthTextView.Buffer.Text);
                animationNew.AnimationInfo.Fps = int.Parse(fpsTextView.Buffer.Text);
                animationNew.AnimationInfo.EnableMultisampling = multisamplingOn.Active;
                animationNew.AnimationInfo.FrameWidth = int.Parse(frameWidthTextView.Buffer.Text);
                animationNew.AnimationInfo.FrameHeight = int.Parse(frameHeightTextView.Buffer.Text);
                animationNew.AnimationInfo.BackgroundColor[0] = rgbBackground[0];
                animationNew.AnimationInfo.BackgroundColor[1] = rgbBackground[1];
                animationNew.AnimationInfo.BackgroundColor[2] = rgbBackground[2];

                List<string> choosenIncludesList = new List<string>();
                using (System.IO.StringReader reader = new System.IO.StringReader(choosenIncludes.Text))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        choosenIncludesList.Add(line);
                    }
                }

                animationNew.AnimationInfo.Include = new string[choosenIncludesList.Count];

                for (var i = 0; i < choosenIncludesList.Count; i++)
                {
                    animationNew.AnimationInfo.Include[i]=(choosenIncludesList[i]);
                }

                LoaderMaf mafLoaderNew = new LoaderMaf();
                var mafStr = mafLoaderNew.CreateMafString(animationNew);

                var writerNew = File.CreateText(animationPath.Text);
                writerNew.Write(mafStr);
                writerNew.Close();

                currentFilePath = animationPath.Text;
                animationInfoLabel.Text = animationNew.AnimationInfo.Type + ", " + animationNew.AnimationInfo.Version + ", " +
                                     animationNew.AnimationInfo.Name + ", " + animationNew.AnimationInfo.Fps + " fps, " +
                                     animationNew.AnimationInfo.FrameHeight + "x" +
                                     animationNew.AnimationInfo.FrameWidth + ", " + animationNew.AnimationInfo.TimeLength + " milliseconds, " + 
                                     animationNew.AnimationInfo.VideoName + ", " + animationNew.AnimationInfo.VideoFormat;
                init.Close();
            }

            void CheckInt(object sender, EventArgs eventArgs)
            {
                string parsedString = "";
                
                int inputLength = ((TextBuffer)sender).Text.Length;
                var input = (string) ((TextBuffer)sender).Text.Clone();
                
                foreach (var ch in input)
                {
                    if (Int32.TryParse(ch.ToString(), out _))
                    {
                        parsedString += ch.ToString();
                    }
                }
                
                if (inputLength != parsedString.Length)
                {
                    ((TextBuffer)sender).Text = parsedString;
                }
            }
        
            void CancelButton_Clicked(object sender, EventArgs a)
            {
                init.Close();
            }
        }
    }
}