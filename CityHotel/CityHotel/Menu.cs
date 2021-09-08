using MyVal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHotel
{
    class Menu
    {

        private int itemId, itemTypeId;
        private double price;
        private string menuItemDesc;

        public Menu()
        {
            itemId = 0;           
            menuItemDesc = "";
            price = 0;
            itemTypeId = 0;
        }

        public Menu(int itemId, string menuItemDesc, double price, int itemTypeId)
        {
            this.itemId = itemId;
            this.menuItemDesc = menuItemDesc;
            this.price = price;
            this.itemTypeId = itemTypeId;
        }

        public int ItemID
        {
            get { return itemId; }
            set { itemId = value; }            
        }

        public string MenuItemDesc
        {
            get { return menuItemDesc; }
            set
            {
                if (MyEx.valLength(value, 3, 40))                
                        menuItemDesc = MyEx.capAllLet(value);

                else if (value == "")
                    throw new MyEx("Plese enter a value!");

                else if (!MyEx.valLength(value, 3, 40))
                    throw new MyEx("This Discription must at lest \nhave uo to 3 to 40 letters.");
            }
        }

        public double Price
        {
            get { return price; }
            set
            {
               
                if (!MyEx.numVal(value.ToString()))
                    throw new MyEx("This field can only accept numbers");


                else if (MyEx.val_Whi_Let(value.ToString()))
                    throw new MyEx("Plese enter a value!");

                else if (MyEx.numVal(value.ToString()))
                    price = value;
            }
        }

        public int ItemTypeID
        {
            get { return itemTypeId; }
            set
            {
                if(!value.ToString().Equals(""))
                itemTypeId = value;

               else if (value.ToString() == "")
                    throw new MyEx("Plese enter a value!");
                
            }
        }
    }
}
