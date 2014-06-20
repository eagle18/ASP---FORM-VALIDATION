using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Text;
namespace DeliveryServiceVisionet
{
    using  System.Web.Mvc;
    public class FormValidation
    {
        OrderedDictionary check = new OrderedDictionary();
        public FormValidation()
        {
            
        }
        public ModelStateDictionary validate(string page,FormCollection form)
        {
            ModelStateDictionary state =  new ModelStateDictionary();
            if (check.Contains(page))
            {
                OrderedDictionary pageCheck = (OrderedDictionary)check[page];
                System.Collections.ICollection keyTemp = pageCheck.Keys;
                string[] keys = new string[pageCheck.Keys.Count];
                keyTemp.CopyTo(keys,0);
                foreach(string key in keys){
                    OrderedDictionary checking = (OrderedDictionary)pageCheck[key];

                    string[] rules = checking["rule"].ToString().Split('|');
                    foreach(string rule in rules){
                        if (validation(rule, form[key]) == false)
                        {
                            state.AddModelError(key, checking["message"].ToString());
                            break;
                        }
                    }
                }
            }
            return state;
        }
        private bool validation(string rule, string value)
        {
            if (rule == "required")
            {
                if (value == null || value.Equals("")) return false;
                return true;
            }
            else if(rule == "email" ){
                try
                {
                    System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(value);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            else if (rule.Contains("array"))
            {
                if (value != null && value.ToString().Split(',').Length > 0)
                    return true;
                else
                    return false;
            }
            else if(rule.Equals("numeric")){
                int number = 0;
                return int.TryParse(value, out number);
            }
            else if(rule.Equals("double")){
                decimal number = 0;
                return decimal.TryParse(value, out number);
            }
            return true;
        }

    }
}
