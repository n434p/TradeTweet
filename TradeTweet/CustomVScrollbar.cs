using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TradeTweet
{
    internal partial class CustomVScrollbar : UserControl
    {
        Brush oBrush = new SolidBrush(Color.FromArgb(0, 0, 0));
        Brush oWhiteBrush = new SolidBrush(Color.FromArgb(31, 31, 31));

        protected int moLargeChange = 10;
        protected int moSmallChange = 1;
        protected int moMinimum = 0;
        protected int moMaximum = 100;
        protected int moValue = 0;
        private int nClickPoint;

        protected int moThumbTop = 0;

        protected bool moAutoSize = false;

        private bool moThumbDown = false;
        private bool moThumbDragging = false;

        public new event EventHandler Scroll = null;
        public event EventHandler ValueChanged = null;

        private int GetThumbHeight()
        {
            int nTrackHeight = (this.Height - 2);
            float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            // min thumb height
            if (nThumbHeight < 10)
            {
                nThumbHeight = 20;
                fThumbHeight = 20;
            }

            return nThumbHeight;
        }

        public CustomVScrollbar()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            //base.MinimumSize = new Size(this.Width, 2 + GetThumbHeight());
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("LargeChange")]
        public int LargeChange
        {
            get { return moLargeChange; }
            set
            {
                moLargeChange = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("SmallChange")]
        public int SmallChange
        {
            get { return moSmallChange; }
            set
            {
                moSmallChange = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Minimum")]
        public int Minimum
        {
            get { return moMinimum; }
            set
            {
                moMinimum = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Maximum")]
        public int Maximum
        {
            get { return moMaximum; }
            set
            {
                moMaximum = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Value")]
        public int Value
        {
            get { return moValue; }
            set
            {
                moValue = value;

                GetThumbHeight();

                //figure out value
                int nPixelRange = this.Height - 2 - GetThumbHeight();
                int nRealRange = (Maximum - Minimum) - LargeChange;
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = (float)moValue / (float)nRealRange;

                }

                float fTop = fPerc * nPixelRange;
                moThumbTop = (int)fTop;


                Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e);
        }

        internal void Draw(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            ////draw channel left and right border colors
            //e.Graphics.FillRectangle(oWhiteBrush, new Rectangle(0, 1, 1, (this.Height - 1)));
            //e.Graphics.FillRectangle(oWhiteBrush, new Rectangle(this.Width-1, 1, 1, (this.Height - 1)));

            ////draw channel
            //e.Graphics.FillRectangle(oBrush, new Rectangle(1, 1, this.Width-2, (this.Height - 1)));

            //Debug.WriteLine(nThumbHeight.ToString());

            float fSpanHeight = GetThumbHeight();
            int nSpanHeight = (int)fSpanHeight;

            int nTop = moThumbTop;
            nTop += 1;

            e.Graphics.FillRectangle(Brushes.DimGray, new Rectangle(1, nTop, this.Width - 2, nSpanHeight));
        }

        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                if (base.AutoSize)
                {
                    //this.Width = moUpArrowImage.Width;
                }
            }
        }

        private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
        {
            Point ptPoint = this.PointToClient(Cursor.Position);

            int nTop = moThumbTop;
            nTop += 1;


            Rectangle thumbrect = new Rectangle(new Point(1, nTop), new Size(this.Width, GetThumbHeight()));
            if (thumbrect.Contains(ptPoint))
            {

                //hit the thumb
                nClickPoint = (ptPoint.Y - nTop);
                //MessageBox.Show(Convert.ToString((ptPoint.Y - nTop)));
                this.moThumbDown = true;
            }
        }

        private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
        {
            this.moThumbDown = false;
            this.moThumbDragging = false;
        }

        private void MoveThumb(int y)
        {
            int nRealRange = Maximum - Minimum;

            int nSpot = nClickPoint;

            int nPixelRange = (this.Height - 2 - GetThumbHeight());
            if (moThumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewThumbTop = y - (1 + nSpot);

                    if (nNewThumbTop < 0)
                    {
                        moThumbTop = nNewThumbTop = 0;
                    }
                    else if (nNewThumbTop > nPixelRange)
                    {
                        moThumbTop = nNewThumbTop = nPixelRange;
                    }
                    else
                    {
                        moThumbTop = y - (1 + nSpot);
                    }

                    //figure out value
                    float fPerc = (float)moThumbTop / (float)nPixelRange;
                    float fValue = fPerc * (Maximum - LargeChange);
                    moValue = (int)fValue;

                    Application.DoEvents();

                    Invalidate();
                }
            }
        }

        private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (moThumbDown == true)
            {
                this.moThumbDragging = true;
            }

            if (this.moThumbDragging)
            {

                MoveThumb(e.Y);
            }

            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());

            if (Scroll != null)
                Scroll(this, new EventArgs());
        }

    }
}
