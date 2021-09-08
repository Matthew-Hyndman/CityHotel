using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVal
{
    class MyEx: Exception
    {
        private String message;
        //                              !!!!!
        public MyEx(String m)
        {
            this.message = m;
        }

        public String toString()
        {
            return String.Format("Error: {0}", message);
        }

        //length Validation
        public static bool valLength(String str, int min, int max)
        {
            bool ok = true;

            if (String.IsNullOrEmpty(str))
            {
                ok = false;
            }

            else if (str.Length < min || str.Length > max)
            {
                ok = false;
            }

            return ok;
        }


        //number validation
        public static bool numVal(String str)
        {
            bool ok = true;

            for (int i = 0; i < str.Length; i++)
            {
                if (!(char.IsNumber(str[i])) && str[i] != '.')
                {
                    ok = false;
                }
            }

            return ok;
        }

        public static bool numVal2(String str)
        {
            bool ok = true;

            for (int i = 0; i < str.Length; i++)
            {
                if (!(char.IsNumber(str[i])) && str[i] == '.')
                {
                    ok = false;
                }
            }

            return ok;
        }


        //letter validation
        public static bool letVal(String str)
        {
            bool ok = true;

            if (str.Trim().Length == 0)
            {
                ok = false;
            }

            else
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!(char.IsLetter(str[i])))
                    {
                        ok = false;
                    }
                }
            }
            return ok;
        }


        //whiteSpace and Letter Validation
        public static bool val_Whi_Let(String str)
        {
            bool ok = true;

            if (str.Trim().Length == 0)
            {
                ok = false;
            }

            else
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!(char.IsLetter(str[i])) && !(char.IsWhiteSpace(str[i])))
                    {
                        ok = false;
                    }
                }
            }
            return ok;
        }

        public static bool valPsotcode(String str)
        {
            bool ok = true;

            if (str.Trim().Length == 0)
            {
                ok = false;
            }

            else
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!(char.IsLetter(str[i])) && !(char.IsWhiteSpace(str[i])) && !(str[i].Equals("-")))
                    {
                        ok = false;
                    }
                }
            }
            return ok;
        }

        public static bool val_Dog_DOB(String str)
        {
            bool ok = true;

            DateTime curDate = DateTime.Now;
            DateTime dogDate = Convert.ToDateTime(str);

            TimeSpan t = curDate - dogDate;
            double noOfDays = t.TotalDays;

            if (str.Trim().Length == 0)
            {
                ok = false;
            }

            else if (noOfDays <= 56)
            {
                ok = false;
            }

            return ok;
        }

        public static String capAllLet(String str)
        {
            char[] arr = str.ToCharArray();

            if (char.IsLower(arr[0]))
            {
                arr[0] = char.ToUpper(arr[0]);
            }

            for (int i = 1; i < str.Length; i++)
            {
                if (arr[i - 1] == ' ')
                {
                    if (char.IsLower(arr[i]))
                        arr[i] = char.ToUpper(arr[i]);
                }

                else
                    arr[i] = char.ToLower(arr[i]);

            }

            return new String(arr);
        }

        //val PostCode
        public static bool valPostcode(String str)
        {
            bool ok = true;

            if (String.IsNullOrEmpty(str))
            {
                ok = false;
            }

            if (str.Length < 7 || str.Length > 8)
            {
                ok = false;
            }

            else
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!(char.IsUpper(str[i])) || !(str[i].Equals("-")))
                    {
                        char.ToUpper(str[i]);
                    }
                }
            }

            for (int i = 0; i < str.Length; i++)
            {
                if (i == 4)
                {
                    if (!char.IsWhiteSpace(str[i]))
                    {
                        ok = false;
                    }
                }

                else
                {
                    ok = false;
                }
            }
            return ok;
        }

    }
}


    

