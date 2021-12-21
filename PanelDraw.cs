using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace FunctionGraph
{
    class Canvas
    {
        public static string[] originAxis = new string[]{"-4","-3","-2","-1","","1","2","3","4"};
	public static int width = 700;
	public static int height = 700;
	public static float scaleX = 1;
	public static float scaleY = 1;
	public static int numberOfUnit = 4;
	public static int allMark = numberOfUnit * 2 +1;
	public static int widthBySquare = 4* (numberOfUnit +1);
	public static int squareByPixel = width/ widthBySquare ;
	public static int X=20;
	public static int Y=90;
	public static string []axisX;
	public static string []axisY;
	public static int unit = squareByPixel * 2;
    }

    class PanelDraw : System.Windows.Forms.Panel
    {
         
	public PanelDraw()
	{
	    this.Location = new Point(20,90);
	    this.Size = new Size(701,701);
	    //THIS.BORDERSTYLE = SYSTEM.WINDOWS.FORMS.BORDERSTYLE.FIXED3D;
	}

  	protected override void OnPaint(PaintEventArgs e)
	{
	   base.OnPaint(e);
	   Graphics g = e.Graphics;
	   Font drawFont = new Font("Arial",8);
	   SolidBrush whiteBrush = new SolidBrush(Color.White);
	   SolidBrush blackBrush = new SolidBrush(Color.Black);
	   Pen blackPen = new Pen(Color.Black);
	   Pen grayPen = new Pen(Color.Gray);
	   g.FillRectangle(whiteBrush,0,0,700,700);
	   if(Canvas.axisX == null || Canvas.axisX.Length == 0)
	   {
	       Canvas.axisX = Canvas.originAxis;
	       Canvas.axisY = Canvas.originAxis;
	   }
	   for(int i = 0; i < Canvas.widthBySquare + 1; i++)
	   {
	       g.DrawLine(grayPen, i * Canvas.squareByPixel,
	       			   0,
	       			   i * Canvas.squareByPixel,
				   Canvas.height);
	       g.DrawLine(grayPen, 0,
	       			   i * Canvas.squareByPixel,
				   Canvas.width,
				   i * Canvas.squareByPixel);
               if(i % 2 == 0 && i != 0 && i < Canvas.widthBySquare)
	       {
	           g.DrawString(Canvas.axisX[i/2 - 1],
		   		drawFont,
				blackBrush,
		   		i * Canvas.squareByPixel + 1,
		             	Canvas.widthBySquare/2 * Canvas.squareByPixel );
                   g.DrawString(Canvas.axisY[Canvas.allMark - i/2],
		   		drawFont,
				blackBrush,
		                Canvas.widthBySquare/2 * Canvas.squareByPixel,
			        i * Canvas.squareByPixel +1);
		   g.DrawLine(blackPen, i * Canvas.squareByPixel,
		   			Canvas.widthBySquare/2 * Canvas.squareByPixel -6,
					i * Canvas.squareByPixel,
					Canvas.widthBySquare/2 * Canvas.squareByPixel +6);
		   g.DrawLine(blackPen, Canvas.widthBySquare/2 * Canvas.squareByPixel -6,
		   			i * Canvas.squareByPixel,
					Canvas.widthBySquare/2 * Canvas.squareByPixel +6,
					i * Canvas.squareByPixel);
	       }
	       
	   }
	   g.DrawRectangle(blackPen,0,0,700,700);
	   g.DrawLine(blackPen,Canvas.widthBySquare * Canvas.squareByPixel/2,
	   		       0,
			       Canvas.widthBySquare * Canvas.squareByPixel/2,
			       Canvas.height);
	   g.DrawLine(blackPen,0,
			       Canvas.widthBySquare * Canvas.squareByPixel/2,
			       Canvas.width,
			       Canvas.widthBySquare * Canvas.squareByPixel/2);
	}

	string postfixFunc;
	float from;
	float to;

	public void DrawGraph(string range)
	{
	    Graphics g = Graphics.FromHwnd(this.Handle);
	    //Graphics g = this.CreateGraphics();
	    int intRange = String.IsNullOrEmpty(range) ? 0 : int.Parse(range);
	    Pen redPen = new Pen(Color.Red);
	    float endX = to;
	    float startX = from;
	    float startY;
	    float nextX,
	    	  nextY;
	    float dis;
	    dis = 0.1f;
	    if(Math.Abs(startX) > 0 && Math.Abs(startX) < 1) dis = startX/10;
	    
	    while(startX < endX - dis)
	    {
		
	        nextX = startX + dis;
		startY = Expression.evaluatePostfix(postfixFunc, startX);
		nextY = Expression.evaluatePostfix(postfixFunc, nextX);
		float x1 = startX * Canvas.unit / Canvas.scaleX;
		float x2 = nextX * Canvas.unit / Canvas.scaleX;
		float y1 = startY * Canvas.unit / Canvas.scaleY;
		float y2 = nextY * Canvas.unit / Canvas.scaleY;
		g.DrawLine(redPen, (int)x1 +  350,
				   (int)(-y1 + 350),
				   (int)x2 +  350,
				   (int)(-y2 + 350));
		Thread.Sleep(intRange);
		startX += dis;
	    }
	    g.Dispose();
	}

	public void InitNewAxis(MainWindow mw)
	{
	    string func = mw.GetText_tbFunc();
	    from = mw.GetText_tbFrom();
	    to = mw.GetText_tbTo();
	    
	    float maxX = Math.Abs(from) <= Math.Abs(to) ? to : from;
	    postfixFunc = Expression.infixToPostfix(func);
	    float maxY = Expression.evaluatePostfix(postfixFunc, maxX);
	    Canvas.scaleX = maxX/4;
	    Canvas.scaleY = maxY/4;
	    Canvas.axisX = new string[9];
	    Canvas.axisY = new string[9];
	    Canvas.axisX[4] = "";
	    Canvas.axisY[4] = "";
	    for(int i = 0; i < 5; ++i)
	    {
	        float x = (4-i) * Canvas.scaleX;
		float y = (4-i) * Canvas.scaleY;
		Canvas.axisX[i] = (-x).ToString();
		Canvas.axisX[8-i] = x.ToString();
		Canvas.axisY[i] = (-y).ToString();
		Canvas.axisY[8-i] = y.ToString();
	    }
	}
    }
}