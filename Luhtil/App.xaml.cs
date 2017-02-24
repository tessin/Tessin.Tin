using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using Luhnaris.Framework;

namespace Luhtil
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] Args;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow mainWindow = new MainWindow();

            if (Luhtil.Properties.Settings.Default.Top > -99999.0) mainWindow.Top = Luhtil.Properties.Settings.Default.Top;
            if (Luhtil.Properties.Settings.Default.Left > -99999.0) mainWindow.Left = Luhtil.Properties.Settings.Default.Left;
            
            MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Args = e.Args;
            if (Args.Length > 0)
            {
                string arg0 = Args[0].ToLower();
                if (arg0 == "/p")
                {
                    string pnr = LuhnGenerate.GeneratePnr();
                    Clipboard.SetDataObject(pnr, true);
                }
                else if (arg0 == "/o")
                {
                    string onr = LuhnGenerate.GenerateOnr();
                    Clipboard.SetDataObject(onr, true);
                }
                Current.Shutdown();
            }
        }

        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        } 
        
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? 
                args.Name.Substring(0, args.Name.IndexOf(',')) : 
                args.Name.Replace(".dll", ""); 
            dllName = dllName.Replace(".", "_");
            if (dllName != "Luhnaris_Framework") return null;
            ResourceManager rm = new ResourceManager(
                GetType().Namespace + ".Properties.Resources", 
                Assembly.GetExecutingAssembly()); 
            byte[] bytes = (byte[])rm.GetObject(dllName); 
            return Assembly.Load(bytes);
        }
    }
}
