using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Grafika4
{

     public class ViewModel : INotifyPropertyChanged
     {
          private double viewHeight;

          private double viewWidth;

          public ViewModel()
          {
               ViewHeight = Constants.height;
               ViewWidth = Constants.width;
          }

          public event PropertyChangedEventHandler PropertyChanged;

          public double ViewHeight
          {
               get => viewHeight;
               set
               {
                    if (value.Equals(viewHeight))
                    {
                         return;
                    }

                    viewHeight = value;
                    OnPropertyChanged();
               }
          }

          public double ViewWidth
          {
               get => viewWidth;
               set
               {
                    if (value.Equals(viewWidth))
                    {
                         return;
                    }

                    viewWidth = value;
                    OnPropertyChanged();
               }
          }
          protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
          {
               PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
          }
     }

}