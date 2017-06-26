using Sulakore.Communication;
using Sulakore.Habbo;
using Sulakore.Modules;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tangine;

namespace Spam
{
    [Module("Spam", "Spam fast sanik")]
    [Author("Author", HabboName = "SeIIer", Hotel = HHotel.Com)]

    public partial class Spam : ExtensionForm
    {
        bool notDragging = true;
        int mX = 0;
        int mY = 0;
        public Spam()
        {
            InitializeComponent();
            main.init(this);
            Hook.init();
            Triggers.OutAttach(main.clickHeader, GrabCoord);
            Hook.F3 += StopClick;
            Hook.F4 += PauseClick;
            Hook.F5 += KillClick;
        }

        private void GrabCoord(DataInterceptedEventArgs e)
        {
            e.Continue();
            main.X = e.Packet.ReadInteger();
            main.Y = e.Packet.ReadInteger();
            output.Text = $"Clicked ( {main.X},{main.Y} )\nSending clicks ...";
            stop.ForeColor = Color.White;
            main.Start();
        }

        public void PauseClick(object sender, EventArgs e)
        {
            if (main.killed) return;
            if (pause.Text == "Pause Listening")
            {
                Triggers.OutDetach(main.clickHeader);
                pause.Text = "Resume Listening";
            }
            else
            {
                Triggers.OutAttach(main.clickHeader, GrabCoord);
                pause.Text = "Pause Listening";
            }
        }
        public void KillClick(object sender, EventArgs e)
        {
            if (kill.Text == "Kill All")
            {
                main.Stop();
                main.killed = true;
                Triggers.OutDetach(main.clickHeader);
                stop.ForeColor = Color.FromArgb(53, 53, 53);
                pause.ForeColor = Color.FromArgb(53, 53, 53);
                output.Text = "Killed";
                kill.Text = "Start";
            }
            else
            {
                main.killed = false;
                Triggers.OutAttach(main.clickHeader, GrabCoord);
                pause.ForeColor = Color.White;
                output.Text = "Ready";
                kill.Text = "Kill All";
                pause.Text = "Pause Listening";
            }
        }
        private void DragDown(object sender, MouseEventArgs e)
        {
            mX = e.X;
            mY = e.Y;
            notDragging = false;
        }
        private void DragUp(object sender, MouseEventArgs e)
        {
            notDragging = true;
            Location = new Point(Math.Min(Math.Max(Location.X, 0), Screen.FromControl(this).WorkingArea.Width), Math.Min(Math.Max(Location.Y, 0), Screen.FromControl(this).WorkingArea.Height - 30));
        }
        private void DragMove(object sender, MouseEventArgs e)
        {
            if (notDragging) return;
            Location = new Point(e.X + Location.X - mX, e.Y + Location.Y - mY);
        }

        private void CloseClick(object sender, EventArgs e)
        {
            Application.Exit();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int val;
            if (!int.TryParse(interval.Text, out val)) val = 100;
            main.SetInterval(val);
            interval.Text = val.ToString();
        }
        private void StopClick(object sender, EventArgs e)
        {
            if (main.killed) return;
            main.Stop();
            output.Text = "Stopped";
            stop.ForeColor = Color.FromArgb(53, 53, 53);
        }
        public void Output(string val)
        {
            output.Text = val;
        }
    }
}

// TOTEST: Dragging, Pausing while killed;