using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace inventory_control
{
    public partial class UserCreationForm : DevExpress.XtraEditors.XtraForm
    {
        clsGlobalValue InvclsGlobalValue = new clsGlobalValue();
        clsUserCreation InvclsUserCreate = new clsUserCreation();
        DataAccessLayer InvDataAccessLayer = new DataAccessLayer();
        clsTools InvTools = new clsTools();

        public UserCreationForm()
        {
            InitializeComponent();
        }

        private void CmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (AcctPeriodDt1.Text.Trim() == "")
                //{
                //    MessageBox.Show("Please Enter StartDate");
                //    AcctPeriodDt1.Focus();
                //}

                //if (AcctPeriodDt2.Text.Trim() == "")
                //{
                //    MessageBox.Show("Please Enter EndDate");
                //    AcctPeriodDt2.Focus();
                //}

                //if (AcctPeriodDt1.Text.Trim() != "" && AcctPeriodDt2.Text.Trim() != "")
                //{
                int usrgrpid = int.Parse(lookUpEditUserGroup.GetColumnValue("UserGroupID").ToString());
                
                string msg = "Do You Want To Save?";
                DialogResult result = MessageBox.Show(this, msg, "Save Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        InvclsUserCreate.UserName = InvTools.formatInputString(txtSupplierName.Text.Trim());
                        InvclsUserCreate.Password = InvTools.formatInputString(txtAddress1.Text.Trim());
                        InvclsUserCreate.FirstName = InvTools.formatInputString(txtPanNo.Text.Trim());
                        InvclsUserCreate.LastName = InvTools.formatInputString(TxtCSTNo.Text.Trim());
                        InvclsUserCreate.Address = InvTools.formatInputString(TxtAddress.Text);
                        InvclsUserCreate.ContactNo = InvTools.formatInputString(txtContactNo.Text.Trim());
                        InvclsUserCreate.UserGroupID = usrgrpid;
                        InvclsUserCreate.Status = true;
                        
                        InvclsUserCreate.Mode = "Insert";

                        int i = InvclsUserCreate.UpdateData();

                        if (i > 0)
                        {
                            MessageBox.Show("New User Created Successfully.");
                        }
                        else
                        {
                            if (i == -1)
                            {
                                MessageBox.Show("Sorry!!! Duplicate UserName Found.");
                            }
                            else
                            {
                                MessageBox.Show("Error In Saving!!!!!!!!!!!");
                            }
                        }
 
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("Error In Saving !!!!!!!!!!");
                        }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void UserCreationForm_Load(object sender, EventArgs e)
        {
            string strSQL = "select * from tbl_UserGroup where Status=1";
            SqlDataAdapter InvDataAdapter = new SqlDataAdapter();
            InvDataAdapter = InvDataAccessLayer.PopulateData(strSQL);

            InvDataAdapter.Fill(dsPopulateData1.tbl_UserGroup);
            //        SetWaitDialogCaption("Loading User Groups...");
            //oleDBAdapter4.Fill(dsCategories1.Suppliers);
        }

    }
}