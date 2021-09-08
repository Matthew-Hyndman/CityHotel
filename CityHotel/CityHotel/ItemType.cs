using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVal;

namespace CityHotel
{
    class ItemType
    {
        private int itemTypeID;
        private string itemDesc;

        public ItemType()
        {
            this.itemTypeID = 0;
            this.itemDesc = "";
        }

        public ItemType(int itemTypeID, string itemDesc)
        {
            this.itemTypeID = itemTypeID;
            this.itemDesc = itemDesc;
        }

        public int ItemTypeID
        {
            get { return itemTypeID; }
            set { itemTypeID = value; }
        }

        public string ItemDesc
        {
            get { return itemDesc; }
            set
            {
                if (!value.Equals("") && !MyEx.numVal(value))
                    itemDesc = MyEx.capAllLet(value);

                else if (value == "")
                    throw new MyEx("Plese enter a value!");

                else if (MyEx.valLength(value, 2, 25))
                    throw new MyEx("There can only be 2 to 25");

                else if (MyEx.numVal(value))
                    throw new MyEx("The value can only be letters");

            }
        }
    }
}
