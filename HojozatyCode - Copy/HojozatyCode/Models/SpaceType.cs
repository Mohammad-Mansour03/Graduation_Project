using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HojozatyCode.Models
{
    public partial class SpaceType : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private bool isSelected;
    }
}
