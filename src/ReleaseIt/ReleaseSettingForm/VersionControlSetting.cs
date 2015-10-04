using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReleaseIt;
using ReleaseIt.VersionControls;

namespace Release
{
    public partial class VersionControlSetting : Form
    {
        public VersionControlSetting()
        {
            InitializeComponent();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var ctrl = Create();
            ctrl.Url = this.urlTextBox.Text;
            ctrl.Password = this.pwdTextBox.Text;
            ctrl.UserName = this.userNameTextBox.Text;
            ctrl.WorkingCopy = this.workFolderTextBox.Text;

            this.Version = ctrl;
        }

        public VersionControl Version { get; set; }

        private VersionControl Create()
        {
            if (this.radioButton1.Checked)
            {
                return new Git();
            }
            return new Svn();
        }


    }
}
