﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using BUS;
using System.Globalization;

namespace Karaoke
{
    public partial class F_Karaoke : MetroFramework.Forms.MetroForm
    {
        public F_Karaoke()
        {
            InitializeComponent();
            
            Load_Room();
            Load_Kind();
           

        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        void Load_Kind()
        {
            List<DTO.Kind_DTO> listKind = Kind_BUS.Getkinds();
            int TableWidth = 125;
            int TableHeight = 70;

            //MessageBox.Show(listKind.Count.ToString());

            foreach (DTO.Kind_DTO item in listKind)
            {
                MetroFramework.Controls.MetroTile tit_kind = new MetroFramework.Controls.MetroTile() { Width = TableWidth, Height = TableHeight };
                flowLayoutPanel_Kind.Controls.Add(tit_kind);
                tit_kind.Theme = MetroFramework.MetroThemeStyle.Light;
                tit_kind.UseTileImage = true;
                tit_kind.Style = MetroFramework.MetroColorStyle.Green;
                tit_kind.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                //tit_kind.TileImage = global::Karaoke.Properties.Resources.wine;
                tit_kind.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
                tit_kind.Text = item.Name;

                tit_kind.Click += tit_kind_Click;
                tit_kind.Tag = item;

               
            }
        }

        void Load_Menu_By_ID(int id)
        {
            flowLayoutPanel_item.Controls.Clear();
            List<DTO.Menu_DTO> listMenu = menu_BUS.GetMenus(id);
            int TableWidth = 125;
            int TableHeight = 100;

            foreach (DTO.Menu_DTO item in listMenu)
            {
                MetroFramework.Controls.MetroTile tit_Item = new MetroFramework.Controls.MetroTile() { Width = TableWidth, Height = TableHeight };
                flowLayoutPanel_item.Controls.Add(tit_Item);
                tit_Item.Theme = MetroFramework.MetroThemeStyle.Light;
                tit_Item.UseTileImage = true;
                tit_Item.Style = MetroFramework.MetroColorStyle.Teal;
                tit_Item.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                tit_Item.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
                tit_Item.Text = item.Name + Environment.NewLine + "Amout: "+ item.Amout;

                tit_Item.Click += tit_Item_Click;
                tit_Item.Tag = item;
            }
        }

       

        void Load_Room()
        {
            flowLayoutPanel_ROOM.Controls.Clear();
            List<DTO.RoomDTO> listroom = RoomBUS.GetRooms();
            int TableWidth = 175;
            int TableHeight = 175;

            foreach (DTO.RoomDTO item in listroom)
            {
                MetroFramework.Controls.MetroTile titRoom = new MetroFramework.Controls.MetroTile() { Width = TableWidth, Height = TableHeight };
                flowLayoutPanel_ROOM.Controls.Add(titRoom);
                titRoom.Theme = MetroFramework.MetroThemeStyle.Light;

                titRoom.UseTileImage = true;
                titRoom.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                titRoom.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;

                


                //Button Btn = new Button() { Width = TableWidth, Height = TableHeight };
                //flowLayoutPanel_Room.Controls.Add(Btn);

                string stt = "";
                if (item.Status == 0)
                {
                    stt = "Available";
                }
                else if (item.Status == 1)
                {
                    stt = "Active";
                }
                else
                    stt = "Fixing";

                titRoom.Text = item.Name + Environment.NewLine + stt;
               


                titRoom.Click += titRoom_Click;
                titRoom.Tag = item; //luu cai room cua minh vo tag, no la kiiu du lieu obj

                switch (item.Status)
                {
                    case 0:
                        titRoom.Style = MetroFramework.MetroColorStyle.Green;
                        titRoom.TileImage = global::Karaoke.Properties.Resources.room;
                        break;
                    case 2:
                        titRoom.Style = MetroFramework.MetroColorStyle.Silver;
                        titRoom.TileImage = global::Karaoke.Properties.Resources.fix;
                        titRoom.Enabled = false;
                        break;
                    default:
                        titRoom.Style = MetroFramework.MetroColorStyle.Orange;
                        titRoom.TileImage = global::Karaoke.Properties.Resources.kara;
                        break;
                }

                if (item.Kind_Of_Room == 1)
                {
                    titRoom.TileImage = global::Karaoke.Properties.Resources.vip;
                }
            }
        }

        private float roomFeePerHour;

        public void LoadItemOfRoom(int id) //hien hoa don cua room nao do
        {
            //lay ra cai bill tu cai room hien tai chua dc check out, tu do lay ra ds bill info
            //List<DTO.ChiTietHoaDon> chitiethoadon = DAO.ChiTietHoaDonDAO.Instance1.GetListChiTietHoaDon(DAO.HoaDonDAO.Instance1.GetHoaDonChuaThanhToanTheoIDPhong(stt));

            listViewItem.Items.Clear();
            

            List<DTO.itemInBill_DTO> listItem = BUS.itemInBill_BUS.GetItemOfRoom(id);

            float serviceFee = 0;

            foreach (DTO.itemInBill_DTO item in listItem)
            {
                ListViewItem lvit = new ListViewItem(item.Name.ToString());
                lvit.SubItems.Add(item.Amout.ToString());
                lvit.SubItems.Add(item.Price.ToString());
                lvit.SubItems.Add(item.ToMoney.ToString());
                serviceFee = serviceFee + item.ToMoney;

                listViewItem.Items.Add(lvit);
                //listViewItem.Tag = item;
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            //textBoxTongTien.Text = Tongtienthucan.ToString("c", culture);

            metroTextBox_ServiceFee.Text = serviceFee.ToString("c", culture);
            metroTextBox_ServiceFee.Enabled = false;



            metroTextBox_timeIn.Clear();
            metroTextBox_timeIn.Enabled = false;
            metroTextBox_roomFee.Clear();
            metroTextBox_roomFee.Enabled = false;
            metroTextBox_VAT.Clear();
            metroTextBox_VAT.Enabled = false;
            metroTextBox_min.Clear();
            metroTextBox_min.Enabled = false;
            metroTextBox_discount.Clear();
            metroTextBox_discount.Enabled = false;
            metroTextBox_percentDiscount.Clear();
            

            //float FeePerHour = 2; // 120k / h -> 
            float roomFee = 0;
            List<DTO.Bill_DTO> getTime = bill_BUS.GetBillByIdRoom(id);
            foreach (DTO.Bill_DTO time in getTime)
            {
                metroTextBox_min.Text = time.TimeUse.ToString() + " min";
                metroTextBox_timeIn.Text = time.TimeIn.ToString();
                int minute = int.Parse(time.TimeUse.ToString());
                roomFee = (float) (minute/0.6) * roomFeePerHour; // 1phut = 2k => 1h = 120k        
            }
            metroTextBox_roomFee.Text = roomFee.ToString("c", culture);

            float VAT = (float) ((serviceFee + roomFee) * 0.1);
            metroTextBox_VAT.Text = VAT.ToString("c", culture);

            metroTextBox_totalMoney.Clear();
            metroTextBox_totalMoney.Enabled = false;

            //float surcharge = 0;
            //if (metroTextBox_Surcharge.Text == "")
            //    surcharge = 0;

            totalMoneyNonDiscount = roomFee + serviceFee + VAT ;
            
            metroTextBox_totalMoney.Text = totalMoneyNonDiscount.ToString("c", culture);

        }

        
        private float totalMoneyNonDiscount;
        private float totalMoneyFinal;

        private int idItem;
        private string nameItem;
        
        private void MetroButton_addItem_Click(object sender, EventArgs e)
        {
            DTO.RoomDTO room = listViewItem.Tag as DTO.RoomDTO; //lay ra table hien tai
            //MessageBox.Show(room.IdRoom.ToString());
            int idBill = BUS.bill_BUS.GetBillNotPaymentByIdRoom(room.IdRoom);
            int idMenu = idItem; //testing
            int count = (int)numericUpDown_Count.Value;

            if (idBill == -1) //chuaco bill nao het
            {
                BUS.bill_BUS.AddBill(room.IdRoom);
                BUS.billInfo_BUS.AddBillInfo(BUS.bill_BUS.getMaxIdBill(), idMenu, count);
               
            }
            else
            {
                
                BUS.billInfo_BUS.AddBillInfo(idBill, idMenu, count);

            }
            //MetroFramework.MetroMessageBox.Show(this, "test");
            if(count > 0)
                MessageBox.Show(nameItem.ToString() + " was added to " + room.Name.ToString() + "!");
            else if(count == 0)
                MessageBox.Show("Please input number of " + nameItem.ToString() + " do you want to add");
            else
                MessageBox.Show(count.ToString() + " " + nameItem.ToString() + " in " + room.Name.ToString() + " successfully!");
            LoadItemOfRoom(room.IdRoom);
            Load_Room();
        }
        
    


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroLabel3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        

        public void tit_kind_Click(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroTile tit = sender as MetroFramework.Controls.MetroTile;
            int idKind = (tit.Tag as Kind_DTO).Id;       
            Load_Menu_By_ID(idKind);
            string title = ((sender as MetroFramework.Controls.MetroTile).Tag as Kind_DTO).Name;
            metroLabel_tit.Text = "List of " + title;
        }

        private void metroScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void metroLabel2_Click(object sender, EventArgs e)
        {

        }

        private void metroButton_Payment_Click(object sender, EventArgs e)
        {
            DTO.RoomDTO room = listViewItem.Tag as RoomDTO;
            int idBill = BUS.bill_BUS.GetBillNotPaymentByIdRoom(room.IdRoom);

            //double totalMoney = Convert.ToDouble(metroTextBox_totalMoney.Text.Split(',')[0]);

            if (idBill != -1) //neu chua co j trog hd thi k lam gi het
            {
                //MetroFramework.MetroMessageBox.Show(this, "\n\n\n\n\n\\n\n\nContinue Logging Out?", "ROOM CHECK OUT", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (MessageBox.Show("Do you want to stop " + room.Name + " and print bill?", "Room Checkout", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BUS.bill_BUS.Payment(idBill);
                    Load_Menu_By_ID(room.IdRoom);
                    Load_Room();
                    
                }

               
            }
            else
            {
                MessageBox.Show("???");
            }

            
        }

        private void titRoom_Click(object sender, EventArgs e)
        {
            int idRoom = ((sender as Button).Tag as DTO.RoomDTO).IdRoom;
            roomFeePerHour = ((sender as Button).Tag as DTO.RoomDTO).Price;

            MessageBox.Show(roomFeePerHour.ToString());
            listViewItem.Tag = (sender as Button).Tag;
            LoadItemOfRoom(idRoom);
        }

        private void tit_Item_Click(object sender, EventArgs e)
        {
            idItem = ((sender as Button).Tag as DTO.Menu_DTO).Id;
            nameItem = ((sender as Button).Tag as DTO.Menu_DTO).Name;
            //listViewItem.Tag = (sender as Button).Tag;
            //MessageBox.Show(idItem.ToString());

        }



        private void metroTextBox_percentDiscount_TextChanged(object sender, EventArgs e)
        {
            CultureInfo culture = new CultureInfo("vi-VN");

            float percent = 0;

            if (metroTextBox_percentDiscount.Text == "")
                percent = 0;
            else
                percent = float.Parse(metroTextBox_percentDiscount.Text);


            float Discount = totalMoneyFinal = totalMoneyNonDiscount - (float)((totalMoneyNonDiscount * percent ) / 100);
            metroTextBox_discount.Text = (totalMoneyNonDiscount - Discount).ToString("c", culture); ;
            
            metroTextBox_totalMoney.Text = Discount.ToString("c", culture);
        }

        private void metroTile_BACK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}