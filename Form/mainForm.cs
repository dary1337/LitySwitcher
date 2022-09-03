using InternetSwitcher.Properties;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetSwitcher {

    public partial class mainForm : Form {

        protected override CreateParams CreateParams {

            get {
                const int CS_DROPSHADOW = 0x00030000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public mainForm() {

            InitializeComponent();


            var pref = EdgeSupport.DWMWCP.DWMWCP_ROUND;
            EdgeSupport.DwmSetWindowAttribute(Handle, EdgeSupport.DWMWA.DWMWA_WINDOW_CORNER_PREFERENCE, ref pref, sizeof(uint));


            ClientSize = new Size(a(390), a(80));

            Graphics g = CreateGraphics();
            AutoScaleDimensions = new SizeF(g.DpiX, g.DpiY);

            Location = new Point(a(ScreenResolution.rWidth - 400), a(ScreenResolution.rHeight - 130));

            Icon = Resources.icon;


            foreach (Control c in new Control[] {

                this,
                label1,
                label3,
                pictureBox1
            })
                c.Click += (s, e) => Exit();


            if (Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Nls\Language").GetValue("Default").ToString() != "0419")
                lang = "en";

        }

        public static string lang = "ru";

        async void Form1_Load(object sender, EventArgs e) {

            if (Process.GetProcesses().Count(x => x.ProcessName == Path.GetFileName(Application.ExecutablePath).Replace(".exe", "")) == 1) {

                bool themeIsLight = true;
                int awaitTime = 3000;


                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                    if (key?.GetValue("AppsUseLightTheme")?.ToString() != "1") {

                        themeIsLight = false;

                        BackColor = Color.FromArgb(35, 40, 45);

                        foreach (Label lab in Controls.OfType<Label>())
                            lab.ForeColor = Color.Yellow;

                        label1.ForeColor = Color.White;
                    }


                try {

                    foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL").Get()) {

                        string name = mo["NetConnectionId"].ToString().ToLower();

                        if (!name.Contains("ethernet") && !name.Contains("wi-fi"))
                            continue;


                        bool netEnabled = (bool)mo["NetEnabled"];

                        mo.InvokeMethod(netEnabled ? "Disable" : "Enable", null);


                        label1.Text = dict.d[(netEnabled ? "iOff_" : "iOn_") + lang];

                        pictureBox1.Image = netEnabled
                            ? (themeIsLight ? Resources.DarkError : Resources.LightError)
                            : (themeIsLight ? Resources.Dark : Resources.Light);

                        break;
                    }

                } catch {

                    label1.Text = dict.d["catch_" + lang];

                    pictureBox1.Image = themeIsLight ? Resources.DarkWarning : Resources.LightWarning;

                    awaitTime = 4000;
                }


                for (double d = 0D; d <= 0.95; d += 0.2, await Task.Delay(20))
                    Opacity = d;

                Opacity = 0.95D;


                new SoundPlayer(Resources.Notify).Play();
                await Task.Delay(awaitTime);
                Exit();
            }
            else
                Close();
        }

        int a(int i) => ScreenResolution.scaleOfScreen == 100 ? i : ScreenResolution.scaleOfScreen / 100;

        async void Exit() {

            while (Opacity != 0) { Opacity -= 0.2; await Task.Delay(20); }

            Close();
        }
    }
}
