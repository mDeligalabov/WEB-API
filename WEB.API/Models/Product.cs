using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBProducts.WEB.API.Models
{
    public class Product
    {
        private String model;
        private String type;
        private String threedModel;
        private List<ProductData> dataList;
        private int id;

        public Product(string model, string type, string threedModel, List<ProductData> dataList, int id)
        {
            this.model = model;
            this.type = type;
            this.threedModel = threedModel;
            this.dataList = dataList;
            this.id = id;
        }

        
        public string Model
        {
            get { return model; }
        }

        public string Type
        {
            get { return type; }
        }


        public string ThreeDModel
        {
            get { return threedModel; }
        }

        public List<ProductData> DataList
        {
            get { return dataList; }
        }

        public int Id
        {
            get { return id; }
        }
    }
}