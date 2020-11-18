﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeetingScheduler
{
    public partial class MainForm : Form
    {
        //other forms
        DevLogForm devForm = new DevLogForm();

        //preset variables
        List<User> presetUsers = new List<User>(){new User("John"), new User("Jack"), new User("Tom"), new User("Ryan"), new User("Mehmet") };
        public MainForm()
        {
            InitializeComponent();
            // set the drop down options
            impersonationComboBox.DataSource = presetUsers;
            impersonationComboBox.DisplayMember = "Name";
            //set up the devForm
            devForm.ShowIcon = false;
            devForm.ShowInTaskbar = true;
            devForm.AddMessage("Initialisation complete");
        }

        //this function is called on ResizeEnd
        private void ResizeWindow(object sender, EventArgs e)
        {
            //TODO: resize inner elements, if needed

        }

        private void yourMeetingsTab_Click(object sender, EventArgs e)
        {
        }

        private void detailsBox_Enter(object sender, EventArgs e)
        {
            devForm.AddMessage("Details Entered");
        }

        private void devWindowBtn_Click(object sender, EventArgs e)
        {
            devForm.Show();
            devForm.Activate();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl sdr = sender as TabControl;
            devForm.AddMessage("Tab selected: " + sdr.SelectedIndex);
        }
    }
}
