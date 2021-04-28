using CheckersV4.Services;
using System.Windows.Input;
using System.Windows.Media;

namespace CheckersV4.ViewModels
{
    public class CellVM : BaseVM
    {

        private SolidColorBrush background;
        public SolidColorBrush Background
        {
            get => background;
            set => SetProperty(ref background, value);
        }


        private Location location;

        public Location Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public CellVM()
        {
        }


        public CellVM(int i = 0, int j = 0, SolidColorBrush background = null)
        {
            Location = new Location(i, j);
            if (background != null)
            {
                Background = background;
            }
            else
            {
                Background = Brushes.Transparent;
            }
            isSelected = false;
        }

        private ICommand selectTileCommand;
        public ICommand SelectTileCommand
        {
            get
            {
                if (selectTileCommand == null)
                {
                    selectTileCommand = new Commands.SelectTileCommand(this);
                }
                return selectTileCommand;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            CellVM cell = obj as CellVM;
            if (cell == null)
            {
                return false;
            }

            return Location == cell.Location;
        }
    }

}

