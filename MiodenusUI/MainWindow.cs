using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
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
        MiodenusUI.MafStructure.Animation animation = new MiodenusUI.MafStructure.Animation();
        LoaderMaf maf = new LoaderMaf();
        private List<Button> modelsNamesButtons = new List<Button>();
        private List<Button> modelsNamesRemoveButtons = new List<Button>();
        private List<Button> addActionButtons = new List<Button>();
        Box timelinesVBox = new Box(Gtk.Orientation.Vertical, 0);
        private string currentFilePath = "test.txt";
        private Label animationInfoLabel = new Label();
        MenuItem openMenuItem = new MenuItem("Open");
        Button includeButton = new Button("Browse");
        private Label choosenIncludes = new Label("");
        private Label animationPath = new Label("");
        private Button animationPathButton = new Button("Browse");
        Box scrolledTimelinesElementsBox = new Box(Gtk.Orientation.Vertical, 0);
        private int dictionaryKey = 0;

        
        Gdk.Color mainColor = new Color(90, 103, 131);
        Gdk.Color almostWhite = new Color(245, 244, 242);
        Gdk.Color almostBlack = new Color(35, 42, 62);
        
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
            
            Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, css, Gtk.StyleProviderPriority.User);
            string cssStyles = "";
            cssStyles = File.ReadAllText("style.css");
            css.LoadFromData(cssStyles);
            
            Box allWindowElements = new Box(Gtk.Orientation.Vertical, 0);

            allWindowElements.StyleContext.AddProvider(css, 0);
            allWindowElements.StyleContext.AddClass("GtkBox");
            
            int firstColumnBoxesAmount = 100;

            MenuItem newMenuItem = new MenuItem("New");
            //MenuItem openMenuItem = new MenuItem("Open");
            MenuItem exitMenuItem = new MenuItem("Exit");

            MenuBar bar = CreateMenuBar(newMenuItem, openMenuItem, exitMenuItem);
            
            allWindowElements.PackStart(bar, false, false, 0);

            builder.Autoconnect(this);
            
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
            Box propertiesVBox = new Box(Gtk.Orientation.Vertical, 0);
            
            propertiesVBox.StyleContext.AddProvider(css, 1);
            propertiesVBox.StyleContext.AddClass(".box");

            timelinesNamesVBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            timelinesVBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            propertiesVBox.ModifyBg(Gtk.StateType.Normal, almostBlack);

            timelinesNamesVBox.Name = "rounded_box";
            timelinesVBox.Name = "rounded_box";
            propertiesVBox.Name = "rounded_box";

            timelinesNamesVBox.Margin = 5;
            timelinesVBox.Margin = 5;
            propertiesVBox.Margin = 5;
            
            backgroundHBox.ModifyBg(Gtk.StateType.Normal, mainColor);

            timelinesNamesVBox.SetSizeRequest(400, 35);
            propertiesVBox.SetSizeRequest(400, 35);

            Button createFirstColumnBox()
            {
                Button firstColumnButton = new Button("Choose model path");
                
                firstColumnButton.Margin = 5;

                //firstColumnButton.ModifyFg(Gtk.StateType.Normal, almostWhite);

                return firstColumnButton;
            }

            List<Button> firstColumnButtons = new List<Button>();
            for(var i = 0;i<1;i++)
            {
                Button firstColumnButton = createFirstColumnBox();
                firstColumnButton.Name = "main_color_button";
                //firstColumnButton.ModifyFg(StateType.Normal, mainColor);
                //firstColumnButton.Child.ModifyFg(StateType.Normal, almostWhite);
                //firstColumnButton.Relief = ReliefStyle.None;
                firstColumnButtons.Add(firstColumnButton);
                scrolledTimelinesElementsBox.Add(firstColumnButtons[^1]);
            }

            scrolledTimelinesElementsBox.Vexpand = true;
            
            timelinesNamesVBox.Add(scrolledTimelinesElementsBox);

            Box timelinesBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box divBox = new Box(Gtk.Orientation.Vertical, 0);
            Box divHBox = new Box(Gtk.Orientation.Horizontal, 0);
            Box macBox = new Box(Gtk.Orientation.Horizontal, 0);

            macBox.Vexpand = true;
            
            macBox.SetSizeRequest(300,600);
            macBox.Margin = 5;
            
            macBox.ModifyBg(Gtk.StateType.Normal, almostWhite);
            
            divBox.Add(macBox);
            
            divHBox.PackStart(timelinesNamesVBox, false, false, 0);
            timelinesVBox.Hexpand = true;
            divHBox.Add(timelinesVBox);
            
            divBox.Add(divHBox);
            
            timelinesBox.Add(divBox);
            
            scrolledTimelinesNames.Add(timelinesBox);
            
            backgroundHBox.Add(scrolledTimelinesNames);
            backgroundHBox.PackEnd(propertiesVBox,false,false,0);

            allWindowElements.Add(backgroundHBox);
            
            animationInfoHBox.Add(animationInfoLabel);

            animationInfoHBox.Margin = 10;
            
            animationInfoHBox.ModifyBg(Gtk.StateType.Normal, almostBlack);
            animationInfoOuterHBox.ModifyBg(Gtk.StateType.Normal, almostBlack);

            allWindowElements.PackEnd(animationInfoOuterHBox, false, false, 0);
            
            animationInfoHBox.Halign = Align.End;

            animationInfoOuterHBox.PackEnd(animationInfoHBox, false, false, 0);
            
            Add(allWindowElements);

            var mafStr = maf.CreateMafString(animation);

            var writer = File.CreateText(currentFilePath);
            writer.Write(mafStr);
            writer.Close();
            
            animation = maf.Read(currentFilePath);
            
            for (var i = 0; i < animation.ModelsInfo.Count; i++)
            {
                Button modelButton = new Button(animation.ModelsInfo[i].Name);
                Button removeModelButton = new Button("Remove");
    
                Box modelAndRemoveButtonBox = new Box(Gtk.Orientation.Horizontal, 0);
                        
                modelsNamesButtons.Add(modelButton);
                modelsNamesRemoveButtons.Add(removeModelButton);
    
                modelsNamesRemoveButtons.Last().Name = "main_color_button";
                modelsNamesRemoveButtons.Last().Margin = 5;
                modelsNamesRemoveButtons.Last().ModifyFg(StateType.Normal, almostWhite);
    
                modelsNamesButtons.Last().Name = "main_color_button";
                modelsNamesButtons.Last().Margin = 5;
                modelsNamesButtons.Last().ModifyFg(StateType.Normal, almostWhite);
                modelsNamesButtons.Last().WidthRequest = 310;
                modelAndRemoveButtonBox.Add(modelsNamesButtons.Last());
                modelAndRemoveButtonBox.PackEnd(modelsNamesRemoveButtons.Last(), false, false, 0);
                scrolledTimelinesElementsBox.Add(modelAndRemoveButtonBox);
                
                modelsNamesRemoveButtons.Last().Data.Add("id", modelsNamesRemoveButtons.Count-1);
                modelsNamesRemoveButtons.Last().Clicked += RemoveModel_Clicked;
                
                Box timelineBox = new Box(Gtk.Orientation.Horizontal, 0);
                Button addActionButton = new Button("+");

                addActionButton.Name = "main_color_button";
                addActionButton.Data.Add("id", modelsNamesRemoveButtons.Last().Data["id"]);
                addActionButton.ModifyFg(StateType.Normal, almostWhite);
                
                if (i == 0)
                {
                    addActionButton.MarginTop = 47;
                }
                else
                {
                    addActionButton.MarginTop = 5;
                }

                addActionButton.MarginStart = 5;
                addActionButton.MarginEnd = 5;
                addActionButton.MarginBottom = 5;
                
                addActionButton.Clicked += AddActionButton_Clicked;

                timelineBox.Add(addActionButton);
                timelinesVBox.Add(timelineBox);
                
                addActionButtons.Add(addActionButton);
            }
            
            this.ShowAll();
            
            DeleteEvent += Window_DeleteEvent;
            exitMenuItem.Activated += Program_Quit;
            newMenuItem.Activated += CreateButton_Clicked;
            openMenuItem.Activated += OpenButton_Clicked;

            for (var i = 0; i < firstColumnButtons.Count; i++)
            {
                firstColumnButtons[i].Clicked += AddModel_Clicked;
            }

            void AddModel_Clicked(object sender, EventArgs a)
            {
                var addModelWindow = new Window("Set up your model");
                
                addModelWindow.SetSizeRequest(500,210);

                addModelWindow.Resizable = false;

                Box addModelWindowAllElements = new Box(Gtk.Orientation.Vertical, 0);
                Box addModelWindowLabelsAndChoices = new Box(Gtk.Orientation.Horizontal, 0);
                Box addModelWindowResponseButtons = new Box(Gtk.Orientation.Horizontal, 0);

                Box addNewModelLabels = new Box(Gtk.Orientation.Vertical, 0);
                Box addNewModelChoices = new Box(Gtk.Orientation.Vertical, 0);

                addModelWindowAllElements.ModifyBg(StateType.Normal, mainColor);
                
                Label newModelNameLabel = new Label("Model name:");
                newModelNameLabel.ModifyFg(StateType.Normal, almostWhite);
                newModelNameLabel.MarginTop = 5;
                newModelNameLabel.MarginBottom = 5;
                newModelNameLabel.MarginStart = 5;
                Label newModelTypeLabel = new Label("Model type:");
                newModelTypeLabel.ModifyFg(StateType.Normal, almostWhite);
                newModelTypeLabel.MarginBottom = 17;
                newModelTypeLabel.MarginStart = 5;
                Label newModelPathLabel = new Label("Model path:");
                newModelPathLabel.ModifyFg(StateType.Normal, almostWhite);
                newModelPathLabel.MarginBottom = 40;
                newModelPathLabel.MarginStart = 5;
                newModelPathLabel.MarginEnd = 5;
                Label newModelColorLabel = new Label("Model color:");
                newModelColorLabel.ModifyFg(StateType.Normal, almostWhite);
                newModelColorLabel.MarginBottom = 23;
                newModelColorLabel.MarginStart = 5;
                Label newModelCalculatedNormalsLabel = new Label("Calculated normals:");
                newModelCalculatedNormalsLabel.ModifyFg(StateType.Normal, almostWhite);
                newModelCalculatedNormalsLabel.MarginBottom = 5;
                newModelCalculatedNormalsLabel.MarginStart = 5;

                addNewModelLabels.Add(newModelNameLabel);
                addNewModelLabels.Add(newModelTypeLabel);
                addNewModelLabels.Add(newModelPathLabel);
                addNewModelLabels.Add(newModelColorLabel);
                addNewModelLabels.Add(newModelCalculatedNormalsLabel);

                TextView newModelName = new TextView();
                newModelName.MarginEnd = 5;
                newModelName.Buffer.Text = DefaultMafParameters.ModelInfo.Name;
                newModelName.MarginBottom = 5;
                newModelName.MarginTop = 5;
                newModelName.ModifyBg(StateType.Normal, new Color(212,224,238));
                addNewModelChoices.Add(newModelName);
                
                TextView newModelType = new TextView();
                newModelType.SetSizeRequest(16,16);
                newModelType.Hexpand = true;
                newModelType.Buffer.Text = DefaultMafParameters.ModelInfo.Type;
                newModelType.ModifyFg(StateType.Normal, almostWhite);
                newModelType.MarginBottom = 5;
                newModelType.ModifyBg(StateType.Normal, mainColor);
                newModelType.Editable = false;
                addNewModelChoices.Add(newModelType);
                
                Box newModelPathBox = new Box(Gtk.Orientation.Horizontal, 0);
                Label newModelPath = new Label(DefaultMafParameters.ModelInfo.Filename);
                newModelPath.ModifyFg(StateType.Normal, almostWhite);
                newModelPath.SetSizeRequest(290,16);
                
                ScrolledWindow newModelPathScrolledWindow = new ScrolledWindow();
                newModelPathScrolledWindow.SetSizeRequest(290,16);
                newModelPathScrolledWindow.Add(newModelPath);
                
                Button newModelPathButton = new Button("Browse");
                newModelPathButton.Name = "light_blue_button";
                
                newModelPathBox.Add(newModelPathScrolledWindow);
                newModelPathBox.PackEnd(newModelPathButton, false, false,0);
                newModelPathButton.MarginEnd = 10;

                newModelPathBox.MarginBottom = 7;
                
                addNewModelChoices.Add(newModelPathBox);
                
                Button chooseColorButton = new Button("Choose color");
                chooseColorButton.Name = "light_blue_button";
                Label choosenColorRedComponent = new Label($"Red = {DefaultMafParameters.AnimationInfo.BackgroundColor[0]*255}");
                Label choosenColorGreenComponent = new Label($"Green = {DefaultMafParameters.AnimationInfo.BackgroundColor[1]*255}");
                Label choosenColorBlueComponent = new Label($"Blue = {DefaultMafParameters.AnimationInfo.BackgroundColor[2]*255}");
                
                choosenColorRedComponent.ModifyFg(StateType.Normal, almostWhite);
                choosenColorGreenComponent.ModifyFg(StateType.Normal, almostWhite);
                choosenColorBlueComponent.ModifyFg(StateType.Normal, almostWhite);

                Box choosenColorBox = new Box(Gtk.Orientation.Horizontal,0);
                choosenColorBox.ModifyBg(StateType.Normal, new Color((byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[0]*255),(byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[1]*255),(byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[2]*255)));

                float[] rgbModelColor = new float[3];
                
                rgbModelColor[0] = DefaultMafParameters.AnimationInfo.BackgroundColor[0];
                rgbModelColor[1] = DefaultMafParameters.AnimationInfo.BackgroundColor[1];
                rgbModelColor[2] = DefaultMafParameters.AnimationInfo.BackgroundColor[2];
                
                choosenColorRedComponent.MarginEnd = 5;
                choosenColorGreenComponent.MarginEnd = 5;
                choosenColorBlueComponent.MarginEnd = 5;

                Box modelColorComponentsVBox = new Box(Gtk.Orientation.Vertical, 0);
                
                modelColorComponentsVBox.Add(choosenColorRedComponent);
                modelColorComponentsVBox.Add(choosenColorGreenComponent);
                modelColorComponentsVBox.Add(choosenColorBlueComponent);
                
                choosenColorBox.MarginEnd = 5;
                choosenColorBox.SetSizeRequest(40,10);
                choosenColorBox.Expand = false;

                Box modelColorComponents = new Box(Gtk.Orientation.Horizontal, 0);
                
                modelColorComponents.Add(modelColorComponentsVBox);
                modelColorComponents.Add(choosenColorBox);
                modelColorComponents.Add(chooseColorButton);

                modelColorComponents.MarginBottom = 5;
                
                addNewModelChoices.Add(modelColorComponents);

                Box calculatedNormalsBox = new Box(Gtk.Orientation.Horizontal, 0);
                RadioButton calculatedNormalsOff = new RadioButton("Don`t use");
                RadioButton calculatedNormalsOn = new RadioButton(calculatedNormalsOff,"Use");
                
                calculatedNormalsOn.ModifyFg(StateType.Normal, almostWhite);
                calculatedNormalsOff.ModifyFg(StateType.Normal, almostWhite);
                
                calculatedNormalsBox.Add(calculatedNormalsOn);
                calculatedNormalsBox.Add(calculatedNormalsOff);
                
                addNewModelChoices.Add(calculatedNormalsBox);
                
                Button addNewModelButtonOk = new Button("Ok");
                Button addNewModelButtonCancel = new Button("Cancel");

                addNewModelButtonCancel.Name = "dark_blue_button";
                addNewModelButtonCancel.Margin = 7;
                addNewModelButtonCancel.Hexpand = true;
                addNewModelButtonCancel.SetSizeRequest(20,35);

                addNewModelButtonOk.Name = "light_blue_button";
                addNewModelButtonOk.Margin = 7;
                addNewModelButtonOk.Hexpand = true;
                addNewModelButtonOk.SetSizeRequest(20,35);
                addNewModelButtonOk.Sensitive = false;
                
                addModelWindowLabelsAndChoices.Add(addNewModelLabels);
                addModelWindowLabelsAndChoices.Add(addNewModelChoices);
                
                addModelWindowResponseButtons.Add(addNewModelButtonOk);
                addModelWindowResponseButtons.Add(addNewModelButtonCancel);
                
                addModelWindowAllElements.Add(addModelWindowLabelsAndChoices);
                addModelWindowAllElements.Add(addModelWindowResponseButtons);
                
                addModelWindow.Add(addModelWindowAllElements);

                addModelWindow.ShowAll();

                newModelPathButton.Clicked += ModelPathButton_Clicked;
                chooseColorButton.Clicked += ChooseColorButton_Clicked;
                newModelName.Buffer.Changed += ModelName_Changed;
                addNewModelButtonOk.Clicked += OkButton_Clicked;
                addNewModelButtonCancel.Clicked += CancelButton_Clicked;

                void CancelButton_Clicked(object sender, EventArgs a)
                {
                    addModelWindow.Close();
                }
                
                void ModelName_Changed(object sender, EventArgs a)
                {
                    if (newModelName.Buffer.Text.Length == 55)
                    {
                        newModelName.Buffer.Text = newModelName.Buffer.Text.Remove(newModelName.Buffer.Text.Length-1, 1);
                    }
                    
                    addNewModelButtonOk.Sensitive = newModelPath.Text != "" && newModelName.Buffer.Text != "";
                }

                void ModelPathButton_Clicked(object sender, EventArgs a)
                {
                    var chooseFolderDialog = new FileChooserDialog("Choose folder and filename", addModelWindow, FileChooserAction.Save, "Cancel",
                        ResponseType.Cancel, "Choose", ResponseType.Accept);
                    chooseFolderDialog.ShowAll();
    
                    if (chooseFolderDialog.Run()==(int)ResponseType.Accept)
                    {
                        newModelPath.Text = chooseFolderDialog.Filename;
                                            
                        if (!(System.IO.Path.GetFileName(newModelPath.Text).Contains('.')))
                        {
                            newModelPath.Text += ".maf";
                        }
                    }

                    addNewModelButtonOk.Sensitive = newModelPath.Text != "" && newModelName.Buffer.Text != "";

                    chooseFolderDialog.Destroy();
                }
                
                void ChooseColorButton_Clicked(object sender, EventArgs a)
                {
                    ColorChooserDialog chooseColorWindow = new ColorChooserDialog("Choose color", addModelWindow);
                    chooseColorWindow.ShowAll();

                    if (chooseColorWindow.Run() == (int)ResponseType.Ok)
                    {
                        choosenColorRedComponent.Text = $"Red = {(((float)chooseColorWindow.Rgba.Red)*255).ToString()}";
                        choosenColorGreenComponent.Text = $"Green = {(((float)chooseColorWindow.Rgba.Green)*255).ToString()}";
                        choosenColorBlueComponent.Text = $"Blue = {(((float)chooseColorWindow.Rgba.Blue)*255).ToString()}";

                        choosenColorBox.ModifyBg(StateType.Normal, new Color((byte)(chooseColorWindow.Rgba.Red*255), (byte)(chooseColorWindow.Rgba.Green*255), (byte)(chooseColorWindow.Rgba.Blue*255)));

                        rgbModelColor[0] = (float) chooseColorWindow.Rgba.Red;
                        rgbModelColor[1] = (float) chooseColorWindow.Rgba.Green;
                        rgbModelColor[2] = (float) chooseColorWindow.Rgba.Blue;
                    }
                    addNewModelButtonOk.Sensitive = newModelPath.Text != "" && newModelName.Buffer.Text != "";
                    chooseColorWindow.Destroy();
                }
                
                void OkButton_Clicked(object sender, EventArgs a)
                {
                    ModelInfo newModel = new ModelInfo();
                    newModel.Name = newModelName.Buffer.Text;
                    newModel.Type = newModelType.Buffer.Text;
                    newModel.Filename = newModelPath.Text;
                    newModel.Color[0] = rgbModelColor[0];
                    newModel.Color[1] = rgbModelColor[1];
                    newModel.Color[2] = rgbModelColor[2];
                    newModel.UseCalculatedNormals = calculatedNormalsOn.Active;
                    
                    animation.ModelsInfo.Add(newModel);

                    Button modelButton = new Button(newModel.Name);
                    Button removeModelButton = new Button("Remove");

                    Box modelAndRemoveButtonBox = new Box(Gtk.Orientation.Horizontal, 0);
                    
                    modelsNamesButtons.Add(modelButton);
                    modelsNamesRemoveButtons.Add(removeModelButton);

                    modelsNamesRemoveButtons.Last().Name = "main_color_button";
                    modelsNamesRemoveButtons.Last().Margin = 5;
                    modelsNamesRemoveButtons.Last().ModifyFg(StateType.Normal, almostWhite);

                    modelsNamesButtons.Last().Name = "main_color_button";
                    modelsNamesButtons.Last().Margin = 5;
                    modelsNamesButtons.Last().ModifyFg(StateType.Normal, almostWhite);
                    modelsNamesButtons.Last().WidthRequest = 310;
                    modelAndRemoveButtonBox.Add(modelsNamesButtons.Last());
                    modelAndRemoveButtonBox.PackEnd(modelsNamesRemoveButtons.Last(), false, false, 0);
                    scrolledTimelinesElementsBox.Add(modelAndRemoveButtonBox);

                    modelsNamesRemoveButtons.Last().Data.Add("id", modelsNamesRemoveButtons.Count-1);
                    modelsNamesRemoveButtons.Last().Clicked += RemoveModel_Clicked;
                    
                    Box timelineBox = new Box(Gtk.Orientation.Horizontal, 0);
                    Button addActionButton = new Button("+");

                    addActionButton.Name = "main_color_button";
                    addActionButton.Data.Add("id", modelsNamesRemoveButtons.Last().Data["id"]);
                    addActionButton.ModifyFg(StateType.Normal, almostWhite);
                
                    if (addActionButtons.Count == 0)
                    {
                        addActionButton.MarginTop = 47;
                    }
                    else
                    {
                        addActionButton.MarginTop = 5;
                    }

                    addActionButton.MarginStart = 5;
                    addActionButton.MarginEnd = 5;
                    addActionButton.MarginBottom = 5;
                    
                    addActionButton.Clicked += AddActionButton_Clicked;

                    timelineBox.Add(addActionButton);
                    timelinesVBox.Add(timelineBox);
                        
                    addActionButtons.Add(addActionButton);
                    
                    ShowAll();

                    LoaderMaf mafLoaderNew = new LoaderMaf();
                    var mafStr = mafLoaderNew.CreateMafString(animation);
    
                    var writerNew = File.CreateText(currentFilePath);
                    writerNew.Write(mafStr);
                    writerNew.Close();
                    
                    addModelWindow.Close();
                }
            }

        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
        
        private void Program_Quit(object sender, EventArgs a)
        {
            Application.Quit();
        }

        private void RemoveModel_Clicked(object sender, EventArgs a)
        {
            int i = modelsNamesRemoveButtons.IndexOf((Button) sender);
            modelsNamesButtons[i].Destroy();
            modelsNamesRemoveButtons[i].Destroy();
            addActionButtons[i].Destroy();
                    
            modelsNamesButtons.Remove(modelsNamesButtons[i]);
            modelsNamesRemoveButtons.Remove(modelsNamesRemoveButtons[i]);
            addActionButtons.Remove(addActionButtons[i]);
            
            if (addActionButtons.Count > 0)
            {
                addActionButtons[0].MarginTop = 47;
            }
            
            animation.ModelsInfo.Remove(animation.ModelsInfo[i]);
                
            var mafStr = maf.CreateMafString(animation);

            var writer = File.CreateText(currentFilePath);
            writer.Write(mafStr);
            writer.Close();

            ShowAll();
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
                    for (var i = 0; i < modelsNamesButtons.Count; i++)
                    {
                        modelsNamesButtons[i].Destroy();
                        modelsNamesRemoveButtons[i].Destroy();
                        addActionButtons[i].Destroy();

                        modelsNamesButtons.Remove(modelsNamesButtons[i]);
                        modelsNamesRemoveButtons.Remove(modelsNamesRemoveButtons[i]);
                        addActionButtons.Remove(addActionButtons[i]);
                    }
                    
                    currentFilePath = openDialog.Filename;
                    animation = maf.Read(currentFilePath);
            
                    for (var i = 0; i < animation.ModelsInfo.Count; i++)
                    {
                        Button modelButton = new Button(animation.ModelsInfo[i].Name);
                        Button removeModelButton = new Button("Remove");
    
                        Box modelAndRemoveButtonBox = new Box(Gtk.Orientation.Horizontal, 0);
                        
                        modelsNamesButtons.Add(modelButton);
                        modelsNamesRemoveButtons.Add(removeModelButton);
    
                        modelsNamesRemoveButtons.Last().Name = "main_color_button";
                        modelsNamesRemoveButtons.Last().Margin = 5;
                        modelsNamesRemoveButtons.Last().ModifyFg(StateType.Normal, almostWhite);
    
                        modelsNamesButtons.Last().Name = "main_color_button";
                        modelsNamesButtons.Last().Margin = 5;
                        modelsNamesButtons.Last().ModifyFg(StateType.Normal, almostWhite);
                        modelsNamesButtons.Last().WidthRequest = 310;
                        modelAndRemoveButtonBox.Add(modelsNamesButtons.Last());
                        modelAndRemoveButtonBox.PackEnd(modelsNamesRemoveButtons.Last(), false, false, 0);
                        scrolledTimelinesElementsBox.Add(modelAndRemoveButtonBox);
                    
                        modelsNamesRemoveButtons.Last().Data.Add("id", modelsNamesRemoveButtons.Count-1);
                        modelsNamesRemoveButtons.Last().Clicked += RemoveModel_Clicked;
                        
                        Box timelineBox = new Box(Gtk.Orientation.Horizontal, 0);
                        Button addActionButton = new Button("+");

                        addActionButton.Name = "main_color_button";
                        addActionButton.Data.Add("id", modelsNamesRemoveButtons.Last().Data["id"]);
                        addActionButton.ModifyFg(StateType.Normal, almostWhite);
                
                        if (i == 0)
                        {
                            addActionButton.MarginTop = 47;
                        }
                        else
                        {
                            addActionButton.MarginTop = 5;
                        }

                        addActionButton.MarginStart = 5;
                        addActionButton.MarginEnd = 5;
                        addActionButton.MarginBottom = 5;

                        addActionButton.Clicked += AddActionButton_Clicked;
                
                        timelineBox.Add(addActionButton);
                        timelinesVBox.Add(timelineBox);
                        
                        addActionButtons.Add(addActionButton);
                    }
            
                    ShowAll();
                }
                else if(sender == includeButton)
                {
                    if (choosenIncludes.Text.Length > 0)
                    {
                        choosenIncludes.Text += "\n";
                    }
                    
                    choosenIncludes.Text += openDialog.Filename;
                    
                }
            }

            openDialog.Destroy();
        }
        
        
        
        private int actionStatesCounter = 0;

        private void AddActionButton_Clicked(object sender, EventArgs a)
        {
            Box addActionWindowLabels = new Box(Gtk.Orientation.Vertical, 0);
            Box addActionWindowResponses = new Box(Gtk.Orientation.Vertical, 0);
            List<float[]> actionStatesColors = new List<float[]>();
            List<TextView> actionStateTimeTextViews = new List<TextView>();
            List<RadioButton> actionStateIsModelVisibleRadioButtons = new List<RadioButton>();

            Window addActionWindow = new Window("Set up your action");
            ScrolledWindow addActionWindowAllElementsScrolled = new ScrolledWindow();
            Box addActionWindowAllElements = new Box(Gtk.Orientation.Horizontal, 0);
            
            addActionWindow.ModifyBg(StateType.Normal, mainColor);

            addActionWindow.WidthRequest = 611;
            addActionWindow.HeightRequest = 115;
            addActionWindowAllElements.Add(addActionWindowLabels);
            addActionWindowAllElements.Add(addActionWindowResponses);
            
            addActionWindowAllElementsScrolled.Add(addActionWindowAllElements);
            addActionWindow.Add(addActionWindowAllElementsScrolled);

            Label actionNameLabel = new Label("Action name:");
            actionNameLabel.ModifyFg(StateType.Normal, almostWhite);
            Box actionNameLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
            actionNameLabelBox.Add(actionNameLabel);
            addActionWindowLabels.Add(actionNameLabelBox);
            actionNameLabelBox.MarginStart = 5;
            actionNameLabelBox.MarginTop = 5;
            actionNameLabelBox.MarginEnd = 80;

            TextView actionNameTextView = new TextView();
            actionNameTextView.SetSizeRequest(450, 16);
            actionNameTextView.MarginEnd = 6;
            actionNameTextView.MarginTop = 5;
            addActionWindowResponses.Add(actionNameTextView);

            Label actionStatesLabel = new Label("Action states:");
            actionStatesLabel.ModifyFg(StateType.Normal, almostWhite);
            Box actionStatesLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
            actionStatesLabelBox.Add(actionStatesLabel);
            addActionWindowLabels.Add(actionStatesLabelBox);
            actionStatesLabelBox.MarginStart = 5;
            actionStatesLabelBox.MarginTop = 15;
            actionStatesLabelBox.MarginEnd = 5;

            Button addNewActionStateButton = new Button("Add");
            addNewActionStateButton.Name = "light_blue_button";
            addNewActionStateButton.Data.Add("id", actionStatesCounter);
            addNewActionStateButton.MarginTop = 5;
            addNewActionStateButton.MarginEnd = 6;

            Button addActionOkButton = new Button("Ok");
            addActionOkButton.Name = "light_blue_button";
            addActionOkButton.MarginTop = 20;
            addActionOkButton.MarginBottom = 5;
            addActionOkButton.MarginStart = 5;
            addActionWindowLabels.PackEnd(addActionOkButton, false, false, 0);
            
            Button addActionCancelButton = new Button("Cancel");
            addActionCancelButton.Name = "dark_blue_button";
            addActionCancelButton.MarginTop = 20;
            addActionCancelButton.MarginBottom = 5;
            addActionCancelButton.MarginStart = 5;
            addActionWindowResponses.PackEnd(addActionCancelButton, false, false, 0);

            addActionWindow.Resizable = false;
            addNewActionStateButton.Clicked += AddActionState_Clicked;
            addActionCancelButton.Clicked += AddActionCancel_Clicked;
            
            
            addActionWindowResponses.Add(addNewActionStateButton);

            void AddActionCancel_Clicked(object sender, EventArgs a)
            {
                addActionWindow.Close();
            }

            List<ActionState> statesBuffer = new List<ActionState>();

            void AddActionState_Clicked(object sender, EventArgs a)
            {
                actionStatesCounter++;
                
                addActionWindow.HeightRequest = 480;

                ActionState actionStateBuffer = new ActionState();
                
                if (actionStatesCounter == 1)
                {
                    Label actionStateTimeLabel = new Label("Action state time:");
                    actionStateTimeLabel.ModifyFg(StateType.Normal, almostWhite);
                    Box actionStateTimeLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                    actionStateTimeLabelBox.Add(actionStateTimeLabel);
                    actionStateTimeLabelBox.MarginStart = 5;
                    actionStateTimeLabelBox.MarginTop = 15;
                    actionStateTimeLabelBox.MarginEnd = 5;
                    addActionWindowLabels.Add(actionStateTimeLabelBox);
                    
                    TextView actionStateTimeTextView = new TextView();
                    actionStateTimeTextView.SetSizeRequest(346, 16);
                    actionStateTimeTextView.MarginTop = 7;
                    actionStateTimeTextView.MarginEnd = 6;
                    actionStateTimeTextView.Data.Add("id", actionStatesCounter-1);
                    actionStateTimeTextViews.Add(actionStateTimeTextView);
                    addActionWindowResponses.Add(actionStateTimeTextView);
                    actionStateTimeTextView.Buffer.Changed += CheckInt;
                }
                else
                {
                    Label actionStateTimeLabel = new Label("Action state time:");
                    actionStateTimeLabel.ModifyFg(StateType.Normal, almostWhite);
                    Box actionStateTimeLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                    actionStateTimeLabelBox.Add(actionStateTimeLabel);
                    actionStateTimeLabelBox.MarginStart = 5;
                    actionStateTimeLabelBox.MarginTop = 15;
                    actionStateTimeLabelBox.MarginEnd = 5;
                    addActionWindowLabels.Add(actionStateTimeLabelBox);
                    
                    TextView actionStateTimeTextView = new TextView();
                    actionStateTimeTextView.SetSizeRequest(346, 16);
                    actionStateTimeTextView.MarginEnd = 7;
                    actionStateTimeTextView.MarginTop = 14;
                    actionStateTimeTextView.Data.Add("id", actionStatesCounter-1);
                    actionStateTimeTextViews.Add(actionStateTimeTextView);
                    addActionWindowResponses.Add(actionStateTimeTextView);
                    actionStateTimeTextView.Buffer.Changed += CheckInt;
                }

                

                Label actionStateModelVisibilityLabel = new Label("Model visibility:");
                Box actionStateModelVisibilityLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                actionStateModelVisibilityLabel.ModifyFg(StateType.Normal, almostWhite);
                actionStateModelVisibilityLabelBox.Add(actionStateModelVisibilityLabel);
                actionStateModelVisibilityLabelBox.MarginStart = 5;
                actionStateModelVisibilityLabelBox.MarginTop = 5;
                actionStateModelVisibilityLabelBox.MarginEnd = 10;
                actionStateModelVisibilityLabelBox.MarginBottom = 15;
                addActionWindowLabels.Add(actionStateModelVisibilityLabelBox);
                
                Box actionStateModelVisibilityBox = new Box(Gtk.Orientation.Horizontal, 0);
                actionStateModelVisibilityBox.MarginTop = 5;
                actionStateModelVisibilityBox.MarginBottom = 5;
                RadioButton actionStateModelVisibilityOn = new RadioButton("On");
                actionStateModelVisibilityOn.ModifyFg(StateType.Normal, almostWhite);
                RadioButton actionStateModelVisibilityOff = new RadioButton(actionStateModelVisibilityOn, "Off");
                actionStateModelVisibilityOff.ModifyFg(StateType.Normal, almostWhite);
                actionStateModelVisibilityBox.Add(actionStateModelVisibilityOn);
                actionStateIsModelVisibleRadioButtons.Add(actionStateModelVisibilityOn);
                actionStateModelVisibilityBox.Add(actionStateModelVisibilityOff);
                addActionWindowResponses.Add(actionStateModelVisibilityBox);
                actionStateModelVisibilityBox.MarginTop = 4;
                
                float[] rgbModelColor = new float[3];

                Label actionStateColorLabel = new Label("Model color:");
                Box actionStateColorLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                actionStateColorLabel.ModifyFg(StateType.Normal, almostWhite);
                actionStateColorLabelBox.Add(actionStateColorLabel);
                actionStateColorLabelBox.MarginTop = 7;
                actionStateColorLabelBox.MarginStart = 5;
                addActionWindowLabels.Add(actionStateColorLabelBox);
                
                Box backgroundColorComponents = new Box(Gtk.Orientation.Horizontal, 0);
                Box backgroundColorComponentsVBox = new Box(Gtk.Orientation.Vertical, 0);
                backgroundColorComponents.SetSizeRequest(400, 16);
                backgroundColorComponents.Hexpand = true;
                backgroundColorComponents.MarginBottom = 5;
                Button chooseColorButton = new Button("Choose color");
                chooseColorButton.Data.Add("id", actionStatesCounter-1);
                chooseColorButton.Name = "light_blue_button";
                Label choosenColorRedComponent = new Label("");
                Label choosenColorGreenComponent = new Label("");
                Label choosenColorBlueComponent = new Label("");
                
                choosenColorRedComponent.ModifyFg(StateType.Normal, almostWhite);
                choosenColorGreenComponent.ModifyFg(StateType.Normal, almostWhite);
                choosenColorBlueComponent.ModifyFg(StateType.Normal, almostWhite);

                Box choosenColorBox = new Box(Gtk.Orientation.Horizontal,0);
                choosenColorBox.ModifyBg(StateType.Normal, new Color((byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[0]*255),(byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[1]*255),(byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[2]*255)));


                rgbModelColor[0] = DefaultMafParameters.AnimationInfo.BackgroundColor[0];
                rgbModelColor[1] = DefaultMafParameters.AnimationInfo.BackgroundColor[1];
                rgbModelColor[2] = DefaultMafParameters.AnimationInfo.BackgroundColor[2];
                
                if (actionStatesColors.Count > (int) (chooseColorButton).Data["id"])
                {
                    actionStatesColors[(int) (chooseColorButton).Data["id"]] = rgbModelColor;
                }
                else
                {
                    actionStatesColors.Add(rgbModelColor);
                }
                
                choosenColorRedComponent.Text = $"Red = {DefaultMafParameters.AnimationInfo.BackgroundColor[0]*255}";
                choosenColorGreenComponent.Text = $"Green = {DefaultMafParameters.AnimationInfo.BackgroundColor[1]*255}";
                choosenColorBlueComponent.Text = $"Blue = {DefaultMafParameters.AnimationInfo.BackgroundColor[2]*255}";
                
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
                
                addActionWindowResponses.Add(backgroundColorComponents);

                Label resetScaleLabel = new Label("Reset scale:");
                Box resetScaleLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                resetScaleLabel.ModifyFg(StateType.Normal, almostWhite);
                resetScaleLabelBox.Add(resetScaleLabel);
                resetScaleLabelBox.MarginStart = 5;
                resetScaleLabelBox.MarginTop = 24;
                resetScaleLabelBox.MarginEnd = 7;
                addActionWindowLabels.Add(resetScaleLabelBox);

                Box resetScaleBox = new Box(Gtk.Orientation.Horizontal, 0);
                resetScaleBox.MarginBottom = 5;
                addActionWindowResponses.Add(resetScaleBox);

                RadioButton resetScaleNo = new RadioButton("No");
                resetScaleNo.ModifyFg(StateType.Normal, almostWhite);
                RadioButton resetScaleYes = new RadioButton(resetScaleNo, "Yes");
                resetScaleYes.ModifyFg(StateType.Normal, almostWhite);
                resetScaleYes.Data.Add("id", actionStatesCounter-1);
                resetScaleBox.Add(resetScaleYes);
                resetScaleBox.Add(resetScaleNo);

                Label transformationScale = new Label("Scale:");
                Box transformationScaleLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                transformationScale.ModifyFg(StateType.Normal, almostWhite);
                transformationScaleLabelBox.Add(transformationScale);
                transformationScale.MarginStart = 5;
                transformationScale.MarginTop = 5;
                transformationScale.MarginEnd = 5;
                addActionWindowLabels.Add(transformationScaleLabelBox);

                Box transformationScaleBox = new Box(Gtk.Orientation.Horizontal, 0);
                addActionWindowResponses.Add(transformationScaleBox);

                TextView transformationScaleX = new TextView();
                transformationScaleX.WidthRequest = 146;
                transformationScaleX.MarginEnd = 5;
                transformationScaleX.Buffer.Changed += CheckFloat;
                transformationScaleX.Data.Add("id", actionStatesCounter-1);
                TextView transformationScaleY = new TextView();
                transformationScaleY.WidthRequest = 146;
                transformationScaleY.MarginEnd = 5;
                transformationScaleX.Buffer.Changed += CheckFloat;
                transformationScaleY.Data.Add("id", actionStatesCounter-1);
                TextView transformationScaleZ = new TextView();
                transformationScaleZ.WidthRequest = 146;
                transformationScaleX.Buffer.Changed += CheckFloat;
                transformationScaleZ.Data.Add("id", actionStatesCounter-1);

                transformationScaleBox.Add(transformationScaleX);
                transformationScaleBox.Add(transformationScaleY);
                transformationScaleBox.Add(transformationScaleZ);

                Label resetLocalRotationLabel = new Label("Reset local rotation:");
                Box resetLocalRotationLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                resetLocalRotationLabel.ModifyFg(StateType.Normal, almostWhite);
                resetLocalRotationLabelBox.Add(resetLocalRotationLabel);
                resetLocalRotationLabelBox.MarginTop = 7;
                resetLocalRotationLabelBox.MarginStart = 5;
                addActionWindowLabels.Add(resetLocalRotationLabelBox);
                
                Box resetLocalRotationBox = new Box(Gtk.Orientation.Horizontal, 0);
                resetLocalRotationBox.MarginTop = 2;
                addActionWindowResponses.Add(resetLocalRotationBox);

                RadioButton resetLocalRotationNo = new RadioButton("No");
                resetLocalRotationNo.ModifyFg(StateType.Normal, almostWhite);
                RadioButton resetLocalRotationYes = new RadioButton(resetLocalRotationNo, "Yes");
                resetLocalRotationYes.ModifyFg(StateType.Normal, almostWhite);
                resetLocalRotationYes.Data.Add("id", actionStatesCounter-1);
                resetLocalRotationBox.Add(resetLocalRotationYes);
                resetLocalRotationBox.Add(resetLocalRotationNo);

                Label localRotationAngleLabel = new Label("Local rotation angle:");
                Box localRotationAngleLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                localRotationAngleLabel.ModifyFg(StateType.Normal, almostWhite);
                localRotationAngleLabelBox.Add(localRotationAngleLabel);
                localRotationAngleLabel.MarginStart = 5;
                localRotationAngleLabel.MarginTop = 7;
                localRotationAngleLabel.MarginEnd = 8;
                addActionWindowLabels.Add(localRotationAngleLabelBox);

                TextView localRotationAngleTextView = new TextView();
                localRotationAngleTextView.MarginTop = 5;
                localRotationAngleTextView.MarginEnd = 8;
                localRotationAngleTextView.Data.Add("id", actionStatesCounter-1);
                localRotationAngleTextView.Buffer.Changed += CheckFloat;
                addActionWindowResponses.Add(localRotationAngleTextView);
                
                Label localRotationUnitsLabel = new Label("Local rotation units:");
                Box localRotationUnitsLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                localRotationUnitsLabel.ModifyFg(StateType.Normal, almostWhite);
                localRotationUnitsLabelBox.Add(localRotationUnitsLabel);
                localRotationUnitsLabel.MarginStart = 5;
                localRotationUnitsLabel.MarginTop = 15;
                localRotationUnitsLabel.MarginEnd = 5;
                addActionWindowLabels.Add(localRotationUnitsLabelBox);

                string[] availableAngleUnits = new[]
                {
                    "rad",
                    "deg"
                };

                ComboBox angleUnitsChooser = new ComboBox(availableAngleUnits);
                Box angleUnitsChooserBox = new Box(Gtk.Orientation.Horizontal, 0);
                angleUnitsChooserBox.MarginTop = 5;
                angleUnitsChooser.Active = 1;
                angleUnitsChooser.Data.Add("id",actionStatesCounter-1);
                angleUnitsChooserBox.Add(angleUnitsChooser);
                addActionWindowResponses.Add(angleUnitsChooserBox);
                
                Label localRotationVectorLabel = new Label("Local rotation vector:");
                Box localRotationVectorLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                localRotationVectorLabel.ModifyFg(StateType.Normal, almostWhite);
                localRotationVectorLabelBox.Add(localRotationVectorLabel);
                localRotationVectorLabel.MarginStart = 5;
                localRotationVectorLabel.MarginTop = 15;
                localRotationVectorLabel.MarginEnd = 5;
                addActionWindowLabels.Add(localRotationVectorLabelBox);

                Box localRotationVectorBox = new Box(Gtk.Orientation.Horizontal, 0);
                localRotationVectorBox.MarginTop = 5;
                addActionWindowResponses.Add(localRotationVectorBox);

                TextView localRotationVectorX = new TextView();
                localRotationVectorX.WidthRequest = 146;
                localRotationVectorX.MarginEnd = 5;
                localRotationVectorX.Buffer.Changed += CheckFloat;
                localRotationVectorX.Data.Add("id", actionStatesCounter-1);
                TextView localRotationVectorY = new TextView();
                localRotationVectorY.WidthRequest = 146;
                localRotationVectorY.MarginEnd = 5;
                localRotationVectorY.Buffer.Changed += CheckFloat;
                localRotationVectorX.Data.Add("id", actionStatesCounter-1);
                TextView localRotationVectorZ = new TextView();
                localRotationVectorZ.WidthRequest = 146;
                localRotationVectorZ.Buffer.Changed += CheckFloat;

                localRotationVectorBox.Add(localRotationVectorX);
                localRotationVectorBox.Add(localRotationVectorY);
                localRotationVectorBox.Add(localRotationVectorZ);
                
                Label resetPositionLabel = new Label("Reset position:");
                Box resetPositionLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                resetPositionLabel.ModifyFg(StateType.Normal, almostWhite);
                resetPositionLabelBox.Add(resetPositionLabel);
                resetPositionLabel.MarginStart = 5;
                resetPositionLabel.MarginTop = 8;
                resetPositionLabel.MarginBottom = 5;
                addActionWindowLabels.Add(resetPositionLabelBox);
                
                Box resetPositionBox = new Box(Gtk.Orientation.Horizontal, 0);
                resetPositionBox.MarginTop = 5;
                resetPositionBox.MarginBottom = 5;
                addActionWindowResponses.Add(resetPositionBox);

                RadioButton resetPositionNo = new RadioButton("No");
                resetPositionNo.ModifyFg(StateType.Normal, almostWhite);
                RadioButton resetPositionYes = new RadioButton(resetPositionNo, "Yes");
                resetPositionYes.ModifyFg(StateType.Normal, almostWhite);
                resetPositionBox.Add(resetPositionYes);
                resetPositionBox.Add(resetPositionNo);
                
                Label globalMoveLabel = new Label("Global move:");
                Box globalMoveLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                globalMoveLabel.ModifyFg(StateType.Normal, almostWhite);
                globalMoveLabelBox.Add(globalMoveLabel);
                globalMoveLabel.MarginStart = 5;
                globalMoveLabel.MarginTop = 2;
                globalMoveLabel.MarginEnd = 5;
                addActionWindowLabels.Add(globalMoveLabelBox);

                Box globalMoveBox = new Box(Gtk.Orientation.Horizontal, 0);
                addActionWindowResponses.Add(globalMoveBox);

                TextView globalMoveX = new TextView();
                globalMoveX.WidthRequest = 146;
                globalMoveX.MarginEnd = 5;
                TextView globalMoveY = new TextView();
                globalMoveY.WidthRequest = 146;
                globalMoveY.MarginEnd = 5;
                TextView globalMoveZ = new TextView();
                globalMoveZ.WidthRequest = 146;
                
                globalMoveBox.Add(globalMoveX);
                globalMoveBox.Add(globalMoveY);
                globalMoveBox.Add(globalMoveZ);
                
                Label localMoveLabel = new Label("Local move:");
                Box localMoveLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                localMoveLabel.ModifyFg(StateType.Normal, almostWhite);
                localMoveLabelBox.Add(localMoveLabel);
                localMoveLabel.MarginStart = 5;
                localMoveLabel.MarginTop = 5;
                localMoveLabel.MarginEnd = 5;
                addActionWindowLabels.Add(localMoveLabelBox);

                Box localMoveBox = new Box(Gtk.Orientation.Horizontal, 0);
                localMoveBox.MarginTop = 5;
                addActionWindowResponses.Add(localMoveBox);

                TextView localMoveX = new TextView();
                localMoveX.WidthRequest = 146;
                localMoveX.MarginEnd = 5;
                TextView localMoveY = new TextView();
                localMoveY.WidthRequest = 146;
                localMoveY.MarginEnd = 5;
                TextView localMoveZ = new TextView();
                localMoveZ.WidthRequest = 146;
                
                localMoveBox.Add(localMoveX);
                localMoveBox.Add(localMoveY);
                localMoveBox.Add(localMoveZ);
                
                Label rotationAngleLabel = new Label("Rotation angle:");
                Box rotationAngleLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                rotationAngleLabel.ModifyFg(StateType.Normal, almostWhite);
                rotationAngleLabelBox.Add(rotationAngleLabel);
                rotationAngleLabel.MarginStart = 5;
                rotationAngleLabel.MarginTop = 5;
                rotationAngleLabel.MarginEnd = 5;
                addActionWindowLabels.Add(rotationAngleLabelBox);
                
                TextView rotationAngleTextView = new TextView();
                rotationAngleTextView.MarginEnd = 5;
                rotationAngleTextView.MarginTop = 5;
                rotationAngleTextView.MarginEnd = 8;
                addActionWindowResponses.Add(rotationAngleTextView);
                
                Label rotationUnitsLabel = new Label("Rotation units:");
                Box rotationUnitsLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                rotationUnitsLabel.ModifyFg(StateType.Normal, almostWhite);
                rotationUnitsLabelBox.Add(rotationUnitsLabel);
                rotationUnitsLabel.MarginStart = 5;
                rotationUnitsLabel.MarginTop = 15;
                rotationUnitsLabel.MarginEnd = 5;
                addActionWindowLabels.Add(rotationUnitsLabelBox);

                ComboBox rotationAngleUnitsChooser = new ComboBox(availableAngleUnits);
                Box rotationAngleUnitsChooserBox = new Box(Gtk.Orientation.Horizontal, 0);
                rotationAngleUnitsChooser.Active = 1;
                rotationAngleUnitsChooserBox.MarginTop = 5;
                rotationAngleUnitsChooserBox.Add(rotationAngleUnitsChooser);
                addActionWindowResponses.Add(rotationAngleUnitsChooserBox);
                
                Label rotationVectorStartLabel = new Label("Rotation vector start point:");
                Box rotationVectorStartLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                rotationVectorStartLabel.ModifyFg(StateType.Normal, almostWhite);
                rotationVectorStartLabelBox.Add(rotationVectorStartLabel);
                rotationVectorStartLabel.MarginStart = 5;
                rotationVectorStartLabel.MarginTop = 14;
                rotationVectorStartLabel.MarginEnd = 5;
                addActionWindowLabels.Add(rotationVectorStartLabelBox);

                Box rotationVectorStartBox = new Box(Gtk.Orientation.Horizontal, 0);
                rotationVectorStartBox.MarginTop = 5;
                addActionWindowResponses.Add(rotationVectorStartBox);

                TextView rotationVectorStartX = new TextView();
                rotationVectorStartX.WidthRequest = 146;
                rotationVectorStartX.MarginEnd = 5;
                TextView rotationVectorStartY = new TextView();
                rotationVectorStartY.WidthRequest = 146;
                rotationVectorStartY.MarginEnd = 5;
                TextView rotationVectorStartZ = new TextView();
                rotationVectorStartZ.WidthRequest = 146;
                
                rotationVectorStartBox.Add(rotationVectorStartX);
                rotationVectorStartBox.Add(rotationVectorStartY);
                rotationVectorStartBox.Add(rotationVectorStartZ);
                
                Label rotationVectorEndLabel = new Label("Rotation vector end point:");
                Box rotationVectorEndLabelBox = new Box(Gtk.Orientation.Horizontal, 0);
                rotationVectorEndLabel.ModifyFg(StateType.Normal, almostWhite);
                rotationVectorEndLabelBox.Add(rotationVectorEndLabel);
                rotationVectorEndLabel.MarginStart = 5;
                rotationVectorEndLabel.MarginTop = 5;
                rotationVectorEndLabel.MarginEnd = 5;
                addActionWindowLabels.Add(rotationVectorEndLabelBox);

                Box rotationVectorEndBox = new Box(Gtk.Orientation.Horizontal, 0);
                rotationVectorEndBox.MarginTop = 5;
                addActionWindowResponses.Add(rotationVectorEndBox);

                TextView rotationVectorEndX = new TextView();
                rotationVectorEndX.WidthRequest = 146;
                rotationVectorEndX.MarginEnd = 5;
                TextView rotationVectorEndY = new TextView();
                rotationVectorEndY.WidthRequest = 146;
                rotationVectorEndY.MarginEnd = 5;
                TextView rotationVectorEndZ = new TextView();
                rotationVectorEndZ.WidthRequest = 146;
                
                rotationVectorEndBox.Add(rotationVectorEndX);
                rotationVectorEndBox.Add(rotationVectorEndY);
                rotationVectorEndBox.Add(rotationVectorEndZ);
                
                chooseColorButton.Clicked += ChooseColorButton_Clicked; 
                
                addActionWindowLabels.ShowAll();
                addActionWindowResponses.ShowAll();
                
                void ChooseColorButton_Clicked(object sender, EventArgs a)
                {
                    ColorChooserDialog chooseColorWindow = new ColorChooserDialog("Choose color", addActionWindow);
                    chooseColorWindow.ShowAll();
                
                    if (chooseColorWindow.Run() == (int)ResponseType.Ok)
                    {
                        choosenColorRedComponent.Text = $"Red = {(((float)chooseColorWindow.Rgba.Red)*255).ToString()}";
                        choosenColorGreenComponent.Text = $"Green = {(((float)chooseColorWindow.Rgba.Green)*255).ToString()}";
                        choosenColorBlueComponent.Text = $"Blue = {(((float)chooseColorWindow.Rgba.Blue)*255).ToString()}";
               
                        choosenColorBox.ModifyBg(StateType.Normal, new Color((byte)(chooseColorWindow.Rgba.Red*255), (byte)(chooseColorWindow.Rgba.Green*255), (byte)(chooseColorWindow.Rgba.Blue*255)));
               
                        rgbModelColor[0] = (float) chooseColorWindow.Rgba.Red;
                        rgbModelColor[1] = (float) chooseColorWindow.Rgba.Green;
                        rgbModelColor[2] = (float) chooseColorWindow.Rgba.Blue;
                    }

                    if (actionStatesColors.Count > (int) ((Button) sender).Data["id"])
                    {
                        actionStatesColors[(int) ((Button) sender).Data["id"]] = rgbModelColor;
                    }
                    else
                    {
                        actionStatesColors.Add(rgbModelColor); 
                    }
                    chooseColorWindow.Destroy();
                }
                
            }
            addActionWindow.ShowAll();
        }

        

        private void CreateButton_Clicked(object sender, EventArgs a)
        {
            includeButton.Name = "browse_button";
            animationPathButton.Name = "browse_button";
            
            int fpsLength = 0;
            
            float[] rgbBackground = new float[3];
            
            var init = new Window("Set up your animation");

            init.SetDefaultSize(500, 500);
            init.Resizable = false;

            Box newFileCreationBox = new Box(Gtk.Orientation.Horizontal, 0);
            
            newFileCreationBox.ModifyBg(StateType.Normal, new Color(90, 103, 131));

            Box labelsBox = new Box(Gtk.Orientation.Vertical, 0);

            labelsBox.Margin = 5;

            //Text labels
            Label animationNameLabel = new Label("Animation name:");
            animationNameLabel.MarginBottom = 17;
            animationNameLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(animationNameLabel);
            
            Label videoNameLabel = new Label("Video path:");
            videoNameLabel.MarginBottom = 20;
            videoNameLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(videoNameLabel);

            Label typeLabel = new Label("Type:");
            typeLabel.MarginBottom = 6;
            typeLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(typeLabel);
            
            Label versionLabel = new Label("Version:");
            versionLabel.MarginBottom = 16;
            versionLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(versionLabel);
            
            Label videoFormatLabel = new Label("Video format:");
            videoFormatLabel.MarginBottom = 23;
            videoFormatLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(videoFormatLabel);
            
            Label videoCodecLabel = new Label("Video codec:");
            videoCodecLabel.MarginBottom = 14;
            videoCodecLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(videoCodecLabel);
            
            Label videoBitrateLabel = new Label("Video bitrate:");
            videoBitrateLabel.MarginBottom = 6;
            videoBitrateLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(videoBitrateLabel);
            
            Label timeLengthLabel = new Label("Video length (ms):");
            timeLengthLabel.MarginBottom = 6;
            timeLengthLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(timeLengthLabel);
            
            Label fpsLabel = new Label("FPS:");
            fpsLabel.MarginBottom = 6;
            fpsLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(fpsLabel);
            
            Label multisamplingLabel = new Label("Multisampling:");
            multisamplingLabel.MarginBottom = 10;
            multisamplingLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(multisamplingLabel);
            
            Label frameWidthLabel = new Label("Frame width:");
            frameWidthLabel.MarginBottom = 6;
            frameWidthLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(frameWidthLabel);

            Label frameHeightLabel = new Label("Frame height:");
            frameHeightLabel.MarginBottom = 20;
            frameHeightLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(frameHeightLabel);
            
            Label backgroundColorLabel = new Label("Background color:");
            backgroundColorLabel.MarginBottom = 35;
            backgroundColorLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(backgroundColorLabel);
            
            Label includeLabel = new Label("Includes:");
            includeLabel.MarginBottom = 30;
            includeLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(includeLabel);
            
            Label animationPathLabel = new Label("Animation path:");
            animationPathLabel.MarginBottom = 19;
            animationPathLabel.ModifyFg(StateType.Normal, almostWhite);
            labelsBox.Add(animationPathLabel);
            
            newFileCreationBox.Add(labelsBox);

            Box choicesBox = new Box(Gtk.Orientation.Vertical, 0);

            choicesBox.Margin = 5;
            
            //Text views

            TextView animationNameTextView = new TextView();
            animationNameTextView.ModifyBg(StateType.Normal, new Color(212,224,238));
            animationNameTextView.SetSizeRequest(400,16);
            animationNameTextView.MarginBottom = 5;
            animationNameTextView.Buffer.Text = DefaultMafParameters.AnimationInfo.Name;
            choicesBox.Add(animationNameTextView);

            Label videoPath = new Label(DefaultMafParameters.AnimationInfo.VideoName+"."+DefaultMafParameters.AnimationInfo.VideoFormat);
            videoPath.ModifyFg(StateType.Normal, almostWhite);
            Box videoPathBox = new Box(Gtk.Orientation.Horizontal, 0);
            Button videoPathButton = new Button("Browse");
            videoPathButton.Name = "browse_button";
            videoPathBox.SetSizeRequest(400, 16);
            videoPathButton.SetSizeRequest(50, 16);
            ScrolledWindow scrolledVideoPath = new ScrolledWindow();
            videoPath.SetSizeRequest(300,16);
            scrolledVideoPath.SetSizeRequest(300,16);
            scrolledVideoPath.Add(videoPath);
            videoPathButton.MarginBottom = 5;
            videoPathBox.Add(scrolledVideoPath);
            videoPathBox.Add(videoPathButton);
            choicesBox.Add(videoPathBox);

            TextView typeTextView = new TextView();
            typeTextView.SetSizeRequest(400, 16);
            typeTextView.Buffer.Text = DefaultMafParameters.AnimationInfo.Type;
            typeTextView.Editable = false;
            typeTextView.ModifyBg(StateType.Normal, mainColor);
            typeTextView.ModifyFg(StateType.Normal, almostWhite);
            typeTextView.MarginBottom = 5;
            choicesBox.Add(typeTextView);

            TextView versionTextView = new TextView();
            versionTextView.SetSizeRequest(400, 16);
            versionTextView.Buffer.Text = DefaultMafParameters.AnimationInfo.Version;
            versionTextView.Editable = false;
            versionTextView.ModifyBg(StateType.Normal, mainColor);
            versionTextView.ModifyFg(StateType.Normal, almostWhite);
            versionTextView.MarginBottom = 5;
            choicesBox.Add(versionTextView);

            string[] availableVideoFormats = new string[]
            {
                "mp4",
                "flv"
            };

            ComboBox videoFormatChooser = new ComboBox(availableVideoFormats);
            videoFormatChooser.Active = 0;
            
            videoFormatChooser.MarginBottom = 5;
            Box videoFormatChoiceBox = new Box(Gtk.Orientation.Horizontal, 0);
            videoFormatChoiceBox.Add(videoFormatChooser);
            choicesBox.Add(videoFormatChoiceBox);

            Box codecChoiceBox = new Box(Gtk.Orientation.Horizontal, 0);

            string[] availableCodecs = new string[]
            {
                "libx264",
                "libx264rgb",
                "libxvid",
                "mpeg1video",
                "mpeg2video",
                "mpeg4",
                "png",
                "H.264"
            };

            ComboBox codecChooser = new ComboBox(availableCodecs);
            
            codecChooser.Active = 7;

            codecChoiceBox.Add(codecChooser);
            
            codecChoiceBox.MarginBottom = 5;
            choicesBox.Add(codecChoiceBox);
            
            TextView videoBitrateTextView = new TextView();
            videoBitrateTextView.ModifyBg(StateType.Normal, new Color(212,224,238));
            videoBitrateTextView.SetSizeRequest(400, 16);
            videoBitrateTextView.Buffer.Text = DefaultMafParameters.AnimationInfo.VideoBitrate.ToString();
            videoBitrateTextView.MarginBottom = 5;
            choicesBox.Add(videoBitrateTextView);

            TextView timeLengthTextView = new TextView();
            timeLengthTextView.ModifyBg(StateType.Normal, new Color(212,224,238));
            timeLengthTextView.SetSizeRequest(400, 16);
            timeLengthTextView.Buffer.Text = DefaultMafParameters.AnimationInfo.TimeLength.ToString();
            timeLengthTextView.MarginBottom = 5;
            choicesBox.Add(timeLengthTextView);
            
            TextView fpsTextView = new TextView();
            fpsTextView.ModifyBg(StateType.Normal, new Color(212,224,238));
            fpsTextView.SetSizeRequest(400, 16);
            fpsTextView.Buffer.Text = DefaultMafParameters.AnimationInfo.Fps.ToString();
            fpsTextView.MarginBottom = 5;
            choicesBox.Add(fpsTextView);
            
            Box multisamplingBox = new Box(Gtk.Orientation.Horizontal,0);
            multisamplingBox.SetSizeRequest(400, 16);
            multisamplingBox.MarginBottom = 5;
            RadioButton multisamplingOn = new RadioButton("On");
            multisamplingOn.ModifyFg(StateType.Normal, almostWhite);
            RadioButton multisamplingOff = new RadioButton(multisamplingOn, "Off");
            multisamplingOff.ModifyFg(StateType.Normal, almostWhite);
            multisamplingBox.Add(multisamplingOn);
            multisamplingBox.Add(multisamplingOff);
            choicesBox.Add(multisamplingBox);
            
            TextView frameWidthTextView = new TextView();
            frameWidthTextView.ModifyBg(StateType.Normal, new Color(212,224,238));
            frameWidthTextView.SetSizeRequest(400, 16);
            frameWidthTextView.Buffer.Text = DefaultMafParameters.AnimationInfo.FrameWidth.ToString();
            frameWidthTextView.MarginBottom = 5;
            choicesBox.Add(frameWidthTextView);
            
            TextView frameHeightTextView = new TextView();
            frameHeightTextView.ModifyBg(StateType.Normal, new Color(212,224,238));
            frameHeightTextView.SetSizeRequest(400, 16);
            frameHeightTextView.Buffer.Text = DefaultMafParameters.AnimationInfo.FrameHeight.ToString();
            frameHeightTextView.MarginBottom = 5;
            choicesBox.Add(frameHeightTextView);
            
            TextView backgroundColorTextViewR = new TextView();
            TextView backgroundColorTextViewG = new TextView();
            TextView backgroundColorTextViewB = new TextView();
            Box backgroundColorComponents = new Box(Gtk.Orientation.Horizontal, 0);
            Box backgroundColorComponentsVBox = new Box(Gtk.Orientation.Vertical, 0);
            backgroundColorComponents.SetSizeRequest(400, 16);
            backgroundColorComponents.Hexpand = true;
            backgroundColorComponents.MarginBottom = 5;
            Button chooseColorButton = new Button("Choose color");
            chooseColorButton.Name = "light_blue_button";
            Label choosenColorRedComponent = new Label("");
            Label choosenColorGreenComponent = new Label("");
            Label choosenColorBlueComponent = new Label("");
            
            choosenColorRedComponent.ModifyFg(StateType.Normal, almostWhite);
            choosenColorGreenComponent.ModifyFg(StateType.Normal, almostWhite);
            choosenColorBlueComponent.ModifyFg(StateType.Normal, almostWhite);
            
            Box choosenColorBox = new Box(Gtk.Orientation.Horizontal,0);
            choosenColorBox.ModifyBg(StateType.Normal, new Color((byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[0]*255),(byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[1]*255),(byte)(DefaultMafParameters.AnimationInfo.BackgroundColor[2]*255)));
            
            rgbBackground[0] = DefaultMafParameters.AnimationInfo.BackgroundColor[0];
            rgbBackground[1] = DefaultMafParameters.AnimationInfo.BackgroundColor[1];
            rgbBackground[2] = DefaultMafParameters.AnimationInfo.BackgroundColor[2];
            
            choosenColorRedComponent.Text = $"Red = {DefaultMafParameters.AnimationInfo.BackgroundColor[0]*255}";
            choosenColorGreenComponent.Text = $"Green = {DefaultMafParameters.AnimationInfo.BackgroundColor[1]*255}";
            choosenColorBlueComponent.Text = $"Blue = {DefaultMafParameters.AnimationInfo.BackgroundColor[2]*255}";
            
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
            
            choicesBox.Add(backgroundColorComponents);
            
            choosenIncludes.Text = "";
            choosenIncludes.ModifyFg(StateType.Normal, almostWhite);
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
            
            animationPath.Text = "";
            animationPath.ModifyFg(StateType.Normal, almostWhite);
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

            newFileCreationBox.Add(choicesBox);

            Box responseBox = new Box(Gtk.Orientation.Horizontal, 0);
            
            responseBox.ModifyBg(StateType.Normal, mainColor);
            //Buttons
            Button okButton = new Button("Ok");

            okButton.Name = "light_blue_button";

            okButton.Margin = 7;
            okButton.Expand = true;
            
            okButton.Sensitive = false;
            
            responseBox.Add(okButton);

            Button cancelButton = new Button("Cancel");
            cancelButton.Name = "dark_blue_button";
            cancelButton.Margin = 7;
            cancelButton.Expand = true;
            Box responseAndCreationBox = new Box(Gtk.Orientation.Vertical, 0);
            
            responseAndCreationBox.Add(newFileCreationBox);
            
            responseBox.Add(cancelButton);
            responseAndCreationBox.Add(responseBox);
            init.Add(responseAndCreationBox);
            init.ShowAll();
            
            typeTextView.Buffer.Changed += InputMade;
            versionTextView.Buffer.Changed += InputMade;
            animationNameTextView.Buffer.Changed += InputMade;
            videoFormatChooser.Changed += VideoFormatChanged;
            videoBitrateTextView.Buffer.Changed += InputMade;
            timeLengthTextView.Buffer.Changed += InputMade;
            fpsTextView.Buffer.Changed += InputMade;
            frameWidthTextView.Buffer.Changed += InputMade;
            frameHeightTextView.Buffer.Changed += InputMade;

            animationPathButton.Clicked += ChooseFolderButton_Clicked;
            videoPathButton.Clicked += ChooseFolderButton_Clicked;
            includeButton.Clicked += OpenButton_Clicked;
            okButton.Clicked += OkButton_Clicked;
            cancelButton.Clicked += CancelButton_Clicked;
            chooseColorButton.Clicked += ChooseColorButton_Clicked;
            fpsTextView.Buffer.Changed += CheckInt;
            videoBitrateTextView.Buffer.Changed += CheckInt;
            timeLengthTextView.Buffer.Changed += CheckInt;
            frameWidthTextView.Buffer.Changed += CheckInt;
            frameHeightTextView.Buffer.Changed += CheckInt;

            void VideoFormatChanged(object sender, EventArgs a)
            {
                if (videoPath.Text != "")
                {
                    videoPath.Text = System.IO.Path.ChangeExtension(videoPath.Text,
                        availableVideoFormats[videoFormatChooser.Active]);
                }
            }

            void InputMade(object sender, EventArgs a)
            {
                okButton.Sensitive = typeTextView.Buffer.Text != "" && versionTextView.Buffer.Text != "" &&
                                     animationNameTextView.Buffer.Text != "" && 
                                     videoBitrateTextView.Buffer.Text != "" && videoPath.Text != "" &&
                                     timeLengthTextView.Buffer.Text != "" && fpsTextView.Buffer.Text != "" &&
                                     frameWidthTextView.Buffer.Text != "" && frameHeightTextView.Buffer.Text != "" &&
                                     animationPath.Text != "";
            }
            
            void ChooseFolderButton_Clicked(object sender, EventArgs a)
            {
                var chooseFolderDialog = new FileChooserDialog("Choose folder and filename", init, FileChooserAction.Save, "Cancel",
                    ResponseType.Cancel, "Choose", ResponseType.Accept);
                chooseFolderDialog.ShowAll();
    
                if (chooseFolderDialog.Run()==(int)ResponseType.Accept)
                {
                    if (sender == animationPathButton)
                    {
                        animationPath.Text = chooseFolderDialog.Filename;
                                            
                        if (!(System.IO.Path.GetFileName(animationPath.Text).Contains('.')))
                        {
                            animationPath.Text += ".maf";
                        }
                    }
                    else if(sender == videoPathButton)
                    {
                        videoPath.Text = chooseFolderDialog.Filename;
                        
                        if (!(System.IO.Path.GetFileName(videoPath.Text).Contains('.')))
                        {
                            videoPath.Text += $".{availableVideoFormats[videoFormatChooser.Active]}";
                        }
                    }
                    
                }

                okButton.Sensitive = typeTextView.Buffer.Text != "" && versionTextView.Buffer.Text != "" &&
                                     animationNameTextView.Buffer.Text != "" &&
                                     videoBitrateTextView.Buffer.Text != "" && videoPath.Text != "" &&
                                     timeLengthTextView.Buffer.Text != "" && fpsTextView.Buffer.Text != "" &&
                                     frameWidthTextView.Buffer.Text != "" && frameHeightTextView.Buffer.Text != "" &&
                                     animationPath.Text != "";
                
                chooseFolderDialog.Destroy();
            }
            
            void Multisampling_Changed(object sender, EventArgs a)
            {
                Console.WriteLine("multisampling changed");
            }

            void ChooseColorButton_Clicked(object sender, EventArgs a)
            {
                ColorChooserDialog chooseColorWindow = new ColorChooserDialog("Choose color", init);
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
                animationNew.AnimationInfo.VideoFormat = availableVideoFormats[videoFormatChooser.Active];

                animationNew.AnimationInfo.VideoCodec = availableCodecs[codecChooser.Active]=="H.264" ? "h264" : availableCodecs[codecChooser.Active];
                
                animationNew.AnimationInfo.VideoBitrate = int.Parse(videoBitrateTextView.Buffer.Text);
                animationNew.AnimationInfo.VideoName = videoPath.Text;
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

            
        
            void CancelButton_Clicked(object sender, EventArgs a)
            {
                init.Close();
            }
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
        
        void CheckFloat(object sender, EventArgs eventArgs)
        {
            string parsedString = "";
                
            int inputLength = ((TextBuffer)sender).Text.Length;
            var input = (string) ((TextBuffer)sender).Text.Clone();
            bool dotUsed = false;
                
            foreach (var ch in input)
            {
                if (float.TryParse(ch.ToString(), out _)||(ch=='.' && !dotUsed))
                {
                    parsedString += ch.ToString();

                    if (parsedString.Contains('.'))
                    {
                        dotUsed = true;
                    }
                }
            }
                
            if (inputLength != parsedString.Length)
            {
                ((TextBuffer)sender).Text = parsedString;
            }
        }
    }
}