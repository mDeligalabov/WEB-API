using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBProducts.WEB.API.Models
{
    public class ProductData
    {
        private String dataType;
        private String dataValue;
        private Boolean isUrl;

        public ProductData(String dataType, String dataValue, Boolean isUrl)
        {
            this.dataType = dataType;
            this.dataValue = dataValue;
            this.isUrl = isUrl;
        }

        public String GetType()
        {
            return dataType;
        }

        public String GetValue()
        {
            return dataValue;
        }

        public String Value
        {
            get { return dataValue; }
        }

        public String Type
        {
            get { return dataType; }
        }

        public Boolean IsUrl
        {
            get { return isUrl; }
        }
    }
}