using System;
using System.Collections.Generic;
using Memories.Model;
using Memories.Network;
using Xamarin.Forms;

namespace Memories
{
    public partial class Profile : ContentPage
    {
        //public Profile()
        //{
        //    InitializeComponent();
        //    onLockTag(false);
        //    UserDetail userDetail = new UserDetail("#November2623", "Duy", "Tran Ngoc",
        //        "26-12-1998", "Male", "Single", "0982257974", "HoChiMinhCity");
        //    BindingContext = userDetail;
        //}
        //public async void onEdit(Object sender, EventArgs e)
        //{
        //    onLockTag(enableEdit.IsToggled);
        //}

        //public async void onLockTag(bool isEnable)
        //{
        //    firstName.IsEnabled = isEnable;
        //    lastName.IsEnabled = isEnable;
        //    PickerGender.IsEnabled = isEnable;
        //    PickerDate.IsEnabled = isEnable;
        //    PickerRelationship.IsEnabled = isEnable;
        //    phone.IsEnabled = isEnable;
        //    address.IsEnabled = isEnable;
        //}
        public DataService dataService = new DataService();
        public bool checkEnable { get; set; }
        public string stt { get; set; }
        public String image;
        public String url_avatar;
           
        public Profile()
        {
            InitializeComponent();
            InitProfile();
            lb_status.Text = "Edit";

        }

        public async void InitProfile()
        {
            UserDetail userDetail = await dataService.GetUser();
            //UserDetail userDetail = new UserDetail("#November2623", null, null,
            //    null,null, null, null, null);
            image = userDetail.avatar;
            userDetail.avatar = Global.IMAGE + "/" + userDetail.avatar;
            BindingContext = userDetail;
        }

        void OnToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                lb_status.Text = "Save";
            }
            else
            {
                lb_status.Text = "Edit";
                String time = dateOfBirth.Date.ToString("yyyy-MM-d");
               
                UserDetail userDetail = new UserDetail(image,
                    username.Text, firstName.Text, lastName.Text, time, gender.SelectedItem.ToString(), relationship.SelectedItem.ToString(), phone.Text, address.Text 
                    );
                String url = userDetail.avatar;
                userDetail.avatar = Global.IMAGE + "/" + userDetail.avatar;
                BindingContext = userDetail;
                userDetail.avatar = url;
                dataService.UpdateProfile(userDetail);
            }

        }
    }
}
