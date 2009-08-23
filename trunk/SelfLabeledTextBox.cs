//Guild Wars MultiLaunch - Safe and efficient way to launch multiple GWs.
//The Guild Wars executable is never modified, keeping you inline with the tos.
//
//Copyright (C) 2009  IMKey@IMKey@GuildWarsGuru

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace GWMultiLaunch
{
    public class SelfLabeledTextBox : System.Windows.Forms.TextBox
    {
        #region Structures

        private struct TextboxLabel
        {
            public string text;
            public Color color;
            public Font font;
        }

        #endregion

        #region Constants

        private const int WM_SETFOCUS   = 7;
        private const int WM_KILLFOCUS  = 8;
        private const int WM_PAINT      = 15;

        #endregion

        #region Member Variables

        private TextboxLabel mLabel;
        private bool mDrawLabel;

        #endregion

        #region Properties

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [Description("The label text.")]
        public string LabelText
        {
            get { return mLabel.text; }
            set { mLabel.text = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [Description("Color of the label text.")]
        public Color LabelColor
        {
            get { return mLabel.color; }
            set { mLabel.color = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [Description("Font of the label text.")]
        public Font LabelFont
        {
            get { return mLabel.font; }
            set { mLabel.font = value; this.Invalidate(); }
        }

        #endregion

        #region Functions

        public SelfLabeledTextBox()
        {
            mLabel = new TextboxLabel();
            mLabel.text = string.Empty;
            mLabel.color = SystemColors.GrayText;
            mLabel.font = this.Font;
            mDrawLabel = true;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.Invalidate();
        }

        protected override void OnTextAlignChanged(EventArgs e)
        {
            base.OnTextAlignChanged(e);
            this.Invalidate();
        }

        protected override void WndProc(ref System.Windows.Forms.Message winMsg)
        {
            base.WndProc(ref winMsg);

            switch (winMsg.Msg)
            {
                case WM_SETFOCUS:
                    mDrawLabel = false;
                    break;

                case WM_KILLFOCUS:
                    mDrawLabel = true;
                    break;

                case WM_PAINT:
                    if (mDrawLabel && this.Text.Length == 0)
                        DrawLabel();
                    break;
            }
        }

        protected virtual void DrawLabel()
        {
            using (Graphics g = this.CreateGraphics())
            {
                DrawLabel(g);
            }
        }

        protected virtual void DrawLabel(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top;

            Rectangle rect = this.ClientRectangle;

            ApplyAlignment(ref flags, ref rect);

            Color backColor = this.Enabled ? this.BackColor : SystemColors.Control;

            TextRenderer.DrawText(g, mLabel.text, mLabel.font,
                rect, mLabel.color, backColor, flags);
        }

        protected virtual void ApplyAlignment(ref TextFormatFlags flags, ref Rectangle rect)
        {
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Center:
                    flags = flags | TextFormatFlags.HorizontalCenter;
                    rect.Offset(0, 1);
                    break;
                case HorizontalAlignment.Left:
                    flags = flags | TextFormatFlags.Left;
                    rect.Offset(1, 1);
                    break;
                case HorizontalAlignment.Right:
                    flags = flags | TextFormatFlags.Right;
                    rect.Offset(0, 1);
                    break;
            }
        }

        #endregion
    }
}
