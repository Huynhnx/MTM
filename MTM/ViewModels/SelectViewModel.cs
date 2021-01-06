using MVVMCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM
{
    class SelectViewModel:ViewModelBase
    {
        public RelayCommand OkCmd { get; set; }
        public RelayCommand CancelCmd { get; set; }
        ObservableCollection<TestCase> listtest = new ObservableCollection<TestCase>();
        public ObservableCollection<TestCase> ListTest
        {
            get
            {
                return listtest;
            }
            set
            {
                if (listtest!= value)
                {
                    listtest = value;
                   
                    RaisePropertyChanged("ListTest");
                }
            }
        }
        bool? isselectall = true;
        public bool? IsSelectAll
        {
            get
            {
                return isselectall;
            }
            set
            {
                if (value!= isselectall)
                {
                    isselectall = value;
                    RaisePropertyChanged("IsSelectAll");
                    if(isselectall != null)
                    {
                        for (int i = 0; i < ListTest.Count; i++)
                        {
                            ListTest[i].selected = (isselectall == true) ? true : false;
                        }
                        RaisePropertyChanged("ListTest");
                    }
                    
                }
            }
        }
    }
}
