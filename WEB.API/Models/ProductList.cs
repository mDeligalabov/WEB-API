using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace HBProducts.WEB.API.Models
{
    public class ProductList
    {
        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        public ProductList()
        {
            products = new ObservableCollection<Product>();
        }

        public ObservableCollection<Product> Products
        {
            get { return products; }
        }

        //Methods to implement commands to manipulate the product list...
        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

    }
}