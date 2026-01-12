using ListaZakupow.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ListaZakupow.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<Category> Categories { get; set; } = new();

        private string _newProductName = string.Empty;
        public string NewProductName
        {
            get => _newProductName;
            set => SetProperty(ref _newProductName, value);
        }

        private string _newProductUnit = string.Empty;
        public string NewProductUnit
        {
            get => _newProductUnit;
            set => SetProperty(ref _newProductUnit, value);
        }

        private string _newCategoryName = string.Empty;
        public string NewCategoryName
        {
            get => _newCategoryName;
            set => SetProperty(ref _newCategoryName, value);
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "shopping_list_v2.json");

        public MainViewModel()
        {
            LoadList();

            if (!Categories.Any())
            {
                var nabial = new Category { Name = "Nabiał", IsExpanded = true };
                nabial.Products.Add(new Product { Name = "Mleko", Unit = "l", Quantity = 1 });
                nabial.Products.Add(new Product { Name = "Ser", Unit = "kg", Quantity = 0.5 });
                
                var warzywa = new Category { Name = "Warzywa" };
                warzywa.Products.Add(new Product { Name = "Pomidory", Unit = "kg", Quantity = 2 });

                Categories.Add(nabial);
                Categories.Add(warzywa);
                
                SelectedCategory = nabial;
            }

            Categories.CollectionChanged += Categories_CollectionChanged;
            foreach (var category in Categories)
            {
                WatchCategory(category);
            }
        }

        public bool AddCategory()
        {
            if (string.IsNullOrWhiteSpace(NewCategoryName))
                return false;

            if (Categories.Any(c => string.Equals(c.Name, NewCategoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            var newCategory = new Category { Name = NewCategoryName };
            Categories.Add(newCategory);
            NewCategoryName = string.Empty;
            SaveList();
            return true;
        }

        public void AddProduct()
        {
            if (SelectedCategory != null && !string.IsNullOrWhiteSpace(NewProductName) && !string.IsNullOrWhiteSpace(NewProductUnit))
            {
                var newProduct = new Product { Name = NewProductName, Unit = NewProductUnit, Quantity = 1 };
                SelectedCategory.Products.Add(newProduct);
                NewProductName = string.Empty;
                NewProductUnit = string.Empty;
            }
        }

        public void DeleteProduct(Product? product)
        {
            if (product == null) return;
            foreach (var category in Categories)
            {
                if (category.Products.Contains(product))
                {
                    category.Products.Remove(product);
                    break;
                }
            }
        }

        public void IncreaseQuantity(Product? product)
        {
            if (product != null)
            {
                product.Quantity++;
            }
        }

        public void DecreaseQuantity(Product? product)
        {
            if (product != null && product.Quantity > 0)
            {
                product.Quantity--;
            }
        }

        private void SaveList()
        {
            var json = JsonSerializer.Serialize(Categories);
            File.WriteAllText(_filePath, json);
        }

        private void LoadList()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    var json = File.ReadAllText(_filePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var loadedCategories = JsonSerializer.Deserialize<ObservableCollection<Category>>(json);
                        if (loadedCategories != null)
                        {
                            Categories = loadedCategories;
                        }
                    }
                }
                catch (JsonException)
                {
                }
            }
        }

        private void SortProductsInCategory(Category category)
        {
            var sortedProducts = category.Products.OrderBy(p => p.IsPurchased).ToList();
            for (int i = 0; i < sortedProducts.Count; i++)
            {
                var productToMove = sortedProducts[i];
                var oldIndex = category.Products.IndexOf(productToMove);
                if (oldIndex != i)
                {
                    category.Products.Move(oldIndex, i);
                }
            }
            SaveList();
        }

        private void WatchCategory(Category category)
        {
            category.PropertyChanged += Category_PropertyChanged;
            category.Products.CollectionChanged += Products_CollectionChanged;
            foreach (var product in category.Products)
            {
                product.PropertyChanged += Product_PropertyChanged;
            }
        }

        private void UnwatchCategory(Category category)
        {
            category.PropertyChanged -= Category_PropertyChanged;
            category.Products.CollectionChanged -= Products_CollectionChanged;
            foreach (var product in category.Products)
            {
                product.PropertyChanged -= Product_PropertyChanged;
            }
        }

        private void Product_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Product.IsPurchased) && sender is Product product)
            {
                var category = Categories.FirstOrDefault(c => c.Products.Contains(product));
                if (category != null)
                {
                    SortProductsInCategory(category);
                }
            }
            SaveList();
        }
        
        private void Categories_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Category category in e.NewItems)
                {
                    WatchCategory(category);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Category category in e.OldItems)
                {
                    UnwatchCategory(category);
                }
            }
            SaveList();
        }

        private void Products_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Product product in e.NewItems)
                {
                    product.PropertyChanged += Product_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Product product in e.OldItems)
                {
                    product.PropertyChanged -= Product_PropertyChanged;
                }
            }
            SaveList();
        }

        private void Category_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Category.Name))
            {
                SaveList();
            }
        }

    }
}
