using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace ListaZakupow.Models
{
    public class Product : ObservableObject
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _unit = string.Empty;
        public string Unit
        {
            get => _unit;
            set => SetProperty(ref _unit, value);
        }

        private double _quantity;
        public double Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        private bool _isPurchased;
        public bool IsPurchased
        {
            get => _isPurchased;
            set
            {
                if (SetProperty(ref _isPurchased, value))
                {
                    OnPropertyChanged(nameof(Strikethrough));
                }
            }
        }

        public TextDecorations Strikethrough => IsPurchased ? TextDecorations.Strikethrough : TextDecorations.None;
    }
}
