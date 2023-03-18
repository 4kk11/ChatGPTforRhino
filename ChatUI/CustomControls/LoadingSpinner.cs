using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace ChatUI.CustomControls
{
	public class LoadingSpinner : Canvas
	{
		// パラメーターとしてプロパティを定義する
		public int CircleCount { get; set; } = 8; // 円の数
		public double CircleRadius { get; set; } = 20; // 円の半径
		public double EllipseSize { get; set; } = 12; // 円のサイズ

		private Canvas LoadingSpinnerCanvas;

		public LoadingSpinner()
		{
			// Canvasコントロールを作成する
			LoadingSpinnerCanvas = new Canvas();
			LoadingSpinnerCanvas.Width = 100;
			LoadingSpinnerCanvas.Height = 100;

			// CreateLoadingSpinnerメソッドを呼び出す
			CreateLoadingSpinner();

			// コントロールにCanvasを追加する
			this.Children.Add(LoadingSpinnerCanvas);
		}

		private void CreateLoadingSpinner()
		{

			double canvasCenterX = LoadingSpinnerCanvas.ActualWidth / 2; // Canvasの中心点のX座標
			double canvasCenterY = LoadingSpinnerCanvas.ActualHeight / 2; // Canvasの中心点のY座標
			double angle = 0; // 円を配置する角度（45度単位）

			for (int i = 0; i < CircleCount; i++)
			{
				// 角度をラジアンに変換
				double angleInRadians = angle * Math.PI / 180;

				// 円を配置する座標を計算
				double circleCenterX = canvasCenterX + CircleRadius * Math.Cos(angleInRadians);
				double circleCenterY = canvasCenterY + CircleRadius * Math.Sin(angleInRadians);

				// 円を作成してCanvasに追加
				Ellipse circle = new Ellipse();
				circle.Width = EllipseSize;
				circle.Height = EllipseSize;
				circle.Fill = new SolidColorBrush(Color.FromRgb(0x62, 0xA7, 0xB7));
				Canvas.SetLeft(circle, circleCenterX - circle.Width / 2);
				Canvas.SetTop(circle, circleCenterY - circle.Height / 2);
				LoadingSpinnerCanvas.Children.Add(circle);

				// Opacityのアニメーションを作成
				DoubleAnimation opacityAnimation = new DoubleAnimation();
				opacityAnimation.From = 1.0;
				opacityAnimation.To = -0.3;
				opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
				opacityAnimation.AutoReverse = true;
				opacityAnimation.RepeatBehavior = RepeatBehavior.Forever;

				// BeginTimeを割り当てて、順番にOpacityが変化するようにする
				opacityAnimation.BeginTime = TimeSpan.FromSeconds(i * 0.125);
				circle.BeginAnimation(Ellipse.OpacityProperty, opacityAnimation);

				// 次の円を配置する角度を更新
				angle += 360 / CircleCount;
			}

		}
	}
}
			

