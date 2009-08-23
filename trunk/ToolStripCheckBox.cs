using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing;

namespace GWMultiLaunch
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripCheckBox : ToolStripControlHost
    {
        public ToolStripCheckBox() : base(new CheckBox()) 
        {
            this.BackColor = Color.Transparent;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CheckBox CheckBoxControl
        {
            get { return (CheckBox)this.Control; }
        }

        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);
            CheckBox checkBoxControl = (CheckBox)c;
            checkBoxControl.CheckedChanged +=
                new EventHandler(OnCheckedChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);
            CheckBox checkBoxControl = (CheckBox)c;
            checkBoxControl.CheckedChanged -=
                new EventHandler(OnCheckedChanged);
        }

        public event EventHandler CheckedChanged;

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
            {
                CheckedChanged(this, e);
            }
        }
    }

    
}



    
