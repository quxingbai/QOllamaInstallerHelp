using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace QOllamaInstallerHelp.Views.CPanels
{
    public class PagePanel : Grid
    {
        private UIElement? NowDisplayElement { get; set; } = null;
        public PagePanel()
        {
            //Task.Run(() =>
            //{
            //    Task.Delay(1000).Wait();
            //    Dispatcher.Invoke(() =>
            //    {
            //        NextPage();
            //    });
            //});
        }
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (visualAdded is UIElement u)
            {
                if (Children.Count ==1)
                {
                    u.Visibility = Visibility.Visible;
                    this.NowDisplayElement = u;
                }
                else
                {
                    u.Visibility = Visibility.Collapsed;
                }
            }
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }
        public void NextPage()
        {
            if (Children.Count == 0) return;
            int NowIndex = NowDisplayElement == null ? -1 : Children.IndexOf(NowDisplayElement);
            int NextIndex = Children.Count == 0 ? -1 : NowIndex == -1 ? 0 : Children.Count == NowIndex + 1 ? 0 : NowIndex + 1;
            if (NextIndex != -1)
            {
                NowDisplayElement = Children[NextIndex];
                ShowPageChangeAnimation(NowIndex == -1 ? null : Children[NowIndex], NowDisplayElement);
            }
        }
        private void ShowPageChangeAnimation(UIElement Old, UIElement New)
        {
            New.Visibility = Visibility.Visible;
            if (Old != null)
            {
                var anima = CreateAnimation(Old, new(0, 0), new(0, -500), 1, 0);
                anima.Completed += HidenAniamtion_Completed;
                anima.Begin();
                void HidenAniamtion_Completed(object? sender, EventArgs e)
                {
                    anima.Completed -= HidenAniamtion_Completed;
                    if (this.NowDisplayElement != Old)
                    {
                        Old.Visibility = Visibility.Collapsed;
                    }
                }
            }
            if (New != null)
            {
                var anima2 = CreateAnimation(New, new(0, 500), new(0, 0), 0, 1);
                anima2.Begin();
            }
        }


        private Storyboard CreateAnimation(UIElement Element, Point FromPoint, Point ToPoint, double FromOpacity, double ToOpacity)
        {
            Duration durtion = new(TimeSpan.Parse("0:0:0.25"));
            Storyboard sb = new();
            var translate = GetOrCreateElementTransform<TranslateTransform>(Element);
            {
                DoubleAnimation Tx = new DoubleAnimation(FromPoint.X, ToPoint.X, durtion);
                Storyboard.SetTarget(Tx, translate);
                Storyboard.SetTargetProperty(Tx, new(TranslateTransform.XProperty.Name));
                DoubleAnimation Ty = new DoubleAnimation(FromPoint.Y, ToPoint.Y, durtion);
                Storyboard.SetTarget(Ty, translate);
                Storyboard.SetTargetProperty(Ty, new(TranslateTransform.YProperty.Name));
                translate.BeginAnimation(TranslateTransform.XProperty, Tx);
                translate.BeginAnimation(TranslateTransform.YProperty, Ty);
            }
            {
                DoubleAnimation Oa = new(FromOpacity, ToOpacity, durtion);
                Storyboard.SetTargetProperty(Oa, new(OpacityProperty));
                Storyboard.SetTarget(Oa, Element);
                sb.Children.Add(Oa);
            }
            return sb;
        }
        private T GetOrCreateElementTransform<T>(UIElement Element) where T : Transform, new()
        {
            TransformGroup group = null;
            if (Element.RenderTransform is TransformGroup isTG)
            {
                group = isTG;
            }
            else
            {
                Element.RenderTransform = (group = new TransformGroup());
            }
            foreach (var i in group.Children)
            {
                if (i is T isT) return isT;
            }
            var df = new T();
            group.Children.Add(df);
            return df;
        }
    }
}
