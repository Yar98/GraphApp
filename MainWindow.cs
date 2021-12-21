using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Drawing.Drawing2D;



namespace FunctionGraph
{
    class MainWindow: Form
    {
        private Label lFunc;
	private Label lTo ;
	private Label lFrom;
	private Label lTimer;
	private Label lBall;
	private TextBox tbFunc;
	private TextBox tbFrom;
	private TextBox tbTo;
	private TextBox tbRange;
	public Button btDraw;
	public Panel pDrawing;
	private object threadLock = new object();

	public MainWindow()
	{
	    //Configure Form
	    this.Text = "Function Drawing";
	    this.Width = 1200;
	    this.Height = 835;
	    this.BackColor = Color.White;
	    CenterToScreen();

	    lFunc = new Label();
	    lFunc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
	    lFunc.Text = "Function";
	    lFunc.Size = new Size(lFunc.PreferredWidth, lFunc.PreferredHeight);
	    lFunc.Location = new Point(20, 20);
	    this.Controls.Add(lFunc);

	    tbFunc = new TextBox();
	    tbFunc.Text = "x^2+2*x+1";
	    tbFunc.Size= new Size(120, lFunc.PreferredHeight - 5);
	    tbFunc.Location = new Point(lFunc.PreferredWidth + 40, 20);
	    this.Controls.Add(tbFunc);

	    lFrom = new Label();
	    lFrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
	    lFrom.Text = "From";
	    lFrom.Size = new Size(lFrom.PreferredWidth, lFrom.PreferredHeight);
	    lFrom.Location = new Point(20,50);
	    this.Controls.Add(lFrom);

	    tbFrom = new TextBox();
	    tbFrom.Text = "1";
	    tbFrom.Size = new Size(50, tbFrom.PreferredHeight - 5);
	    tbFrom.Location = new Point(lFunc.PreferredWidth +40, 50);
	    this.Controls.Add(tbFrom);

	    lTo = new Label();
	    lTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
	    lTo.Text = "To";
	    lTo.Size = new Size(lTo.PreferredWidth, lTo.PreferredHeight);
	    lTo.Location = new Point(200, 50);
	    this.Controls.Add(lTo);

	    tbTo = new TextBox();
	    tbTo.Text = "5";
	    tbTo.Size = new Size(50, tbTo.PreferredHeight - 5);
	    tbTo.Location = new Point(260,50);
	    this.Controls.Add(tbTo);

	    lTimer = new Label();
	    lTimer.Text = "Timer";
	    lTimer.Font = new Font("Arial", 45);
	    lTimer.TextAlign = ContentAlignment.TopCenter;
	    lTimer.Margin = new Padding(0);
	    lTimer.Size = new Size(400, 60);
	    lTimer.Location = new Point(355,0);
	    this.Controls.Add(lTimer);
	    
	    
	    pDrawing = new PanelDraw();
	    this.Controls.Add(pDrawing);
	    btDraw = new Button();
	    btDraw.Text = "Draw";
	    btDraw.Location = new Point(pDrawing.Location.X + 740, 90);
	    this.Controls.Add(btDraw);
	    btDraw.Click += (o , e) =>
	    		 {
			     PanelDraw p = (PanelDraw)pDrawing;
			     p.InitNewAxis(this);
			     p.Refresh();
			     Task.Factory.StartNew(() =>
			     {
			         p.DrawGraph(tbRange.Text);
			     });
			 };

	    tbRange = new TextBox();
	    tbRange.Size = new Size(50, tbRange.PreferredHeight);
	    tbRange.Location = new Point(btDraw.Location.X + btDraw.Width,90);
	    this.Controls.Add(tbRange);
	    
	    lBall = new Label();
	    lBall.Location = new Point(800,500);
	    lBall.Size = new Size(70, 70);
	    lBall.BackColor = Color.White;
	    this.Controls.Add(lBall);

	    GetResource();

	    Thread thread = new Thread(new ThreadStart(RotateImage));
	    thread.Start();

	    Thread thread1 = new Thread(new ThreadStart(Move_lBall));
	    thread1.Start();

	    Thread thread2 = new Thread(new ThreadStart(PrintTime));
	    thread2.Start();
	}


	public void PrintTime()
	{	    
	    //lTimer.Text = DateTime.Now.ToLongTimeString();
	    while(true)
	    {
	        lTimer.Invoke((Action)delegate
			{
			    lTimer.Text = DateTime.Now.ToLongTimeString();
			}
		);
		Thread.Sleep(1000);
	    }
	}

	public void GetResource()
	{
	    ResourceManager rm = new ResourceManager("imageresource",typeof(Program).Assembly);
	    Bitmap screen = (Bitmap)Image.FromStream(rm.GetStream("Image"));
	    lBall.Image = new Bitmap(screen,70,70);
	}

	public void AddResource()
	{
	    string resFile = @".\imageresource.resx";
	    using(ResXResourceWriter resx = new ResXResourceWriter(resFile))
	    {
		Image img = Image.FromFile(@".\Resource\download.png");
		Bitmap bmp = new Bitmap(img,70,70);
		MemoryStream imageStream = new MemoryStream();
		bmp.Save(imageStream, ImageFormat.Jpeg);
	        resx.AddResource("Image", imageStream);
	    }
	}
	
	public void RotateImage()
	{
	   // lock(threadLock)
	   // {
        	Image img = lBall.Image;
		if(img == null) return;
		PointF[] rotationPoints = {new PointF(0,0),
					 new PointF(img.Width,0),
					 new PointF(0,img.Height),
					 new PointF(img.Width,img.Height)};
		double degreeAngle = 0;
		while(true)
		{
		    degreeAngle += 5;
		    PointMath.RotationPoints(rotationPoints, new PointF(img.Width/2.0f, img.Height/2.0f),degreeAngle);
		    Rectangle bounds = PointMath.GetBounds(rotationPoints);
		    Bitmap rotateBitmap = new Bitmap(bounds.Width, bounds.Height);
		    using(Graphics g = Graphics.FromImage(rotateBitmap))
		    {
			Matrix m = new Matrix();
			m.RotateAt((float)degreeAngle, new PointF(img.Width/2.0f,img.Height/2.0f));
			m.Translate(-bounds.Left, -bounds.Top, MatrixOrder.Append);

			g.Transform = m;
			g.DrawImage(img,0,0);
		    }
		    lBall.Invoke((Action)delegate
			{
			    lBall.Image = (Image)rotateBitmap;
			}
		    );
		    Thread.Sleep(10);
		}
	   // }

	}
	
	public void Move_lBall()
	{
	    //lock(threadLock)
	    //{
	       int vecX = 1;
	       int vecY = 1;

	       while(true)
	       {
		   if(lBall.Location.X < 1 || lBall.Location.X + 70 > 1190)
		   {
		       vecX = -vecX;
		   }
		   if(lBall.Location.Y < 1 || lBall.Location.Y + 70 > 800)
		   {
		       vecY = -vecY;
		   }
		   lBall.Invoke((Action)delegate
		   {
		       lBall.Location = new Point(lBall.Location.X + vecX,
						  lBall.Location.Y + vecY);        
		   });
		   Thread.Sleep(10);					   					   
	       }
	    //}
	}


	public string GetText_tbFunc()
	{
	    return tbFunc.Text;
	}

	public float GetText_tbFrom()
	{
	    return float.Parse(tbFrom.Text);
	}

	public float GetText_tbTo()
	{
	    return float.Parse(tbTo.Text);
	}
    }
}












































