﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeetingScheduler
{
    public partial class ParticipantPanel : UserControl
    {
        private Meeting meeting;
        private Participant participant;
        private int mode = 0;
        private CreateMeeting cMCaller;

        public ParticipantPanel(Meeting m, Participant p, int mode = 0,CreateMeeting cMCaller=null)
        {
            /*Modes:
             * 0 = initiator (default)
             * 1 = adding to meeting (instead of selecting role and removing)
             * 2 = non initiator (hide controls);
             * */
            this.cMCaller = cMCaller;
            this.meeting = m;
            this.participant = p;
            InitializeComponent();
            this.nameLbl.Text = p.user.getName();
            this.roleBox.SelectedIndex = p.status;
            ToolTip tTip = new ToolTip();
            tTip.ToolTipTitle = "Roles";
            tTip.SetToolTip(this.roleBox, "Standard: can request equipment.\nImportant: can request equipment & locations.\nGuest Speaker: can request equipment & locations. 1 per meeting.");
            this.mode = mode;
            //if this panel is being created in view of a user who is not the initiator, hide edit options
            if (mode != 0)
            {
                this.roleBox.Visible = false;
                this.roleBox.Enabled = false;
                this.removeBtn.Enabled = false;
                this.removeBtn.Visible = false;
            }
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //try to set the persons image, if this fails then the background image will be used by default
            try
            {
                this.pictureBox1.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(p.user.ImageName.ToString());
                if (this.meeting.Name.ToLower().Contains("funky") && this.participant.user.ImageName == "mehmet")
                {
                    this.pictureBox1.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("funky");
                }
            }
            catch (Exception e)
            {
                Logging.AddMessage(e.Message);
            }
        }

        private void roleBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newStatus = (sender as ComboBox).SelectedIndex;
            //if the newStatus is GS, the old GS will be overwritten
            //and the display updated
                if (newStatus == 2)
                {
                bool hasGSChanged = this.participant != this.meeting.GuestSpeaker;
                    this.meeting.GuestSpeaker = this.participant;
                    //if the parent of this ParticipantPanel is running inside CreateMeeting, call DrawParticipantList()
                    if (hasGSChanged && this.cMCaller!=null)
                    {
                        Logging.AddMessage("Participant list redraw called from child participant panel");
                        this.cMCaller.DrawParticipantList();
                    }
                }
                else {
                    this.participant.status = newStatus;
                }
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if (this.mode == 0)
            {
                this.meeting.Participants.Remove(this.participant);
                this.Parent.Controls.Remove(this);
                if (this.cMCaller!=null){
                    this.cMCaller.RefreshParticipantMeetings();
                    this.cMCaller.AddUserToDropdown(this.participant.user);
                }
            }
        }
    }
}
