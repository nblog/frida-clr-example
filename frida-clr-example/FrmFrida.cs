using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace frida_clr_example
{
    public partial class FrmFrida : Form
    {




        private Frida.DeviceManager deviceManager;
        private Frida.Device localDevice;

        public ObservableCollection<Frida.Device> Devices { get; private set; }
        public ObservableCollection<Frida.Process> Processes { get; private set; }


        private Frida.Script script;
        private Frida.Session session;



        public bool HexShow { get; set; } = false;
        public string InjectScript { get; set; } = string.Empty;

        public string ScriptSourceFromFile()
        {
            var f = System.IO.Path.GetFullPath(InjectScript);
            if (System.IO.File.Exists(f))
            {
                var content = System.IO.File.ReadAllBytes(f);
                return Encoding.UTF8.GetString(content);
            }
            return string.Empty;
        }




        private ContextMenuStrip lstProcessesMenu = new ContextMenuStrip();


        public FrmFrida()
        {
            InitializeComponent();

            Devices = new ObservableCollection<Frida.Device>();
            Processes = new ObservableCollection<Frida.Process>();

            deviceManager = new Frida.DeviceManager(Dispatcher.CurrentDispatcher);


            this.lstProcessesMenu.Items.Add("Refresh", null,
                (s, eArgs) =>
                {
                    RefreshProcesses();
                });
            this.lstProcessesMenu.Items.Add("-");
            this.lstProcessesMenu.Items.Add(HexShow ? "Dec" : "Hex", null,
                (s, eArgs) =>
                {
                    HexShow = !HexShow;
                    RefreshProcesses();
                });

        }



        private void FrmFrida_Load(object sender, EventArgs e)
        {
            //this.MaximizeBox = false;
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.StartPosition = FormStartPosition.CenterParent;

            this.Text = "frida clr example";

            this.lstProcesses.View = View.Details;
            this.lstProcesses.FullRowSelect = true;
            this.lstProcesses.GridLines = true;
            this.lstProcesses.MultiSelect = false;
            this.lstProcesses.MouseClick += lstProcesses_MouseClick;
            this.lstProcesses.DoubleClick += lstProcesses_DoubleClick;


            this.lstProcesses.Columns.Clear();
            this.lstProcesses.BeginUpdate();
            foreach (var item in new string[] { "PID", "NAME" })
            {
                this.lstProcesses.Columns.Add(item, 120);
            }
            this.lstProcesses.EndUpdate();

            
            //
            try
            {
                var devices = deviceManager.EnumerateDevices();

                Devices.Clear();

                Array.Sort(devices, delegate (Frida.Device a, Frida.Device b) {
                    return a.Type.CompareTo(b.Type);
                });

                foreach (var dev in devices)
                    Devices.Add(dev);

            }
            catch (Exception ex)
            {
                Devices.Clear();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            localDevice = Devices[0] as Frida.Device;


            RefreshProcesses();
        }


        private void lstProcesses_DoubleClick(object sender, EventArgs e)
        {
            if (1 == lstProcesses.SelectedItems.Count)
            {
                var process = lstProcesses.SelectedItems[0].Tag as Frida.Process;

                if ((btnInject.Enabled = txtScript.Enabled = AttachProcess(process.Pid)))
                {
                    labInfo.Text = String.Format("Name: {0}", process.Name);

                    txtDebug.Clear();
                    txtDebug.Text += String.Format("[{0}] " + "attach process succeed." + Environment.NewLine,
                        process.Pid.ToString(HexShow ? "X08" : "G"));
                }

            }
        }

        private void lstProcesses_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                this.lstProcessesMenu.Items[2].Text = HexShow ? "Dec" : "Hex";
                this.lstProcessesMenu.Show(sender as ListView, e.Location);
            }
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            if (CreateScript(txtScript.Text))
            {
                txtDebug.Text += String.Format(Environment.NewLine + "inject script succeed."
                    + Environment.NewLine
                    + "======== Debug Infos:" + Environment.NewLine);
            }
        }


        public void RefreshProcesses()
        {
            try
            {
                var processes = localDevice.EnumerateProcesses();

                Processes.Clear();

                Array.Sort(processes, delegate (Frida.Process a, Frida.Process b) {
                    return a.Pid.CompareTo(b.Pid);
                });

                foreach (var proc in processes)
                    Processes.Add(proc);
            }
            catch (Exception ex)
            {
                Processes.Clear();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            //
            this.lstProcesses.BeginUpdate();
            this.lstProcesses.Items.Clear();
            foreach (var proc in Processes)
            {
                ListViewItem item = new ListViewItem(new string[] {
                    proc.Pid.ToString(HexShow ? "X08" : "G"),
                    proc.Name,
                });
                item.Tag = proc;
                this.lstProcesses.Items.Add(item);
            }
            this.lstProcesses.EndUpdate();

            return;
        }

        public bool AttachProcess(UInt32 processId)
        {
            if (session != null)
            {
                try
                {
                    session.Detach();
                }
                catch (Exception)
                {

                }
                session = null;
            }

            try
            {
                session = localDevice.Attach(processId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            session.Detached += delegate (object sender, Frida.SessionDetachedEventArgs e)
            {
                if (sender == session)
                {
                    session = null;
                    script = null;
                }
            };

            return true;
        }


        public bool CreateScript(string utf8Source = "")
        {
            if (null == session || (string.Empty == utf8Source && string.Empty == (utf8Source = ScriptSourceFromFile())))
            {
                return false;
            }

            if (script != null)
            {
                try
                {
                    script.Unload();
                }
                catch (Exception)
                {

                }
                script = null;
            }

            try
            {
                script = session.CreateScript(utf8Source);

                script.Load();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            script.Message += new Frida.ScriptMessageHandler(onMessage);

            return true;
        }


        private void onMessage(object sender, Frida.ScriptMessageEventArgs e)
        {
            if (sender == script)
            {
                dynamic message = JsonConvert.DeserializeObject(e.Message);

                if (null == message)
                    return;

                if ("log" == message.type.Value)
                {
                    txtDebug.Text += "log " + message.level.Value as String + ": " + message.payload.Value + Environment.NewLine;
                }
                else if ("send" == message.type.Value)
                {
                    if (message.payload.Value is String)
                    {
                        txtDebug.Text += message.payload.Value + Environment.NewLine;
                    }
                    else {

                        var strJson = JsonConvert.SerializeObject(message.payload);

                        txtDebug.Text += strJson + Environment.NewLine;
                    }

                    //txtDebug.Text += 
                    //    String.Format("Data: {0}", e.Data == null ? "null" : String.Join(", ", e.Data)+ Environment.NewLine);
                }
            }
        }

    }
}
