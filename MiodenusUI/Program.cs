using System;
using Gtk;
using MiodenusUI.MAFStructure;

namespace MiodenusUI
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.MiodenusUI.MiodenusUI", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            LoaderMaf maf = new LoaderMaf();

            MAFStructure.Animation animation = new Animation();
            animation = maf.Load("test.txt");
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}